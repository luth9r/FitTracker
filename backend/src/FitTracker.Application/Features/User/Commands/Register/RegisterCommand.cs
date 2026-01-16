using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.Register;

/// <summary>
///     Command for registering a new user.
/// </summary>
/// <param name="Username">The username for the new user.</param>
/// <param name="Email">The email for the new user.</param>
/// <param name="Password">The password for the new user.</param>
[ExcludeFromCodeCoverage]
public sealed record RegisterCommand(string Username, string Email, string Password)
    : IRequest<Result<RegisterResponse, ValidationResult>>;
