using Moq;

namespace OTP.UnitTests.Domain;

public class NaiveOtpServiceTests
{
    private readonly NonEmptyString userId = new NonEmptyString("John Doe");

    private OtpSettings settings = new OtpSettings(TimeSpan.FromSeconds(30), 6);
    private Mock<IOtpRepository> otpRepoMock = new Mock<IOtpRepository>();
    private Mock<IHashService> hashServiceMock = new Mock<IHashService>();
    private NaiveOtpService sut;

    public NaiveOtpServiceTests()
    {
        sut = new NaiveOtpService(settings, otpRepoMock.Object, hashServiceMock.Object);
    }

    [Fact]
    public void GenerateTotp_OtpIsOfGivenLength()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(6, actual.Otp.Value.Length);
    }

    [Fact]
    public void GenerateTotp_OtpIsNumerical()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.True(int.TryParse(actual.Otp, out _));
    }

    [Fact]
    public void GenerateTotp_ExpiresAtLaterThanCreateAt()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(30), TimeSpan.FromTicks(actual.ExpiresAt.UtcTicks - actual.CreatedAt.UtcTicks));
    }

    [Fact]
    public void ValidateOtp_NoOtpStored_ReturnsFalse()
    {
        // Arrange

        // Act
        var actual = sut.ValidateOtp(userId, new OneTimePassword("something"), DateTimeOffset.UtcNow);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void ValidateExpiredOtp_ReturnsFalse()
    {
        // Arrange
        var hashedOtp = new Hash("hashed otp");
        hashServiceMock.Setup(call => call.Hash(It.IsAny<string>())).Returns(hashedOtp);
        var activeTotpHashes = new Queue<HashedTotp?>(new[]
            {
                new HashedTotp(hashedOtp, DateTimeOffset.UtcNow.AddSeconds(-2), DateTimeOffset.UtcNow.AddSeconds(-1))
            });
        otpRepoMock.Setup(call => call.GetLatestTotpHash(userId)).Returns(activeTotpHashes.Dequeue);

        // Act
        var actual = sut.ValidateOtp(userId, new OneTimePassword("something"), DateTimeOffset.UtcNow);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void OtpCanBeUsedOnceAtMost()
    {
        // Arrange
        var hashedOtp = new Hash("hashed otp");
        hashServiceMock.Setup(call => call.Hash(It.IsAny<string>())).Returns(hashedOtp);
        var activeTotpHashes = new Queue<HashedTotp?>(new[]
            {
                new HashedTotp(hashedOtp, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddSeconds(123)),
                null
            });
        otpRepoMock.Setup(call => call.GetLatestTotpHash(userId)).Returns(activeTotpHashes.Dequeue);

        // Act
        var totp = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);
        var firstValidation = sut.ValidateOtp(userId, totp.Otp, DateTimeOffset.UtcNow);
        var secondValidation = sut.ValidateOtp(userId, totp.Otp, DateTimeOffset.UtcNow);

        // Assert
        Assert.True(firstValidation);
        Assert.False(secondValidation);
    }
}
