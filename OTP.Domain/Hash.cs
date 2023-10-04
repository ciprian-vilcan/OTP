namespace OTP.Domain;

public sealed record Hash(string Value)
{
    public static implicit operator string(Hash instance) => instance.Value;
}
