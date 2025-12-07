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
    /// Command for logging in with Google.
    /// </summary>
    /// <param name="Request">The <see cref="GoogleLoginRequest"/>.</param>
    public record GoogleLoginCommand(GoogleLoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
