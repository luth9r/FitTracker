using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands.Google;

/// <summary>
///     Represents a command for authenticating a user via Google's mobile authentication flow.
/// </summary>
/// <param name="Code">The authorization code returned from Google.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleMobileAuthCommand(string Code)
    : IRequest<Result<LoginResponse, ValidationResult>>;
