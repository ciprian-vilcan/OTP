namespace OTP.Domain;

public sealed record NonEmptyString
{
    public string Value { get; init; }

    /// <summary>Initializes a new instance of the <see cref="NonEmptyString" /> class.</summary>
    /// <param name="value">The value.</param>
    /// <exception cref="System.ArgumentException">Value</exception>
    public NonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("String must not be null or empty");
        }

        this.Value = value;
    }

    /// <summary>Performs an implicit conversion from <see cref="NonEmptyString" /> to <see cref="System.String" />.</summary>
    /// <param name="instance">The instance.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator string(NonEmptyString instance) => instance.Value;

    public override string ToString() => Value;
}
