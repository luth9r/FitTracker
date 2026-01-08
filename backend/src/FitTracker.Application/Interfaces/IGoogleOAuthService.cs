using FitTracker.Application.DTOs.Auth.Google;

namespace FitTracker.Application.Interfaces;

/// <summary>
///     Service for handling Google OAuth operations.
/// </summary>
public interface IGoogleOAuthService
{
    /// <summary>
    ///     Exchanges authorization code for tokens.
    /// </summary>
    /// <param name="code">The authorization code.</param>
    /// <param name="codeVerifier">The code verifier.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> representing the asynchronous operation, containing the
    ///     <see cref="TokenResponse" />.
    /// </returns>
    Task<TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier);

    /// <summary>
    ///     Validates the given ID token and returns its payload.
    /// </summary>
    /// <param name="idToken">The ID token to validate.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> representing the asynchronous operation, containing the
    ///     <see cref="GoogleTokenPayload" /> if validation is successful; otherwise, null.
    /// </returns>
    Task<GoogleTokenPayload?> ValidateAsync(string idToken);
}