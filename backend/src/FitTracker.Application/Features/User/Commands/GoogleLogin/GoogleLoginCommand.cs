using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.User.Common;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.GoogleLogin;

/// <summary>
///     Command for logging in with Google.
/// </summary>
/// <param name="Code">The authorization code returned from Google.</param>
/// <param name="CodeVerifier">The code verifier used in PKCE flow.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleLoginCommand(string Code, string CodeVerifier)
    : IRequest<Result<LoginResponse, ValidationResult>>;
