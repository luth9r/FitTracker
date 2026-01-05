using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands.Google;

/// <summary>
///     Command for logging in with Google.
/// </summary>
/// <param name="Request">The <see cref="GoogleLoginRequest" />.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleLoginCommand(GoogleLoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
