namespace OTP.UnitTests.Domain;

public sealed class NonEmptyStringTests
{
    [Fact]
    public void EmptyString_ThrowsNullArgumentException()
    {
        // Arrange


        // Act
        Action action = () => new NonEmptyString("");

        // Assert
        var exception = Assert.Throws<ArgumentException>(() => action());
        Assert.Equal("String must not be null or empty", exception.Message);
    }

    [Fact]
    public void NonEmptyString_ConstructorWorksAndValueIsSameAsParameter()
    {
        // Arrange

        // Act
        var actual = new NonEmptyString("random");

        // Assert
        Assert.Equal("random", actual.Value);
        Assert.Equal("random", actual);
    }
}
