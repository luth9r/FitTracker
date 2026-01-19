using FitTracker.Domain.Abstract;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities.TemplateAggregate;

/// <summary>
///     Represents a planned set within a workout template exercise.
/// </summary>
public class TemplateSet : BaseEntity
{
    /// <summary>
    ///     The maximum weight allowed in kilograms.
    /// </summary>
    public const double MaxWeightKg = 10000d;

    /// <summary>
    ///     The maximum number of planned reps allowed.
    /// </summary>
    public const int MaxReps = 1000;

    /// <summary>
    ///     The maximum rest time allowed in seconds.
    /// </summary>
    public const int MaxRestSeconds = 3600;

    /// <summary>
    ///     Gets the unique identifier of the workout template exercise this set belongs to.
    /// </summary>
    public Guid WorkoutTemplateExerciseId { get; private set; }

    /// <summary>
    ///     Gets the sequential number of this set within the template exercise.
    /// </summary>
    public int SetNumber { get; private set; }

    /// <summary>
    ///     Gets the planned weight for this set.
    /// </summary>
    public double PlannedWeightKg { get; private set; }

    /// <summary>
    ///     Gets the planned number of repetitions for this set.
    /// </summary>
    public int PlannedReps { get; private set; }

    /// <summary>
    ///     Gets the planned rest period in seconds before the next set, or null if not specified.
    /// </summary>
    public int? RestSeconds { get; private set; }

    /// <summary>
    ///     Gets the type of this set (Normal, Dropset, Superset, etc.).
    /// </summary>
    public SetType SetType { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSet" /> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="workoutTemplateExerciseId">The unique identifier of the workout template exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="plannedWeight">The planned weight for the set.</param>
    /// <param name="plannedReps">The planned number of repetitions.</param>
    /// <param name="restSeconds">The planned rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    /// <param name="createdAt">The date and time of creation.</param>
    /// <param name="updatedAt">The date and time of the last update.</param>
    internal TemplateSet(
        Guid id,
        Guid workoutTemplateExerciseId,
        int setNumber,
        double plannedWeight,
        int plannedReps,
        int? restSeconds,
        SetType setType,
        DateTime createdAt,
        DateTime updatedAt)
        : base(id, createdAt, updatedAt)
    {
        WorkoutTemplateExerciseId = workoutTemplateExerciseId;
        SetNumber = setNumber;
        PlannedWeightKg = plannedWeight;
        PlannedReps = plannedReps;
        RestSeconds = restSeconds;
        SetType = setType;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSet" /> class.
    /// </summary>
    private TemplateSet()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplateSet" /> class.
    /// </summary>
    /// <param name="workoutTemplateExerciseId">The unique identifier of the workout template exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="plannedWeight">The planned weight for the set.</param>
    /// <param name="plannedReps">The planned number of repetitions.</param>
    /// <param name="restSeconds">The planned rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    private TemplateSet(
        Guid workoutTemplateExerciseId,
        int setNumber,
        double plannedWeight,
        int plannedReps,
        int? restSeconds,
        SetType setType = SetType.Normal)
    {
        if (workoutTemplateExerciseId == Guid.Empty)
        {
            throw new ArgumentException(
                "WorkoutTemplateExerciseId cannot be empty.",
                nameof(workoutTemplateExerciseId));
        }

        if (setNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(setNumber), setNumber, "Set number must be greater than 0.");
        }

        if (plannedWeight <= 0 || plannedWeight > MaxWeightKg)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedWeight),
                plannedWeight,
                $"Planned weight must be between 0 and {MaxWeightKg} kg.");
        }

        if (plannedReps <= 0 || plannedReps > MaxReps)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedReps),
                plannedReps,
                $"Planned reps must be between 1 and {MaxReps}.");
        }

        if (restSeconds.HasValue && (restSeconds.Value < 0 || restSeconds.Value > MaxRestSeconds))
        {
            throw new ArgumentOutOfRangeException(
                nameof(restSeconds),
                restSeconds,
                $"Rest seconds must be between 0 and {MaxRestSeconds}.");
        }

        WorkoutTemplateExerciseId = workoutTemplateExerciseId;
        SetNumber = setNumber;
        PlannedWeightKg = plannedWeight;
        PlannedReps = plannedReps;
        RestSeconds = restSeconds;
        SetType = setType;
    }

    /// <summary>
    ///     Creates a new <see cref="TemplateSet" />.
    /// </summary>
    /// <param name="workoutTemplateExerciseId">The unique identifier of the workout template exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="plannedWeight">The planned weight for the set.</param>
    /// <param name="plannedReps">The planned number of repetitions.</param>
    /// <param name="restSeconds">The planned rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    /// <returns>A new instance of <see cref="TemplateSet" />.</returns>
    public static TemplateSet Create(
        Guid workoutTemplateExerciseId,
        int setNumber,
        double plannedWeight,
        int plannedReps,
        int? restSeconds = null,
        SetType setType = SetType.Normal)
    {
        return new TemplateSet(workoutTemplateExerciseId, setNumber, plannedWeight, plannedReps, restSeconds, setType);
    }

    /// <summary>
    ///     Updates the template set details.
    /// </summary>
    /// <param name="plannedWeight">The new planned weight.</param>
    /// <param name="plannedReps">The new planned reps.</param>
    /// <param name="restSeconds">The new rest period in seconds.</param>
    public void Update(
        double plannedWeight,
        int plannedReps,
        int? restSeconds = null)
    {
        if (plannedWeight <= 0 || plannedWeight > MaxWeightKg)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedWeight),
                plannedWeight,
                $"Planned weight must be between 0 and {MaxWeightKg} kg.");
        }

        if (plannedReps <= 0 || plannedReps > MaxReps)
        {
            throw new ArgumentOutOfRangeException(
                nameof(plannedReps),
                plannedReps,
                $"Planned reps must be between 1 and {MaxReps}.");
        }

        if (restSeconds.HasValue && (restSeconds.Value < 0 || restSeconds.Value > MaxRestSeconds))
        {
            throw new ArgumentOutOfRangeException(
                nameof(restSeconds),
                restSeconds,
                $"Rest seconds must be between 0 and {MaxRestSeconds}.");
        }

        PlannedWeightKg = plannedWeight;
        PlannedReps = plannedReps;
        RestSeconds = restSeconds;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Updates the sequential number of the set.
    /// </summary>
    /// <param name="newSetNumber">The new set number.</param>
    public void UpdateSetNumber(int newSetNumber)
    {
        if (newSetNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newSetNumber),
                newSetNumber,
                "Set number must be greater than 0.");
        }

        SetNumber = newSetNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Changes the type of the set.
    /// </summary>
    /// <param name="setType">The new set type.</param>
    public void ChangeSetType(SetType setType)
    {
        SetType = setType;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Determines whether this set is a warmup set.
    /// </summary>
    /// <returns><c>true</c> if this set is a warmup set; otherwise, <c>false</c>.</returns>
    public bool IsWarmupSet()
    {
        return SetType == SetType.Warmup;
    }

    /// <summary>
    ///     Determines whether this set is a working set.
    /// </summary>
    /// <returns><c>true</c> if this set is a working set; otherwise, <c>false</c>.</returns>
    public bool IsWorkingSet()
    {
        return SetType == SetType.Normal;
    }

    /// <summary>
    ///     Determines whether this set has a specified rest period.
    /// </summary>
    /// <returns><c>true</c> if a rest period is specified; otherwise, <c>false</c>.</returns>
    public bool HasRestPeriod()
    {
        return RestSeconds.HasValue;
    }
}