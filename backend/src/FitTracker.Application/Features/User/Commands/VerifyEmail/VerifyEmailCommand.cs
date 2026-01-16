using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.User.Common;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.VerifyEmail;

/// <summary>
///     Command for verifying an email address.
/// </summary>
/// <param name="Token">The verification token.</param>
[ExcludeFromCodeCoverage]
public sealed record VerifyEmailCommand(string Token) : IRequest<Result<LoginResponse, ValidationResult>>;
