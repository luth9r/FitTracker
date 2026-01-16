using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.User.Common;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.GoogleRegister;

/// <summary>
///     Command for registering with Google.
/// </summary>
/// <param name="Code">The authorization code returned from Google.</param>
/// <param name="CodeVerifier">The code verifier used in PKCE flow.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleRegisterCommand(string Code, string CodeVerifier)
    : IRequest<Result<LoginResponse, ValidationResult>>;
