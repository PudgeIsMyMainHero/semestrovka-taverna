using System.Reflection;

namespace ORM.UnitTests;
[TestClass]
public class SqlCommandBuilderTests
{
    
    private static PropertyInfo[] GetProperties<T>()
    {
        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "ID")
            .ToArray();
    }
    
    // INSERT
    [DataTestMethod]
    [DataRow("users", typeof(User),
        "INSERT INTO users (Name, Age) VALUES (@p0, @p1)")]

    [DataRow("games", typeof(Game),
        "INSERT INTO games (Name, Platform, Status) VALUES (@p0, @p1, @p2)")]

    public void BuildInsert_ValidModel_ReturnsCorrectSql(
        string tableName,
        Type modelType,
        string expected)
    {
        // Arrange
        var properties = modelType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "ID")
            .ToArray();

        // Act
        string result = SqlCommandBuilder.BuildInsert(
            tableName,
            properties);

        // Assert
        Assert.AreEqual(expected, result);
    }
    

    [TestMethod]
    public void BuildInsert_EmptyProperties_ThrowsException()
    {
        // Arrange
        PropertyInfo[] properties = GetProperties<EmptyModel>();

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(
            () => SqlCommandBuilder.BuildInsert("empty_models", properties));
    }
    
    [TestMethod]
    public void BuildInsert_NullProperties_ThrowsException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => SqlCommandBuilder.BuildInsert("users", null));
    }
    
    
    // UPDATE

    [DataTestMethod]
    [DataRow("users", typeof(User),
        "UPDATE users SET Name = @p0, Age = @p1 WHERE id = @id")]

    [DataRow("games", typeof(Game),
        "UPDATE games SET Name = @p0, Platform = @p1, Status = @p2 WHERE id = @id")]

    public void BuildUpdate_ValidModel_ReturnsCorrectSql(
        string tableName,
        Type modelType,
        string expected)
    {
        // Arrange
        var properties = modelType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "ID")
            .ToArray();

        // Act
        string result = SqlCommandBuilder.BuildUpdate(
            tableName,
            properties);

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [TestMethod]
    public void BuildUpdate_InvalidTableName_ThrowsException()
    {
        string tableName = "users; DROP TABLE users";
        PropertyInfo[] properties = GetProperties<User>();

        Assert.ThrowsException<ArgumentException>(
            () => SqlCommandBuilder.BuildUpdate(tableName, properties));
    }
    
    [TestMethod]
    public void BuildUpdate_NullProperties_ThrowsException()
    {
        string tableName = "users";

        Assert.ThrowsException<ArgumentNullException>(
            () => SqlCommandBuilder.BuildUpdate(tableName, null));
    }
    
    // DELETE
    
    [TestMethod]
    public void BuildDelete_ValidTableName_ReturnsCorrectSql()
    {
        string tableName = "users";

        string result = SqlCommandBuilder.BuildDelete(tableName);

        string expected = "DELETE FROM users WHERE id = @id";

        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void BuildDelete_InvalidTableName_ThrowsException()
    {
        string tableName = "users; DROP TABLE users";

        Assert.ThrowsException<ArgumentException>(
            () => SqlCommandBuilder.BuildDelete(tableName));
    }
    
    [TestMethod]
    public void BuildDelete_EmptyTableName_ThrowsException()
    {
        string tableName = "";

        Assert.ThrowsException<ArgumentException>(
            () => SqlCommandBuilder.BuildDelete(tableName));
    }
    
    // MapColumns
    
    [TestMethod]
    public void MapColumns_ValidColumns_ReturnsCorrectMapping()
    {
        string[] columnNames = { "Id", "Name", "Age" };

        var result = OrmContext.MapColumns(columnNames);

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(0, result["Id"]);
        Assert.AreEqual(1, result["Name"]);
        Assert.AreEqual(2, result["Age"]);
    }
    
    [TestMethod]
    public void MapColumns_EmptyColumns_ReturnsEmptyDictionary()
    {
        string[] columnNames = Array.Empty<string>();

        var result = OrmContext.MapColumns(columnNames);

        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public void MapColumns_CaseInsensitive_ReturnsCorrectMapping()
    {
        // Arrange
        string[] columnNames = { "Id", "Name", "Age" };

        // Act
        var result = OrmContext.MapColumns(columnNames);
        
        // Assert
        Assert.AreEqual(0, result["id"]);
        Assert.AreEqual(1, result["NAME"]);
        Assert.AreEqual(2, result["age"]);
    }
    
    [TestMethod]
    public void MapColumns_NullColumns_ThrowsException()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => OrmContext.MapColumns(null));
    }
}

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class EmptyModel
{
    public int ID { get; set; }
}

public class Game
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Platform { get; set; }
    public bool Status { get; set; }
}