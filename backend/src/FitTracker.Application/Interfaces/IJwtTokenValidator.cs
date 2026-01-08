using System.Security.Claims;
using CSharpFunctionalExtensions;

namespace FitTracker.Application.Interfaces;

public interface IJwtTokenValidator
{
    /// <summary>
    ///     Validates the provided JWT token and extracts claims.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The claims principal if valid; otherwise, null.</returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    ///     Validates the token and ensures it contains the expected purpose claim and a valid user identifier.
    /// </summary>
    /// <param name="token">The JWT token string to validate.</param>
    /// <param name="expectedPurpose">The required value for the 'purpose' claimd.</param>
    /// <returns>
    ///     A <see cref="Result{Guid}" /> containing the User ID if validation succeeds; otherwise, a failure result explaining
    ///     the reason.
    /// </returns>
    Result<Guid> ValidatePurposeToken(string token, string expectedPurpose);
}