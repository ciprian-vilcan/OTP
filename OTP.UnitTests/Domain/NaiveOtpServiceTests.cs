using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public void OtpIsOfGivenLength()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(6, actual.Otp.Value.Length);
    }

    [Fact]
    public void OtpIsNumerical()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.True(int.TryParse(actual.Otp, out _));
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
        otpRepoMock.Setup(call => call.GetLatestActiveTotpHash(userId)).Returns(activeTotpHashes.Dequeue);

        // Act
        var totp = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);
        var firstValidation = sut.ValidateOtp(userId, totp.Otp, DateTimeOffset.UtcNow);
        var secondValidation = sut.ValidateOtp(userId, totp.Otp, DateTimeOffset.UtcNow);

        // Assert
        Assert.True(firstValidation);
        Assert.False(secondValidation);
    }

    [Fact]
    public void ExpiresAtLaterThanCreateAt()
    {
        // Arrange

        // Act
        var actual = sut.GenerateTotp(userId, DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(30), TimeSpan.FromTicks(actual.ExpiresAt.UtcTicks - actual.CreatedAt.UtcTicks));
    }
}
