using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;

/// <summary>
///     Represents a command to create a new workout template.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record CreateTemplateWorkoutCommand(
    Guid UserId,
    string Name,
    string? Description,
    List<CreateTemplateExerciseDto> Exercises) : IRequest<Result<Unit, ValidationResult>>;
