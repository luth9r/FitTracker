using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands.Google;

/// <summary>
///     Represents a command for authenticating a user via Google's mobile authentication flow.
/// </summary>
/// <param name="Request">
///     The request object containing the necessary authentication details, such as the Google authentication code.
/// </param>
[ExcludeFromCodeCoverage]
public sealed record GoogleMobileAuthCommand(GoogleMobileAuthRequest Request)
    : IRequest<Result<LoginResponse, ValidationResult>>;
