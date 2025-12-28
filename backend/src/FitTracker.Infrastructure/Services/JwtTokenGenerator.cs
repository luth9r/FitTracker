using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitTracker.Application.Constants;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FitTracker.Infrastructure.Services
{
    public sealed class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
    {
        private const string PurposeClaim = "purpose";
        private readonly JwtSettings settings = options.Value;

        /// <inheritdoc/>
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("is_email_verified", user.IsEmailVerified.ToString().ToLower()),
            };

            return CreateToken(claims, TimeSpan.FromDays(settings.TokenValidityInDays));
        }

        /// <inheritdoc/>
        public string GenerateVerificationToken(Guid userId) => GeneratePurposeToken(userId, TokenPurposes.EmailVerification, settings.EmailVerificationTokenValidityInMinutes);

        /// <inheritdoc/>
        public string GeneratePasswordResetToken(Guid userId) => GeneratePurposeToken(userId, TokenPurposes.PasswordReset, settings.PasswordResetTokenValidityInMinutes);

        private string GeneratePurposeToken(Guid userId, string purpose, int expiryMinutes)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(PurposeClaim, purpose),
            };

            return CreateToken(claims, TimeSpan.FromMinutes(expiryMinutes));
        }

        private string CreateToken(IEnumerable<Claim> claims, TimeSpan expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiration),
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
