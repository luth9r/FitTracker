namespace FitTracker.Domain.ReadModels
{
    /// <summary>
    /// Represents a single data point in the exercise performance history.
    /// </summary>
    /// <param name="Date">The calendar date when the exercise was performed.</param>
    /// <param name="Value">The metric value for that date (e.g., total volume, max weight, or estimated 1RM).</param>
    public sealed record ExerciseHistoryPoint(
        DateOnly Date,
        double Value);
}
