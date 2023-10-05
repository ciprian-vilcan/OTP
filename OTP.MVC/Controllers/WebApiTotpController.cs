using Microsoft.AspNetCore.Mvc;
using OTP.Domain;
namespace OTP.Web.Controllers;

[Route("api/totp")]
[ApiController]
public class WebApiTotpController : ControllerBase
{
    private readonly IOtpService otpService;

    public WebApiTotpController(IOtpService otpService)
    {
        this.otpService = otpService;
    }

    [HttpGet]
    public IActionResult GetTotp(string userId) 
    {
        try 
        { 
            var validatedUserId = new NonEmptyString(userId);
            var totp = otpService.GenerateTotp(validatedUserId, DateTimeOffset.UtcNow);

            return this.Ok(totp);
        }
        catch (ArgumentException ex)
        { 
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult ValidateTotp(string userId, string otp)
    {
        try
        {
            var validatedUserId = new NonEmptyString(userId);
            var validated = otpService.ValidateOtp(validatedUserId, new OneTimePassword(otp), DateTimeOffset.UtcNow);

            return this.Ok(validated);
        }       
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
