namespace OTP.Domain;

public sealed record OneTimePassword(string Value)
{
    public static implicit operator string(OneTimePassword instance) => instance.Value;

    public override string ToString() => Value;
}
