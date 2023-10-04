using Microsoft.AspNetCore.Mvc;
using Moq;
using OTP.MVC.Controllers;

namespace OTP.UnitTests.MVC;

public class OtpControllerTests
{
    private Mock<IOtpService> otpServiceMock = new Mock<IOtpService>();
    private TotpController sut;

    public OtpControllerTests()
    {
        sut = new TotpController(otpServiceMock.Object);
    }

    [Fact]
    public void InvalidToken_RendersInvalidOtpView()
    {
        // Arrange

        // Act
        var actual = sut.ValidateTotp("qwe", "123456");

        // Assert
        Assert.Equal("InvalidTotp", (actual as ViewResult).ViewName);
    }


    [Fact]
    public void ValidToken_RendersValidOtpView()
    {
        // Arrange
        otpServiceMock
            .Setup(call => call.ValidateOtp(It.IsAny<NonEmptyString>(), It.IsAny<OneTimePassword>(), It.IsAny<DateTimeOffset>()))
            .Returns(true);

        // Act
        var actual = sut.ValidateTotp("qwe", "123456");

        // Assert
        Assert.Equal("ValidTotp", (actual as ViewResult).ViewName);
    }
}
