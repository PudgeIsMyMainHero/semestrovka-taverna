namespace ORM.UnitTests;

[TestClass]
public class EscapeIdentifierTests
{
    [TestMethod]
    public void EscapeIdentifier_ValidTableName_ReturnsSameName()
    {
        // Arrange
        string tableName = "Users";

        // Act
        string result = OrmContext.EscapeIdentifier(tableName);

        // Assert
        Assert.AreEqual(tableName, result);
    }
    
    [DataTestMethod]
    [DataRow("123Users")]
    [DataRow("User-Data")]
    [DataRow("User Data")]
    [DataRow("Users; DROP TABLE Users")]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void EscapeIdentifier_InvalidName_ThrowsArgumentException(
        string identifier)
    {
        // Arrange

        // Act
        Action action = () => OrmContext.EscapeIdentifier(identifier);

        // Assert
        Assert.ThrowsException<ArgumentException>(action);
    }
}