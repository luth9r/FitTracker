using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Users
{
    public class RecentWorkoutResponse
    {
        public Guid Id { get; init; }

        public DateTime WorkoutDate { get; init; }

        public string Name { get; init; } = default!;

        public bool IsCompleted { get; init; }

        public int DurationMinutes { get; init; }

        public double TotalVolume { get; init; }
    }
}
