using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands;

/// <summary>
///     Query to resend email verification link to a user who has not yet verified their account.
/// </summary>
/// <param name="Email">The email address to send the verification link to.</param>
[ExcludeFromCodeCoverage]
public sealed record ResendVerificationEmailCommand(string Email) : IRequest<Result<Unit, ValidationResult>>;
