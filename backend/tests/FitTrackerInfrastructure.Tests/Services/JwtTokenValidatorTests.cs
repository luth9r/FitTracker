using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitTracker.Infrastructure.Services;
using FitTracker.Infrastructure.Settings;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitTrackerInfrastructure.Tests.Services
{
    public sealed class JwtTokenValidatorTests
    {
        private readonly JwtSettings _jwtSettings = new()
        {
            Key = "this-is-a-very-secure-secret-key-for-testing-at-least-32-chars",
            Issuer = "FitTrackerTest",
            Audience = "FitTrackerTestAudience",
            TokenValidityInDays = 7,
            EmailVerificationTokenValidityInMinutes = 60,
            PasswordResetTokenValidityInMinutes = 30,
        };

        private JwtTokenValidator CreateValidator()
        {
            var options = Options.Create(_jwtSettings);
            return new JwtTokenValidator(options);
        }

        private string GenerateValidToken(Guid userId, string? purpose = null, int? expiresInMinutes = null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Email, "test@example.com"),
            };

            if (purpose != null)
            {
                claims.Add(new Claim("purpose", purpose));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes ?? 60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId);

            // Act
            var result = validator.ValidateToken(token);

            // Assert
            result.Should().NotBeNull();
            result!.FindFirst(ClaimTypes.NameIdentifier)?.Value.Should().Be(userId.ToString());
            result.FindFirst(ClaimTypes.Email)?.Value.Should().Be("test@example.com");
        }

        [Fact]
        public void ValidateToken_WithExpiredToken_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId, expiresInMinutes: -1); // Expired

            // Act
            var result = validator.ValidateToken(token);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_WithInvalidSignature_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();

            // Generate token with different key
            var wrongKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("different-key-that-is-at-least-32-chars-long"));
            var credentials = new SigningCredentials(wrongKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Act
            var result = validator.ValidateToken(token);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_WithInvalidIssuer_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "WrongIssuer",
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Act
            var result = validator.ValidateToken(token);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_WithInvalidAudience_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: "WrongAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Act
            var result = validator.ValidateToken(token);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_WithMalformedToken_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.ValidateToken("not-a-valid-jwt-token");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidateToken_WithEmptyToken_ShouldReturnNull()
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.ValidateToken(string.Empty);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ValidatePurposeToken_WithValidTokenAndPurpose_ShouldReturnSuccess()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId, purpose: "email-verification");

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(userId);
        }

        [Fact]
        public void ValidatePurposeToken_WithWrongPurpose_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId, purpose: "password-reset");

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid token purpose");
        }

        [Fact]
        public void ValidatePurposeToken_WithMissingPurpose_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId); // No purpose claim

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid token purpose");
        }

        [Fact]
        public void ValidatePurposeToken_WithInvalidToken_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();

            // Act
            var result = validator.ValidatePurposeToken("invalid-token", "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid token");
        }

        [Fact]
        public void ValidatePurposeToken_WithMissingUserId_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();

            var claims = new List<Claim>
            {
                new("purpose", "email-verification"),

                // No NameIdentifier claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid user ID");
        }

        [Fact]
        public void ValidatePurposeToken_WithInvalidUserIdFormat_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "not-a-guid"),
                new("purpose", "email-verification"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid user ID");
        }

        [Fact]
        public void ValidatePurposeToken_WithExpiredToken_ShouldReturnFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token = GenerateValidToken(userId, purpose: "email-verification", expiresInMinutes: -1);

            // Act
            var result = validator.ValidatePurposeToken(token, "email-verification");

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Invalid token");
        }

        [Fact]
        public void ValidatePurposeToken_WithMultiplePurposes_ShouldValidateCorrectly()
        {
            // Arrange
            var validator = CreateValidator();
            var userId = Guid.NewGuid();
            var token1 = GenerateValidToken(userId, purpose: "email-verification");
            var token2 = GenerateValidToken(userId, purpose: "password-reset");

            // Act
            var result1 = validator.ValidatePurposeToken(token1, "email-verification");
            var result2 = validator.ValidatePurposeToken(token2, "password-reset");

            // Assert
            result1.IsSuccess.Should().BeTrue();
            result1.Value.Should().Be(userId);

            result2.IsSuccess.Should().BeTrue();
            result2.Value.Should().Be(userId);
        }

        [Fact]
        public void Constructor_WithMinimumKeyLength_ShouldWork()
        {
            // Arrange
            var settings = new JwtSettings
            {
                Key = "12345678901234567890123456789012", // Exactly 32 chars
                Issuer = "Test",
                Audience = "Test",
                TokenValidityInDays = 1,
                EmailVerificationTokenValidityInMinutes = 30,
                PasswordResetTokenValidityInMinutes = 15,
            };

            var options = Options.Create(settings);

            // Act
            var validator = new JwtTokenValidator(options);

            // Assert
            validator.Should().NotBeNull();
        }
    }
}
