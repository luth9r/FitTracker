using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.ChangePassword;

/// <summary>
///     Represents a command to change a user's password in the system.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record ChangePasswordCommand(string OldPassword, string NewPassword, Guid UserId)
    : IRequest<Result<Unit, ValidationResult>>;
