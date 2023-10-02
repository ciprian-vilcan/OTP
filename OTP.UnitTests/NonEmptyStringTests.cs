namespace OTP.UnitTests
{
    public sealed class NonEmptyStringTests
    {
        [Fact]
        public void EmptyString_ThrowsNullArgumentException()
        {
            // Arrange


            // Act
            Action action = () => new NonEmptyString("");

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => action());
            Assert.Equal(nameof(NonEmptyString.Value), exception.ParamName);
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
}
