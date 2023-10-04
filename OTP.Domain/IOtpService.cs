namespace OTP.Domain;

public interface IOtpService
{
    Totp GenerateTotp(NonEmptyString userId, DateTimeOffset momentOfRequest);

    bool ValidateOtp(NonEmptyString userId, OneTimePassword otp, DateTimeOffset momentOfRequest);
}
