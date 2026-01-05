using System.IdentityModel.Tokens.Jwt;
using FitTracker.Application.Constants;
using FitTracker.Infrastructure.Services;
using FitTracker.Infrastructure.Settings;
using FitTrackerInfrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace FitTrackerInfrastructure.Tests.Services;

public class JwtTokenGeneratorTests
{
    private readonly JwtTokenGenerator _generator;
    private readonly JwtSettings _settings;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenGeneratorTests()
    {
        _settings = new JwtSettings
        {
            Key = "this-is-a-very-secure-secret-key-with-at-least-32-characters",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            TokenValidityInDays = 7,
            EmailVerificationTokenValidityInMinutes = 60,
            PasswordResetTokenValidityInMinutes = 30,
        };

        var options = Options.Create(_settings);
        _generator = new JwtTokenGenerator(options);
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser(isEmailVerified: true);

        // Act
        var token = _generator.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.Subject.Should().Be(user.Id.ToString());

        jwtToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == user.Username);

        jwtToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);

        jwtToken.Claims.Should().Contain(c =>
            c.Type == "is_email_verified" && c.Value == "true");

        jwtToken.Issuer.Should().Be(_settings.Issuer);
        jwtToken.Audiences.Should().Contain(_settings.Audience);
    }

    [Fact]
    public void GenerateToken_WithUnverifiedEmail_ShouldIncludeFalseVerificationClaim()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser(
            "unverified",
            "unverified@example.com",
            isEmailVerified: false);

        // Act
        var token = _generator.GenerateToken(user);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);
        jwtToken.Claims.Should().Contain(c => c.Type == "is_email_verified" && c.Value == "false");
    }

    [Fact]
    public void GenerateVerificationToken_ShouldContainCorrectPurpose()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var token = _generator.GenerateVerificationToken(userId);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.Subject.Should().Be(userId.ToString());
        jwtToken.Claims.Should().Contain(c =>
            c.Type == "purpose" && c.Value == TokenPurposes.EmailVerification);
        jwtToken.Issuer.Should().Be(_settings.Issuer);
        jwtToken.Audiences.Should().Contain(_settings.Audience);
    }

    [Fact]
    public void GeneratePasswordResetToken_ShouldContainCorrectPurpose()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var token = _generator.GeneratePasswordResetToken(userId);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.Subject.Should().Be(userId.ToString());
        jwtToken.Claims.Should().Contain(c =>
            c.Type == "purpose" && c.Value == TokenPurposes.PasswordReset);
        jwtToken.Issuer.Should().Be(_settings.Issuer);
        jwtToken.Audiences.Should().Contain(_settings.Audience);
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectExpiration()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser();
        var expectedExpiry = DateTime.UtcNow.AddDays(_settings.TokenValidityInDays);

        // Act
        var token = _generator.GenerateToken(user);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
        jwtToken.ValidTo.Should().BeCloseTo(expectedExpiry, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void GenerateVerificationToken_ShouldExpireInConfiguredMinutes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedExpiry = DateTime.UtcNow.AddMinutes(_settings.EmailVerificationTokenValidityInMinutes);

        // Act
        var token = _generator.GenerateVerificationToken(userId);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.ValidTo.Should().BeCloseTo(expectedExpiry, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void GeneratePasswordResetToken_ShouldExpireInConfiguredMinutes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedExpiry = DateTime.UtcNow.AddMinutes(_settings.PasswordResetTokenValidityInMinutes);

        // Act
        var token = _generator.GeneratePasswordResetToken(userId);

        // Assert
        var jwtToken = _tokenHandler.ReadJwtToken(token);

        jwtToken.ValidTo.Should().BeCloseTo(expectedExpiry, TimeSpan.FromSeconds(5));
    }
}
