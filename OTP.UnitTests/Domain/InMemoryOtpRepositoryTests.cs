namespace OTP.UnitTests.Domain;

public sealed class InMemoryOtpRepositoryTests
{
    private InMemoryOtpRepository repo = new InMemoryOtpRepository();

    [Fact]
    public void MultipleOtpsForSameUser_GetLatestActiveTotpHashReturnsLatest()
    {
        // Arrange
        var userId = new NonEmptyString("user1");

        var otpHash = new HashedTotp(new Hash("hashedOtp"), DateTimeOffset.Now, DateTimeOffset.Now);
        var secondHash = new HashedTotp(new Hash("another hash"), DateTimeOffset.Now.AddSeconds(1), DateTimeOffset.Now.AddSeconds(1));

        // Act
        repo.SaveOtp(userId, otpHash);
        repo.SaveOtp(userId, secondHash);

        var actual = repo.GetLatestTotpHash(userId);

        // Assert
        Assert.Equal(secondHash, actual);
    }

    [Fact]
    public void NoOtpsStored_GetLatestActiveTotpHashReturnsNull()
    {
        // Arrange

        // Act
        var actual = repo.GetLatestTotpHash(new NonEmptyString("user1"));

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void NoOtpsStored_UseUpOtpDoesNothing()
    {
        // Arrange

        // Act
        repo.UseUpOtp(new NonEmptyString("user1"), new Hash(""));

        // Assert
    }

    [Fact]
    public void UseUpOtpExisting_RemovesLastEntry()
    {
        // Arrange
        var userId = new NonEmptyString("user1");

        var otpHash = new HashedTotp(new Hash("hashedOtp"), DateTimeOffset.Now, DateTimeOffset.Now);

        // Act
        repo.SaveOtp(userId, otpHash);
        repo.UseUpOtp(userId, otpHash.HashedOtp);

        var actual = repo.GetLatestTotpHash(new NonEmptyString("user1"));

        // Assert
        Assert.Null(actual);
    }
}
