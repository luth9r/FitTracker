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
        /// Generates an access token for the authenticated user.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <returns>A JWT access token.</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Generates an email verification token for the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A JWT email verification token.</returns>
        string GenerateVerificationToken(Guid userId);

        /// <summary>
        /// Generates a password reset token for the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A JWT password reset token.</returns>
        string GeneratePasswordResetToken(Guid userId);
    }
}
