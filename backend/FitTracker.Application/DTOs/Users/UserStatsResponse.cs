using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Users
{
    /// <summary>
    /// DTO representing user workout statistics.
    /// </summary>
    /// <param name="TotalWorkouts">The total number of workouts completed.</param>
    /// <param name="TrainingDays">The total number of training days.</param>
    /// <param name="LongestStreak">The longest consecutive streak of workouts.</param>
    /// <param name="TotalWeightLifted">The total weight lifted across all workouts.</param>
    public sealed record UserStatsResponse(int TotalWorkouts, int TrainingDays, int LongestStreak, double TotalWeightLifted);
}
