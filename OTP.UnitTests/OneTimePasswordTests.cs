namespace OTP.UnitTests
{
    public sealed class OneTimePasswordTests
    {
        [Theory]
        [InlineData("12345")]
        [InlineData("1234567")]
        public void Not6Characters_ThrowsArgumentException(string value)
        {
            // Arrange

            // Act
            Action act = () => new OneTimePassword(value);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
            Assert.Equal("value", exception.ParamName);
            Assert.Equal("OTP must be exactly 6 characters long (Parameter 'value')", exception.Message);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("AB45CD")]
        public void SixCharacters_ValueIsSameAsGivenParameter(string value)
        {
            // Arrange

            // Act
            var actual = new OneTimePassword(value);

            // Assert
            Assert.Equal(value, actual.Value);
            Assert.Equal(value, actual);
        }
    }
}
