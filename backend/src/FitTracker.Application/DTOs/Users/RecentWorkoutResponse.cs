using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Users;

/// <summary>
///     DTO representing a recent workout summary.
/// </summary>
/// <param name="Id">Unique identifier of the workout.</param>
/// <param name="WorkoutDate">Date and time when the workout occurred.</param>
/// <param name="Name">Name of the workout.</param>
/// <param name="IsCompleted">Indicates whether the workout was completed.</param>
/// <param name="DurationMinutes">Duration of the workout in minutes.</param>
/// <param name="TotalVolumeKg">Total volume (weight × reps) for the workout.</param>
[ExcludeFromCodeCoverage]
public sealed record RecentWorkoutResponse(
    Guid Id,
    DateTime WorkoutDate,
    string Name,
    bool IsCompleted,
    int DurationMinutes,
    double TotalVolumeKg);
