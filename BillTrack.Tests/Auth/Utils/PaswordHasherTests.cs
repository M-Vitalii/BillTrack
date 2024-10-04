using BillTrack.Auth.Utils;

namespace BillTrack.Tests.Auth.Utils;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void Hash_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hashedPassword = _passwordHasher.Hash(password);

        // Assert
        Assert.False(string.IsNullOrEmpty(hashedPassword));
    }

    [Fact]
    public void Verify_ShouldReturnTrueForValidPassword()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordHasher.Hash(password);

        // Act
        var isValid = _passwordHasher.Verify(hashedPassword, password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Verify_ShouldReturnFalseForInvalidPassword()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordHasher.Hash(password);
        var invalidPassword = "WrongPassword456";

        // Act
        var isValid = _passwordHasher.Verify(hashedPassword, invalidPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Hash_ShouldReturnDifferentHashesForDifferentPasswords()
    {
        // Arrange
        var password1 = "PasswordOne";
        var password2 = "PasswordTwo";

        // Act
        var hash1 = _passwordHasher.Hash(password1);
        var hash2 = _passwordHasher.Hash(password2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_ShouldReturnDifferentHashesForSamePassword()
    {
        // Arrange
        var password = "SamePassword";

        // Act
        var hash1 = _passwordHasher.Hash(password);
        var hash2 = _passwordHasher.Hash(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // Because of random salt, hashes should be different
    }
}