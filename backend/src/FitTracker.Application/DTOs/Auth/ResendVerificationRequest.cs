using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// Request to resend the email verification link to a user.
    /// </summary>
    /// <param name="Email">The email address to send the verification link to.</param>
    [ExcludeFromCodeCoverage]
    public sealed record ResendVerificationRequest(string Email);
}
