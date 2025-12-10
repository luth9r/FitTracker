using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.ReadModels
{
    public class WorkoutSummary
    {
        public Guid Id { get; init; }

        public DateTime WorkoutDate { get; init; }

        public string Name { get; init; } = default!;

        public bool IsCompleted { get; init; }

        public int DurationMinutes { get; init; }

        public decimal TotalVolumeKg { get; init; }
    }
}
