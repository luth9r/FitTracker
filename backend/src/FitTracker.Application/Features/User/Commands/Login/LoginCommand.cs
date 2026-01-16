using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.User.Common;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.User.Commands.Login;

/// <summary>
///     Command for logging in.
/// </summary>
/// <param name="Email">User's email address.</param>
/// <param name="Password">User's password.</param>
[ExcludeFromCodeCoverage]
public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse, ValidationResult>>;
