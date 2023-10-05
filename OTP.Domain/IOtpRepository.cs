namespace OTP.Domain;

public interface IOtpRepository
{
    void SaveOtp(NonEmptyString userId, HashedTotp otpHash);

    HashedTotp? GetLatestTotpHash(NonEmptyString userId);

    /// <summary>
    /// Uses up a totp and invalidate it for any future usage.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="hashedOtp">The hashed otp.</param>
    void UseUpOtp(NonEmptyString userId, Hash hashedOtp);
}
