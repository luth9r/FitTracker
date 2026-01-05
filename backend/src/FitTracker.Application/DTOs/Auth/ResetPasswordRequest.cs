using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Auth;

/// <summary>
///     Request model for resetting a user's password.
/// </summary>
/// <param name="NewPassword">The new password for the user account.</param>
[ExcludeFromCodeCoverage]
public sealed record ResetPasswordRequest(string NewPassword);
