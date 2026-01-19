namespace FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

/// <summary>
///     Represents an exercise within a workout template.
/// </summary>
public class TemplateWorkoutExerciseEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets iD of the workout template.
    /// </summary>
    public Guid WorkoutTemplateId { get; set; }

    /// <summary>
    ///     Gets or sets iD of the exercise.
    /// </summary>
    public Guid ExerciseId { get; set; }

    /// <summary>
    ///     Gets or sets the order of the exercise in the template.
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    ///     Gets or sets optional notes for this exercise.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    ///     Gets or sets navigation to the workout template.
    /// </summary>
    public TemplateWorkoutEf? WorkoutTemplate { get; set; } = null!;

    /// <summary>
    ///     Gets or sets navigation to the exercise.
    /// </summary>
    public ExerciseEf? Exercise { get; set; } = null!;

    /// <summary>
    ///     Gets or sets a collection of planned sets for this exercise.
    /// </summary>
    public ICollection<TemplateSetEf> PlannedSets { get; set; } = new HashSet<TemplateSetEf>();
}