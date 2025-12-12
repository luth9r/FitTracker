using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands.Google
{
    /// <summary>
    /// Command for registering with Google.
    /// </summary>
    /// <param name="Request">The <see cref="GoogleRegisterRequest"/>.</param>
    [ExcludeFromCodeCoverage]
    public record GoogleRegisterCommand(GoogleRegisterRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
