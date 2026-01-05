using FitTracker.Infrastructure.Services;
using FluentAssertions;
#pragma warning disable CS8669 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. Auto-generated code requires an explicit '#nullable' directive in source.

namespace FitTrackerInfrastructure.Tests.Services;

public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher = new();

    [Fact]
    public void HashPassword_WithValidPassword_ShouldReturnHash()
    {
        // Arrange
        var password = "MySecurePassword123!";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().NotBe(password);
        hash.Should().StartWith("$2"); // BCrypt hash format
    }

    [Fact]
    public void HashPassword_SamePasswordMultipleTimes_ShouldGenerateDifferentHashes()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hash1 = _passwordHasher.HashPassword(password);
        var hash2 = _passwordHasher.HashPassword(password);

        // Assert
        hash1.Should().NotBe(hash2); // BCrypt uses random salt
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void HashPassword_WithEmptyOrNullPassword_ShouldThrowArgumentException(string? invalidPassword)
    {
        // Act
        var act = () => _passwordHasher.HashPassword(invalidPassword!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Password cannot be empty");
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "CorrectPassword123!";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var result = _passwordHasher.VerifyPassword(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "CorrectPassword123!";
        var incorrectPassword = "WrongPassword456!";
        var hash = _passwordHasher.HashPassword(correctPassword);

        // Act
        var result = _passwordHasher.VerifyPassword(incorrectPassword, hash);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "validhash")]
    [InlineData("", "validhash")]
    [InlineData("   ", "validhash")]
    public void VerifyPassword_WithEmptyOrNullPassword_ShouldReturnFalse(string? invalidPassword, string hash)
    {
        // Act
        var result = _passwordHasher.VerifyPassword(invalidPassword!, hash);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("password", null)]
    [InlineData("password", "")]
    [InlineData("password", "   ")]
    public void VerifyPassword_WithEmptyOrNullHash_ShouldReturnFalse(string password, string? invalidHash)
    {
        // Act
        var result = _passwordHasher.VerifyPassword(password, invalidHash!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_WithInvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123";
        var invalidHash = "not-a-valid-bcrypt-hash";

        // Act
        var result = _passwordHasher.VerifyPassword(password, invalidHash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_WithMalformedHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123";
        var malformedHash = "$2a$10$invalid";

        // Act
        var result = _passwordHasher.VerifyPassword(password, malformedHash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HashPassword_WithLongPassword_ShouldWork()
    {
        // Arrange
        var longPassword = new string('a', 100);

        // Act
        var hash = _passwordHasher.HashPassword(longPassword);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordHasher.VerifyPassword(longPassword, hash).Should().BeTrue();
    }

    [Fact]
    public void HashPassword_WithSpecialCharacters_ShouldWork()
    {
        // Arrange
        var password = "P@ssw0rd!#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordHasher.VerifyPassword(password, hash).Should().BeTrue();
    }

    [Fact]
    public void HashPassword_WithUnicodeCharacters_ShouldWork()
    {
        // Arrange
        var password = "Пароль123!密码";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordHasher.VerifyPassword(password, hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_IsCaseSensitive()
    {
        // Arrange
        var password = "Password123";
        var hash = _passwordHasher.HashPassword(password);

        // Act
        var resultLower = _passwordHasher.VerifyPassword("password123", hash);
        var resultUpper = _passwordHasher.VerifyPassword("PASSWORD123", hash);
        var resultCorrect = _passwordHasher.VerifyPassword(password, hash);

        // Assert
        resultLower.Should().BeFalse();
        resultUpper.Should().BeFalse();
        resultCorrect.Should().BeTrue();
    }

    [Fact]
    public void HashPassword_MultiplePasswords_ShouldProduceDifferentHashes()
    {
        // Arrange
        var password1 = "Password1";
        var password2 = "Password2";

        // Act
        var hash1 = _passwordHasher.HashPassword(password1);
        var hash2 = _passwordHasher.HashPassword(password2);

        // Assert
        hash1.Should().NotBe(hash2);
        _passwordHasher.VerifyPassword(password1, hash1).Should().BeTrue();
        _passwordHasher.VerifyPassword(password2, hash2).Should().BeTrue();
        _passwordHasher.VerifyPassword(password1, hash2).Should().BeFalse();
        _passwordHasher.VerifyPassword(password2, hash1).Should().BeFalse();
    }

    [Fact]
    public void HashPassword_WithMinimumLengthPassword_ShouldWork()
    {
        // Arrange
        var password = "a";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordHasher.VerifyPassword(password, hash).Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithSpacesInPassword_ShouldBeExact()
    {
        // Arrange
        var passwordWithSpaces = "Pass word 123";
        var passwordNoSpaces = "Password123";
        var hash = _passwordHasher.HashPassword(passwordWithSpaces);

        // Act
        var resultCorrect = _passwordHasher.VerifyPassword(passwordWithSpaces, hash);
        var resultIncorrect = _passwordHasher.VerifyPassword(passwordNoSpaces, hash);

        // Assert
        resultCorrect.Should().BeTrue();
        resultIncorrect.Should().BeFalse();
    }

    [Fact]
    public void HashPassword_ShouldUseBCryptFormat()
    {
        // Arrange
        var password = "TestPassword";

        // Act
        var hash = _passwordHasher.HashPassword(password);

        // Assert
        hash.Should().MatchRegex(@"^\$2[aby]?\$\d{2}\$.{53}$"); // BCrypt format regex
    }

    [Fact]
    public void VerifyPassword_WithHashFromDifferentPassword_ShouldReturnFalse()
    {
        // Arrange
        var password1 = "FirstPassword";
        var password2 = "SecondPassword";
        var hash1 = _passwordHasher.HashPassword(password1);

        // Act
        var result = _passwordHasher.VerifyPassword(password2, hash1);

        // Assert
        result.Should().BeFalse();
    }
}
