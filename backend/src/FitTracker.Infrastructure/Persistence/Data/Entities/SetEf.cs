namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a completed set in a workout.
/// </summary>
public class SetEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets iD of the workout exercise.
    /// </summary>
    public Guid WorkoutExerciseId { get; set; }

    /// <summary>
    ///     Gets or sets set number in the exercise sequence.
    /// </summary>
    public int SetNumber { get; set; }

    /// <summary>
    ///     Gets or sets weight used in kilograms.
    /// </summary>
    public double WeightKg { get; set; }

    /// <summary>
    ///     Gets or sets number of repetitions completed.
    /// </summary>
    public int Reps { get; set; }

    /// <summary>
    ///     Gets or sets optional rest period in seconds.
    /// </summary>
    public int? RestSeconds { get; set; }

    /// <summary>
    ///     Gets or sets type of the set (normal, warmup, drop set, etc.).
    /// </summary>
    public int SetType { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether indicates if the set was completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    ///     Gets or sets timestamp when the set was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    ///     Gets or sets navigation to the workout exercise.
    /// </summary>
    public WorkoutExerciseEf? WorkoutExercise { get; set; } = null!;
}