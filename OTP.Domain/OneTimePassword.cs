namespace OTP.Domain
{
    public sealed partial record OneTimePassword(string Value)
    {
        public static implicit operator string(OneTimePassword instance) => instance.Value;
    }
}
