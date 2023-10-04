namespace OTP.Domain;

public sealed record Totp(OneTimePassword Otp, DateTimeOffset CreatedAt, DateTimeOffset ExpiresAt);
