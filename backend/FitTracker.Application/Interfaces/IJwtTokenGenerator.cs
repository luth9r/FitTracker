using FitTracker.Domain.Entities;
using System.Security.Claims;

namespace FitTracker.Application.Interfaces
{
    /// <summary>
    /// Service for generating and validating JWT tokens.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The <see cref="User"/>.</param>
        /// <returns>The generated JWT token string.</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Generates a verification token for the given user.
        /// </summary>
        /// <param name="user">The <see cref="User"/>.</param>
        /// <returns>The generated verification token string.</returns>
        string GenerateVerificationToken(User user);

        /// <summary>
        /// Validates the given JWT token and returns the claims principal.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>The <see cref="ClaimsPrincipal"/> if valid; otherwise, null.</returns>
        ClaimsPrincipal? ValidateToken(string token);
    }
}
