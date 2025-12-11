using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Users
{
    /// <summary>
    /// DTO representing a recent workout summary.
    /// </summary>
    /// <param name="Id">Unique identifier of the workout.</param>
    /// <param name="WorkoutDate">Date and time when the workout occurred.</param>
    /// <param name="Name">Name of the workout.</param>
    /// <param name="IsCompleted">Indicates whether the workout was completed.</param>
    /// <param name="DurationMinutes">Duration of the workout in minutes.</param>
    /// <param name="TotalVolume">Total volume (weight × reps) for the workout.</param>
    public sealed record RecentWorkoutResponse(Guid Id, DateTime WorkoutDate, string Name, bool IsCompleted, int DurationMinutes, double TotalVolume);
}
