using System.Diagnostics.CodeAnalysis;
using FitTracker.Domain.Enums;

namespace FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;

/// <summary>
///     Represents the request object for creating a new workout template.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record CreateTemplateWorkoutRequest(
    string Name,
    string? Description,
    List<CreateTemplateExerciseDto> Exercises);

[ExcludeFromCodeCoverage]
public sealed record CreateTemplateExerciseDto(
    Guid ExerciseId,
    int OrderIndex,
    string? Notes,
    List<CreateTemplateSetDto> Sets);

[ExcludeFromCodeCoverage]
public sealed record CreateTemplateSetDto(
    int SetNumber,
    double Weight,
    int Reps,
    int? Rest,
    SetType Type);
