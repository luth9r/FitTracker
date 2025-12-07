using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.UseCases.User.Commands
{
    /// <summary>
    /// Command for logging in.
    /// </summary>
    /// <param name="Request">The <see cref="LoginRequest"/>.</param>
    public record LoginCommand(LoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
