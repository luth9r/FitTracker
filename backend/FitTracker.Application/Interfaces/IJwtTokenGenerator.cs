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
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateToken(User user);

        /// <summary>
        /// Generates a verification token for the given user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateVerificationToken(User user);

        /// <summary>
        /// Validates the given JWT token and returns the claims principal.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal? ValidateToken(string token);
    }
}
