using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Users
{
    /// <summary>
    /// Response for user stats.
    /// </summary>
    public class UserStatsResponse
    {
        /// <summary>
        /// The total number of workouts.
        /// </summary>
        public int TotalWorkouts { get; init; }

        /// <summary>
        /// The total number of training days.
        /// </summary>
        public int TrainingDays { get; init; }

        /// <summary>
        /// The longest streak of workouts.
        /// </summary>
        public int LongestStreak { get; init; }

        /// <summary>
        /// The total weight lifted.
        /// </summary>
        public double TotalWeightLifted { get; init; }
    }
}
