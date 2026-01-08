using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands.Google;

/// <summary>
///     Command for registering with Google.
/// </summary>
/// <param name="Request">The <see cref="GoogleRegisterRequest" />.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleRegisterCommand(GoogleRegisterRequest Request)
    : IRequest<Result<LoginResponse, ValidationResult>>;