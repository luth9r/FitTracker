using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.ReadModels
{
    public record WorkoutSummary(Guid Id, DateTime WorkoutDate, string Name, bool IsCompleted, int DurationMinutes, double TotalVolumeKg);
}
