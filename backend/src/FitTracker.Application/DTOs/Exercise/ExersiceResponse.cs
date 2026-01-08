using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Exercise;

/// <summary>
///     DTO representing an exercise data.
/// </summary>
/// <param name="Id">The unique id of the exercise.</param>
/// <param name="Name">The name of the exercise.</param>
/// <param name="Description">The description of the exercise.</param>
/// <param name="ImageUrl">The URL of the exercise image.</param>
/// <param name="VideoUrl">The URL of the exercise video.</param>
/// <param name="MuscleGroup">The display value of the muscle group targeted by the exercise.</param>
/// <param name="Equipment">The display value of the equipment required for the exercise.</param>
/// <param name="IsCustom">Indicates whether the exercise is a custom user-defined exercise.</param>
[ExcludeFromCodeCoverage]
public record ExerciseResponse(
    Guid Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string? VideoUrl,
    string MuscleGroup,
    string Equipment,
    bool IsCustom);