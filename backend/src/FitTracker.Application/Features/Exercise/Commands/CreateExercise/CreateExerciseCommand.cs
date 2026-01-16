using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Domain.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FitTracker.Application.Features.Exercise.Commands.CreateExercise;

/// <summary>
///     Represents a command to create a new exercise with specified attributes.
/// </summary>
/// <param name="Name">The name of the exercise.</param>
/// <param name="MuscleGroup">The muscle group of the exercise.</param>
/// <param name="Equipment">The equipment required for the exercise.</param>
/// <param name="Description">The description of the exercise.</param>
/// <param name="Image">The image of the exercise.</param>
[ExcludeFromCodeCoverage]
public sealed record CreateExerciseCommand(
    string Name,
    MuscleGroup MuscleGroup,
    Equipment Equipment,
    string? Description,
    IFormFile? Image,
    Guid UserId) : IRequest<Result<Unit, ValidationResult>>;
