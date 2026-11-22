namespace ORM.UnitTests;
[TestClass]
public class ValueConvertingTests
{
    
    [TestMethod]
    public void ConvertValue_IntToInt_ReturnsSameValue()
    {
        // Arrange
        int value = 42;

        // Act
        object result = OrmContext.ConvertValue(value, typeof(int));

        // Assert
        Assert.AreEqual(42, result);
        Assert.AreEqual(typeof(int), result.GetType());
    }

    [TestMethod]
    public void ConvertValue_IntToString_ReturnsString()
    {
        // Arrange
        int value = 42;

        // Act
        object result = OrmContext.ConvertValue(value, typeof(string));

        // Assert
        Assert.AreEqual("42", result);
        Assert.AreEqual(typeof(string), result.GetType());
    }

    [TestMethod]
    public void ConvertValue_IntToDecimal_ReturnsDecimal()
    {
        // Arrange
        int value = 42;

        // Act
        object result = OrmContext.ConvertValue(value, typeof(decimal));

        // Assert
        Assert.AreEqual(42m, result);
        Assert.AreEqual(typeof(decimal), result.GetType());
    }

    [TestMethod]
    public void ConvertValue_StringToInt_ReturnsInt()
    {
        // Arrange
        string value = "123";

        // Act
        object result = OrmContext.ConvertValue(value, typeof(int));

        // Assert
        Assert.AreEqual(123, result);
        Assert.AreEqual(typeof(int), result.GetType());
    }

    [TestMethod]
    public void ConvertValue_IntToNullableInt_ReturnsInt()
    {
        // Arrange
        int value = 42;

        // Act
        object result = OrmContext.ConvertValue(value, typeof(int?));

        // Assert
        Assert.AreEqual(42, result);
        Assert.AreEqual(typeof(int), result.GetType());
    }

    [TestMethod]
    public void ConvertValue_NullToString_ReturnsNull()
    {
        // Act
        object result = OrmContext.ConvertValue(null, typeof(string));

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ConvertValue_DbNullToString_ReturnsNull()
    {
        // Act
        object result = OrmContext.ConvertValue(DBNull.Value, typeof(string));

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ConvertValue_DbNullToNullableInt_ReturnsNull()
    {
        // Act
        object result = OrmContext.ConvertValue(DBNull.Value, typeof(int?));

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ConvertValue_DbNullToInt_ReturnsDefaultValue()
    {
        // Act
        object result = OrmContext.ConvertValue(DBNull.Value, typeof(int));

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ConvertValue_DbNullToBool_ReturnsDefaultValue()
    {
        // Act
        object result = OrmContext.ConvertValue(DBNull.Value, typeof(bool));

        // Assert
        Assert.AreEqual(false, result);
    }
    

    [TestMethod]
    public void ConvertValue_InvalidStringToInt_ThrowsFormatException()
    {
        // Arrange
        string value = "not a number";

        // Act & Assert
        Assert.ThrowsException<FormatException>(
            () => OrmContext.ConvertValue(value, typeof(int)));
    }

    [TestMethod]
    public void ConvertValue_TooLargeNumberToInt_ThrowsOverflowException()
    {
        // Arrange
        long value = long.MaxValue;

        // Act & Assert
        Assert.ThrowsException<OverflowException>(
            () => OrmContext.ConvertValue(value, typeof(int)));
    }

}