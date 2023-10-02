namespace OTP.Domain
{
    public sealed record OneTimePassword
    {
        public string Value { get; init;}

        public OneTimePassword(string value)
        {
            if (value.Length != 6)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "OTP must be exactly 6 characters long");
            }

            this.Value = value;
        }

        public static implicit operator string(OneTimePassword instance) => instance.Value;
    }
}
