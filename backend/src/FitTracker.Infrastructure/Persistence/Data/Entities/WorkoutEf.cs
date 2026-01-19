using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a workout session.
/// </summary>
public class WorkoutEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets iD of the user performing the workout.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets optional ID of the template used to create this workout.
    /// </summary>
    public Guid? WorkoutTemplateId { get; set; }

    /// <summary>
    ///     Gets or sets workout name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Gets or sets optional workout notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    ///     Gets or sets date of the workout.
    /// </summary>
    public DateTime WorkoutDate { get; set; }

    /// <summary>
    ///     Gets or sets total duration of the workout.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether indicates if the workout is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether indicates if the workout is currently in progress.
    /// </summary>
    public bool IsInProgress { get; set; }

    /// <summary>
    ///     Gets or sets timestamp when the workout was started.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    ///     Gets or sets timestamp when the workout was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    ///     Gets or sets total volume lifted in kilograms.
    /// </summary>
    public double TotalVolumeKg { get; set; }

    /// <summary>
    ///     Gets or sets navigation to the user.
    /// </summary>
    public UserEf? User { get; set; } = null!;

    /// <summary>
    ///     Gets or sets navigation to the workout template (if used).
    /// </summary>
    public TemplateWorkoutEf? WorkoutTemplate { get; set; }

    /// <summary>
    ///     Gets or sets collection of exercises performed in this workout.
    /// </summary>
    public ICollection<WorkoutExerciseEf> Exercises { get; set; } = new HashSet<WorkoutExerciseEf>();
}