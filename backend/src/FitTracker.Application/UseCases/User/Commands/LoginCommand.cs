using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.UseCases.User.Commands
{
    /// <summary>
    /// Command for logging in.
    /// </summary>
    /// <param name="Request">The <see cref="LoginRequest"/>.</param>
    [ExcludeFromCodeCoverage]
    public sealed record LoginCommand(LoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
