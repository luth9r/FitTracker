using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.ChangePassword;

/// <summary>
///     Represents the request to change a user's password.
/// </summary>
/// <param name="OldPassword">
///     The current password of the user, which will be verified before
///     applying the password change.
/// </param>
/// <param name="NewPassword">
///     The new password specified by the user, which will replace the current password.
/// </param>
[ExcludeFromCodeCoverage]
public sealed record ChangePasswordRequest(string OldPassword, string NewPassword);
