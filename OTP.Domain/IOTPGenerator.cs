namespace OTP.Domain
{
    public interface IOTPGenerator
    {
        OneTimePassword GenerateOTP(NonEmptyString userId, DateTimeOffset momentOfRequest);
    }
}
