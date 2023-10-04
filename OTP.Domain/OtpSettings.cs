namespace OTP.Domain;

public sealed record OtpSettings(TimeSpan Step, int Length);
