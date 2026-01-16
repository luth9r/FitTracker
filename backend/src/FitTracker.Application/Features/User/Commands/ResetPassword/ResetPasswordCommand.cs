using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.ResetPassword;

/// <summary>
///     Command to reset a user's password using a password reset token.
/// </summary>
/// <param name="NewPassword">The new password for the user account.</param>
/// <param name="Token">The password reset token from the reset link.</param>
[ExcludeFromCodeCoverage]
public sealed record ResetPasswordCommand(string NewPassword, string Token) : IRequest<Result<Unit, ValidationResult>>;
