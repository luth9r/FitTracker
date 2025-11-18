using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google registration request.
    /// </summary>
    public class GoogleRegisterRequest
    {
        /// <summary>
        /// Authorization code received from Google.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Code verifier for PKCE flow.
        /// </summary>
        public string CodeVerifier { get; set; } = string.Empty;
    }
}
