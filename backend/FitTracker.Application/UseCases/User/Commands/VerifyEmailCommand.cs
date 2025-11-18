using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands
{
    /// <summary>
    /// Verify Email Command
    /// </summary>
    /// <param name="Token"></param>
    public record VerifyEmailCommand(string Token) : IRequest<Result<LoginResponse, ValidationResult>>;
}
