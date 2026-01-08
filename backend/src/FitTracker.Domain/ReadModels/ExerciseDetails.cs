using FitTracker.Domain.Enums;

namespace FitTracker.Domain.ReadModels;

/// <summary>
///     Represents a comprehensive read model for exercise details, including metadata, personal records (PRs), cumulative
///     statistics, and historical performance data <see cref="ExerciseHistoryPoint" />.
/// </summary>
/// ///
/// <param name="Id">Unique identifier of the exercise.</param>
/// <param name="Name">The display name of the exercise.</param>
/// <param name="MuscleGroup">The primary muscle group targeted by this exercise.</param>
/// <param name="Equipment">The required equipment for the exercise.</param>
/// <param name="Description">Optional detailed instructions or notes.</param>
/// <param name="ImageUrl">URL to a thumbnail or illustrative image.</param>
/// <param name="VideoUrl">URL to a demonstration video.</param>
/// <param name="IsCustom">
///     Indicates if the exercise was created by a user (<c>true</c>) or is a system default (
///     <c>false</c>).
/// </param>
/// <param name="MaxWeightKg">The highest weight ever lifted in a single set.</param>
/// <param name="MaxReps">The highest number of repetitions ever performed in a single set.</param>
/// <param name="MaxVolume">The highest volume (Weight x Reps) achieved in a single set.</param>
/// <param name="MaxTotalVolume">The highest total volume achieved across all sets in a single workout session.</param>
/// <param name="MaxWeightDate">The date when the maximum weight record was achieved.</param>
/// <param name="MaxRepsDate">The date when the maximum repetitions record was achieved.</param>
/// <param name="MaxVolumeDate">The date when the maximum single-set volume record was achieved.</param>
/// <param name="MaxTotalVolumeDate">The date when the maximum session volume record was achieved.</param>
/// <param name="TotalWorkouts">Cumulative number of workouts where this exercise was performed.</param>
/// <param name="TotalSets">Cumulative number of completed sets for this exercise.</param>
/// <param name="TotalReps">Cumulative number of repetitions performed for this exercise.</param>
/// <param name="TotalLifted">Cumulative weight lifted across all sessions (tonnage).</param>
/// <param name="AvgWeightPerSet">Average weight lifted per set across all performances.</param>
/// <param name="AvgRepsPerSet">Average number of repetitions performed per set.</param>
/// <param name="LastPerformed">The date and time of the most recent performance of this exercise.</param>
/// <param name="VolumeHistory">A chronological list of total volume points for progress tracking (charts).</param>
public sealed record ExerciseDetails(

    // Main
    Guid Id,
    string Name,
    MuscleGroup MuscleGroup,
    Equipment Equipment,
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
    IReadOnlyList<ExerciseHistoryPoint> VolumeHistory);