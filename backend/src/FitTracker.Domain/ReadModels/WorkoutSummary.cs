namespace FitTracker.Domain.ReadModels
{
    /// <summary>
    /// Represents a summary view of a workout, including basic details and total training volume.
    /// </summary>
    public record WorkoutSummary(Guid Id, DateTime WorkoutDate, string Name, bool IsCompleted, int DurationMinutes, double TotalVolumeKg);
}
