namespace OTP.Domain
{
    public sealed record NonEmptyString
    {
        public string Value { get; init; }

        /// <summary>Initializes a new instance of the <see cref="NonEmptyString" /> class.</summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">Value</exception>
        public NonEmptyString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("Value");
            }

            this.Value = value;
        }

        /// <summary>Performs an implicit conversion from <see cref="NonEmptyString" /> to <see cref="System.String" />.</summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(NonEmptyString instance) => instance.Value;
    }
}
