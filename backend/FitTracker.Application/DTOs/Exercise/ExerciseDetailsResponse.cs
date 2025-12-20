using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Application.DTOs.Exercise
{
    /// <summary>
    /// Represents detailed information about an exercise, including metadata, performance records,
    /// and historical training volume for presentation in the API layer.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record ExerciseDetailsResponse(

        // Main
        Guid Id,
        string Name,
        string MuscleGroup,
        string Equipment,
        string? Description,
        string? ImageUrl,
        string? VideoUrl,
        bool IsCustom,

        // PR / records
        double MaxWeightKg,
        int MaxReps,
        double MaxVolume,
        double MaxTotalVolume,
        DateTime? MaxWeightDate,
        DateTime? MaxRepsDate,
        DateTime? MaxVolumeDate,
        DateTime? MaxTotalVolumeDate,

        int TotalWorkouts,
        int TotalSets,
        int TotalReps,
        double TotalLifted,
        double AvgWeightPerSet,
        double AvgRepsPerSet,
        DateTime? LastPerformed,
        IReadOnlyList<ExerciseHistoryPointResponse> VolumeHistory);

    /// <summary>
    /// Represents a single point in the exercise volume history, containing the workout date and total volume for that day.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record ExerciseHistoryPointResponse(
        string Date,
        double Value);
}
