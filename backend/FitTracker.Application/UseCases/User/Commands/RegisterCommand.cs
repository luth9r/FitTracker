using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands
{
    /// <summary>
    /// Register Command
    /// </summary>
    /// <param name="User"></param>
    public record RegisterCommand(RegisterRequest User) : IRequest<Result<LoginResponse, ValidationResult>>;
}
