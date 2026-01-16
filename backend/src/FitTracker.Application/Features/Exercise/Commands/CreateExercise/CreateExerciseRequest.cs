using System.Diagnostics.CodeAnalysis;
using FitTracker.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace FitTracker.Application.Features.Exercise.Commands.CreateExercise;

/// <summary>
///     Represents the request object for creating a new exercise.
/// </summary>
/// <param name="Name">
///     The name of the exercise. This field is required to identify the exercise.
/// </param>
/// <param name="MuscleGroup">
///     The target muscle group for the exercise. Must be a valid enum value from <see cref="MuscleGroup" />.
/// </param>
/// <param name="Equipment">
///     The type of equipment required for the exercise. Must be a valid enum value from <see cref="Equipment" />.
/// </param>
/// <param name="Description">
///     An optional description providing additional details about the exercise.
/// </param>
/// <param name="Image">
///     An optional image file representing the exercise. Must be a valid instance of
///     <see cref="Microsoft.AspNetCore.Http.IFormFile" />.
/// </param>
[ExcludeFromCodeCoverage]
public sealed record CreateExerciseRequest(
    string Name,
    MuscleGroup MuscleGroup,
    Equipment Equipment,
    string? Description,
    IFormFile? Image);
