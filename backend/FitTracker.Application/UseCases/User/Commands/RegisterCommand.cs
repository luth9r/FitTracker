using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Commands
{
    public record RegisterCommand(RegisterRequest User) : IRequest<Result<RegisterResponse, ValidationResult>>;
}
