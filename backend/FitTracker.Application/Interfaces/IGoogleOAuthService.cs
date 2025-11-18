using FitTracker.Application.DTOs.Auth.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Interfaces
{
    /// <summary>
    /// Service for handling Google OAuth operations.
    /// </summary>
    public interface IGoogleOAuthService
    {
        /// <summary>
        /// Exchanges authorization code for tokens.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeVerifier"></param>
        /// <returns></returns>
        Task<TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier);

        /// <summary>
        /// Validates the given ID token and returns its payload.
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        Task<GoogleTokenPayload?> ValidateAsync(string idToken);
    }
}
