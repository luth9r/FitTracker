using System.Diagnostics.CodeAnalysis;
using FitTracker.Domain.Enums;

namespace FitTracker.Application.Features.WorkoutTemplate.Queries.GetWorkoutTemplate;

/// <summary>
///     Represents the response containing details of a workout template,
///     including metadata, usage statistics, and associated exercises.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record GetTemplateWorkoutResponses(
    Guid Id,
    string Name,
    string? Description,
    int UsageCount,
    DateTime? LastUsedAt,
    List<WorkoutTemplateExerciseDto> Exercises);

/// <summary>
///     Represents an exercise in a workout template, including its identifier,
///     position in the sequence of exercises, optional notes, and the associated sets.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record WorkoutTemplateExerciseDto(
    Guid ExerciseId,
    int OrderIndex,
    string? Notes,
    List<WorkoutTemplateSetDto> Sets);

/// <summary>
///     Represents a specific set within a workout template, including its unique identifier,
///     set number, planned weight, planned repetitions, rest duration, and type of set.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record WorkoutTemplateSetDto(
    Guid Id,
    int SetNumber,
    double PlannedWeightKg,
    int PlannedReps,
    int? RestSeconds,
    SetType SetType);
