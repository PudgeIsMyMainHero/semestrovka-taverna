using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace ORM;

public sealed class OrmContext : IOrmContext
{
    private readonly string _connectionString;
    /// <summary>
    /// Создаёт экземпляр контекста для работы
    /// с базой данных SQL Server.
    /// </summary>
    /// <param name="connectionString">
    /// Строка подключения к базе данных.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Строка подключения не задана.
    /// </exception>
    public OrmContext(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(
            connectionString);
 
        _connectionString = connectionString;
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync<T>(string tableName, CancellationToken cancellationToken = default) where T : class, new()
    {
        string safeTableName = EscapeIdentifier(tableName);
 
        string sql = $"SELECT * FROM {safeTableName}";
 
        var result = new List<T>();
 
        await using var connection =
            new NpgsqlConnection(_connectionString);
 
        await connection.OpenAsync(cancellationToken);
 
        await using var command =
            new NpgsqlCommand(sql, connection);
 
        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);
 
        PropertyInfo[] properties = typeof(T)
            .GetProperties(BindingFlags.Public |
                           BindingFlags.Instance)
            .Where(property => property.CanWrite &&
                               property.SetMethod?.IsPublic == true)
            .ToArray();
 
        var columns = MapColumns(
            Enumerable.Range(0, reader.FieldCount)
                .Select(index => reader.GetName(index)));
 
        while (await reader.ReadAsync(cancellationToken))
        {
            T entity = new();
 
            foreach (PropertyInfo property in properties)
            {
                if (!columns.TryGetValue(
                        property.Name, out int ordinal))
                {
                    continue;
                }
 
                object value = await reader.IsDBNullAsync(
                    ordinal, cancellationToken)
                    ? DBNull.Value
                    : reader.GetValue(ordinal);

                object convertedValue = ConvertValue(
                    value,
                    property.PropertyType);

                property.SetValue(entity, convertedValue);
            }
 
            result.Add(entity);
        }
 
        return result;
    }
    
    public static string EscapeIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier) ||
            !Regex.IsMatch(
                identifier,
                @"^[A-Za-z_][A-Za-z0-9_]*$"))
        {
            throw new ArgumentException(
                "Недопустимое имя SQL-идентификатора.",
                nameof(identifier));
        }
 
        return $"{identifier}";
    }

    public async Task<int> InsertAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public |
                           BindingFlags.Instance)
            .Where(property => property.CanRead &&
                               property.GetMethod?.IsPublic == true &&
                               !property.Name.Equals("ID", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        string sql = SqlCommandBuilder.BuildInsert(tableName, properties);
        
        await using var connection =
            new NpgsqlConnection(_connectionString);
 
        await connection.OpenAsync(cancellationToken);
 
        await using var command =
            new NpgsqlCommand(sql, connection);

        for (int i = 0; i < properties.Length; i++)
        {
            var value =  properties[i].GetValue(entity);
            command.Parameters.AddWithValue($"p{i}", value ?? DBNull.Value);
        }
        
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<int> UpdateAsync<T>(string tableName, int id, T entity, CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public |
                           BindingFlags.Instance)
            .Where(property => property.CanRead &&
                               property.GetMethod?.IsPublic == true &&
                               !property.Name.Equals("ID", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (properties.Length == 0)
            throw new InvalidOperationException("Нет свойств для обновления.");
 
        string sql = SqlCommandBuilder.BuildUpdate(tableName, properties);

        await using var connection =
            new NpgsqlConnection(_connectionString);
 
        await connection.OpenAsync(cancellationToken);
 
        await using var command =
            new NpgsqlCommand(sql, connection);

        for (int i = 0; i < properties.Length; i++)
        {
            var value =  properties[i].GetValue(entity);
            command.Parameters.AddWithValue($"p{i}", value ?? DBNull.Value);
        }
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync(string tableName, int id, CancellationToken cancellationToken = default)
    {
        string sql = SqlCommandBuilder.BuildDelete(tableName);
        
        await using var connection =
            new NpgsqlConnection(_connectionString);
 
        await connection.OpenAsync(cancellationToken);
 
        await using var command =
            new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }
    
    
    public static Dictionary<string, int> MapColumns(
        IEnumerable<string> columnNames)
    {
        ArgumentNullException.ThrowIfNull(columnNames);
        var columns = new Dictionary<string, int>(
            StringComparer.OrdinalIgnoreCase);

        int index = 0;

        foreach (string columnName in columnNames)
        {
            columns[columnName] = index;
            index++;
        }

        return columns;
    }
    
    public static object ConvertValue(object value, Type targetType)
    {
        Type actualType =
            Nullable.GetUnderlyingType(targetType)
            ?? targetType;

        if (value == null || value == DBNull.Value)
        {
            if (!targetType.IsValueType ||
                Nullable.GetUnderlyingType(targetType) != null)
            {
                return null;
            }

            return Activator.CreateInstance(targetType);
        }

        return actualType.IsInstanceOfType(value)
            ? value
            : actualType.IsEnum
                ? Enum.ToObject(actualType, value)
                : Convert.ChangeType(
                    value,
                    actualType,
                    CultureInfo.InvariantCulture);
    }
    
    public async Task<T?> GetByIdAsync<T>(
        string tableName,
        int id,
        CancellationToken cancellationToken = default)
        where T : class, new()
    {
        string safeTableName = EscapeIdentifier(tableName);

        string sql = $"SELECT * FROM {safeTableName} WHERE id = @id";

        await using var connection =
            new NpgsqlConnection(_connectionString);

        await connection.OpenAsync(cancellationToken);

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        PropertyInfo[] properties = typeof(T)
            .GetProperties(BindingFlags.Public |
                           BindingFlags.Instance)
            .Where(property => property.CanWrite &&
                               property.SetMethod?.IsPublic == true)
            .ToArray();

        var columns = MapColumns(
            Enumerable.Range(0, reader.FieldCount)
                .Select(index => reader.GetName(index)));

        if (await reader.ReadAsync(cancellationToken))
        {
            T entity = new();

            foreach (PropertyInfo property in properties)
            {
                if (!columns.TryGetValue(
                        property.Name, out int ordinal))
                {
                    continue;
                }

                object value = await reader.IsDBNullAsync(
                    ordinal, cancellationToken)
                    ? DBNull.Value
                    : reader.GetValue(ordinal);

                object convertedValue = ConvertValue(
                    value,
                    property.PropertyType);

                property.SetValue(entity, convertedValue);
            }

            return entity;
        }

        return null;
    }
}