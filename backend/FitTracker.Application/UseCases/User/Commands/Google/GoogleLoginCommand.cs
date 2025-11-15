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
    public record GoogleLoginCommand(GoogleLoginRequest Request) : IRequest<Result<LoginResponse, ValidationResult>>;
}
