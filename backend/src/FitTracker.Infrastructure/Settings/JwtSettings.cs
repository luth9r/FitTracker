using System.ComponentModel.DataAnnotations;

namespace FitTracker.Infrastructure.Settings
{
    /// <summary>
    /// Configuration settings for JWT token generation and validation.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        [Required]
        [MinLength(32)]
        public string Key { get; init; } = string.Empty;

        [Required]
        public string Issuer { get; init; } = string.Empty;

        [Required]
        public string Audience { get; init; } = string.Empty;

        [Range(1, 365)]
        public int TokenValidityInDays { get; init; }

        [Range(1, 1440)]
        public int EmailVerificationTokenValidityInMinutes { get; init; }

        [Range(1, 1440)]
        public int PasswordResetTokenValidityInMinutes { get; init; }
    }
}
