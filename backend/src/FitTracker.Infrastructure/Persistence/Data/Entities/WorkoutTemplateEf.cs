namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a reusable workout template.
/// </summary>
public class WorkoutTemplateEf : BaseEntityEf
{
    /// <summary>
    ///     Gets or sets iD of the user who owns this template.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets template name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Gets or sets optional template description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets number of times this template has been used.
    /// </summary>
    public int UsageCount { get; set; }

    /// <summary>
    ///     Gets or sets timestamp of last template usage.
    /// </summary>
    public DateTime? LastUsedAt { get; set; }

    /// <summary>
    ///     Gets or sets navigation to the user.
    /// </summary>
    public UserEf? User { get; set; } = null!;

    /// <summary>
    ///     Gets or sets collection of exercises in this template.
    /// </summary>
    public ICollection<WorkoutTemplateExerciseEf> Exercises { get; set; } = new HashSet<WorkoutTemplateExerciseEf>();
}
