using System.Reflection;

namespace ORM;

public static class SqlCommandBuilder
{
    public static string BuildInsert(string tableName, PropertyInfo[] properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        string safeTableName = OrmContext.EscapeIdentifier(tableName);
        if (properties.Length == 0)
        {
            throw new InvalidOperationException(
                "Нет свойств для вставки.");
        }
        string columns = string.Join(", ", properties.Select(p => OrmContext.EscapeIdentifier(p.Name)));
        string parameters = string.Join(", ", properties.Select((p, i) => $"@p{i}"));
        
        string sql = $"INSERT INTO {safeTableName} ({columns}) VALUES ({parameters})";
        return sql;
    }
    
    public static string BuildUpdate(string tableName, PropertyInfo[] properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        string safeTableName = OrmContext.EscapeIdentifier(tableName);
        string setClause = string.Join(", ",
            properties.Select((property, index) =>
                $"{OrmContext.EscapeIdentifier(property.Name)} = @p{index}"));

        string sql = $"UPDATE {safeTableName} SET {setClause} WHERE id = @id";
        return sql;
    }

    public static string BuildDelete(string tableName)
    {
        string safeTableName = OrmContext.EscapeIdentifier(tableName);
        
        string sql = $"DELETE FROM {safeTableName} WHERE id = @id";
        return sql;
    }
}