namespace OTP.Domain;

/// <summary>
/// Naive because it uses <see cref="Random"/> instead of a cryptographically sound RNG.
/// </summary>
/// <seealso cref="OTP.Domain.IOtpService" />
public sealed class NaiveOtpService : IOtpService
{
    private readonly OtpSettings otpSettings;
    private readonly IOtpRepository otpRepository;
    private readonly IHashService hashService;

    public NaiveOtpService(OtpSettings otpSettings, IOtpRepository otpRepository, IHashService hashService)
    {
        this.otpSettings = otpSettings;
        this.otpRepository = otpRepository;
        this.hashService = hashService;
    }

    public Totp GenerateTotp(NonEmptyString userId, DateTimeOffset momentOfRequest)
    {
        var random = new Random();

        var otpNumberSequence = Enumerable.Range(1, otpSettings.Length)
            .Select(i => random.Next(0, 9))
            .Aggregate("", (a, b) => a.ToString() + b.ToString());
        var otp = new OneTimePassword(otpNumberSequence);
        var hashedOtp = hashService.Hash(otp);

        var hashedTotp = new HashedTotp(hashedOtp, momentOfRequest, momentOfRequest.Add(otpSettings.Step));
        otpRepository.SaveOtp(userId, hashedTotp);

        return new Totp(otp, momentOfRequest, momentOfRequest.Add(otpSettings.Step));
    }

    public bool ValidateOtp(NonEmptyString userId, OneTimePassword otp, DateTimeOffset momentOfRequest)
    {
        var hashedOtp = hashService.Hash(otp);
        var latestTotpHash = otpRepository.GetLatestActiveTotpHash(userId);

        if (latestTotpHash != null && latestTotpHash.HashedOtp == hashedOtp)
        {
            otpRepository.UseUpOtp(userId, hashedOtp);
            return true;
        }

        return false;
    }
}
