using Microsoft.AspNetCore.Mvc;
using OTP.Domain;
using OTP.MVC.Models;

namespace OTP.MVC.Controllers;

public class TotpController : Controller
{
    private readonly IOtpService otpService;

    public TotpController(IOtpService otpService)
    {
        this.otpService = otpService;
    }

    [HttpGet]
    public IActionResult RequestTotp() => View();

    [HttpPost]
    public IActionResult RequestTotp(string userId)
    {
        var validatedUserId = new NonEmptyString(userId);

        var totp = otpService.GenerateTotp(validatedUserId, DateTimeOffset.UtcNow);

        var model = new TotpReceivedModel(validatedUserId, totp);

        return View("TotpReceived", model);
    }

    [HttpGet]
    public IActionResult ValidateTotp() => View();

    [HttpPost]
    public IActionResult ValidateTotp(string userId, string totp)
    {
        var validateUserId = new NonEmptyString(userId);
        var otp = new OneTimePassword(totp);

        var success = otpService.ValidateOtp(validateUserId, otp, DateTimeOffset.UtcNow);

        if (!success)
        {
            return View("InvalidTotp");
        }

        return View("ValidTotp");
    }
}