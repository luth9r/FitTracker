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
    /// Login Command
    /// </summary>
    /// <param name="Request"></param>
    public record LoginCommand(LoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
