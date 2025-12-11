using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google registration request.
    /// </summary>
    /// <param name="Code">Authorization code received from Google.</param>
    /// <param name="CodeVerifier">Code verifier for PKCE flow.</param>
    public sealed record GoogleRegisterRequest(string Code, string CodeVerifier);
}
