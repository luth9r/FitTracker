using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.UseCases.User.Commands.Google
{
    /// <summary>
    /// Command for registering with Google.
    /// </summary>
    /// <param name="Request"></param>
    public record GoogleRegisterCommand(GoogleRegisterRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
