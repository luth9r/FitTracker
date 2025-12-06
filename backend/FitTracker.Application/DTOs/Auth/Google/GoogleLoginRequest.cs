using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google login request containing authorization code and code verifier.
    /// </summary>
    public class GoogleLoginRequest
    {
        /// <summary>
        /// Gets or sets the authorization code received from Google after user consent.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the code verifier used in PKCE flow.
        /// </summary>
        public string CodeVerifier { get; set; } = string.Empty;
    }
}
