namespace OTP.Domain;

/// <summary>
/// A really rough sketch of what a database implementation should look like. Not to be taken too seriously.
/// If this were a real solution, the TOTPs and their history would be stored in a DB and not deleted like I do here.
/// </summary>
/// <seealso cref="OTP.Domain.IOtpRepository" />
public sealed class InMemoryOtpRepository : IOtpRepository
{
    private readonly Dictionary<NonEmptyString, HashedTotp> UserHashedTotps = new();

    public void SaveOtp(NonEmptyString userId, HashedTotp totpHash)
    {
        if (!UserHashedTotps.ContainsKey(userId))
        {
            UserHashedTotps[userId] = totpHash;
        }
        else
        {
            UserHashedTotps[userId] = totpHash;
        }
    }

    public HashedTotp? GetLatestActiveTotpHash(NonEmptyString userId)
    {
        if (!UserHashedTotps.ContainsKey(userId))
        {
            return null;
        }

        return UserHashedTotps[userId];
    }

    /// <summary>
    /// Uses up a totp and invalidate it for any future usage.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="hashedOtp">The hashed otp.</param>
    public void UseUpOtp(NonEmptyString userId, Hash hashedOtp)
    {
        UserHashedTotps.Remove(userId);
    }
}
