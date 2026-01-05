using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands;

/// <summary>
///     Command for registering a new user.
/// </summary>
/// <param name="User">The <see cref="RegisterRequest" />.</param>
[ExcludeFromCodeCoverage]
public sealed record RegisterCommand(RegisterRequest User) : IRequest<Result<RegisterResponse, ValidationResult>>;
