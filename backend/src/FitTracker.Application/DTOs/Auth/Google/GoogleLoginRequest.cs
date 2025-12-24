using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google login request containing authorization code and code verifier.
    /// </summary>
    /// <param name="Code">The authorization code received from Google after user consent.</param>
    /// <param name="CodeVerifier">The code verifier used in PKCE flow.</param>
    [ExcludeFromCodeCoverage]
    public sealed record GoogleLoginRequest(string Code, string CodeVerifier);
}
