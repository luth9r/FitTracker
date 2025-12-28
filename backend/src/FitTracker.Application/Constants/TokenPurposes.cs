using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FitTracker.Application.Constants
{
    /// <summary>
    /// Contains constant values for JWT token purposes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TokenPurposes
    {
        public const string EmailVerification = "email_verification";
        public const string PasswordReset = "password_reset";
    }
}
