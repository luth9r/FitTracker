using FitTracker.Application.Interfaces;
using FitTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitTracker.Infrastructure.Services
{
    public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
    {
        private const string EmailVerificationPurpose = "email_verification";

        /// <inheritdoc/>
        public string GenerateToken(User user)
        {
            var key = GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("Jwt:TokenValidityInDays", 7));

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Email, user.Email),
            };

            return CreateToken(claims, expires, creds);
        }

        /// <inheritdoc/>
        public string GenerateVerificationToken(User user)
        {
            var key = GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryInMinutes = configuration.GetValue<int>("Jwt:EmailVerificationTokenValidityInMinutes", 60);
            var expires = DateTime.UtcNow.AddMinutes(expiryInMinutes);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new ("purpose", EmailVerificationPurpose),
            };

            return CreateToken(claims, expires, creds);
        }

        /// <inheritdoc/>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetSymmetricSecurityKey();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the symmetric security key from the configuration.
        /// </summary>
        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key not configured");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }

        /// <summary>
        /// Creates a JWT token.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <param name="expires">The expiration date of the token.</param>
        /// <param name="creds">The signing credentials for the token.</param>
        /// <returns>The created JWT token.</returns>
        private string CreateToken(List<Claim> claims, DateTime expires, SigningCredentials creds)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
