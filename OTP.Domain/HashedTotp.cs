namespace OTP.Domain;

public sealed record HashedTotp(Hash HashedOtp, DateTimeOffset CreatedAt, DateTimeOffset ExpiresAt);
