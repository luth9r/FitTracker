using FitTracker.Domain.Abstract;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Entities;

/// <summary>
///     Represents a single set within a workout exercise.
/// </summary>
public class Set : BaseEntity
{
    /// <summary>
    ///     The maximum number of reps allowed in a set.
    /// </summary>
    public const int MaxReps = 1000;

    /// <summary>
    ///     The maximum rest time allowed in seconds.
    /// </summary>
    public const int MaxRestSeconds = 3600; // 1 hour

    /// <summary>
    ///     The maximum weight allowed in kilograms.
    /// </summary>
    public const double MaxWeightKg = 10000d;

    /// <summary>
    ///     Gets the unique identifier of the workout exercise this set belongs to.
    /// </summary>
    public Guid WorkoutExerciseId { get; private set; }

    /// <summary>
    ///     Gets the sequential number of this set within the workout exercise.
    /// </summary>
    public int SetNumber { get; private set; }

    /// <summary>
    ///     Gets the weight used for this set.
    /// </summary>
    public double WeightKg { get; private set; }

    /// <summary>
    ///     Gets the number of repetitions performed in this set.
    /// </summary>
    public int Reps { get; private set; }

    /// <summary>
    ///     Gets the rest period in seconds before the next set, or null if not specified.
    /// </summary>
    public int? RestSeconds { get; private set; }

    /// <summary>
    ///     Gets the type of this set (Normal, Dropset, Superset, etc.).
    /// </summary>
    public SetType SetType { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether this set has been completed.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    ///     Gets the date and time when this set was completed, or null if not yet completed.
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Set" /> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="workoutExerciseId">The unique identifier of the workout exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="weight">The weight used.</param>
    /// <param name="reps">The number of repetitions.</param>
    /// <param name="restSeconds">The rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    /// <param name="isCompleted">Whether the set is completed.</param>
    /// <param name="completedAt">The date and time of completion.</param>
    /// <param name="createdAt">The date and time of creation.</param>
    /// <param name="updatedAt">The date and time of the last update.</param>
    internal Set(
        Guid id,
        Guid workoutExerciseId,
        int setNumber,
        double weight,
        int reps,
        int? restSeconds,
        SetType setType,
        bool isCompleted,
        DateTime? completedAt,
        DateTime createdAt,
        DateTime updatedAt)
        : base(id, createdAt, updatedAt)
    {
        WorkoutExerciseId = workoutExerciseId;
        SetNumber = setNumber;
        WeightKg = weight;
        Reps = reps;
        RestSeconds = restSeconds;
        SetType = setType;
        IsCompleted = isCompleted;
        CompletedAt = completedAt;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Set" /> class.
    /// </summary>
    private Set()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Set" /> class.
    /// </summary>
    /// <param name="workoutExerciseId">The unique identifier of the workout exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="weight">The weight used.</param>
    /// <param name="reps">The number of repetitions.</param>
    /// <param name="restSeconds">The rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    private Set(
        Guid workoutExerciseId,
        int setNumber,
        double weight,
        int reps,
        int? restSeconds,
        SetType setType = SetType.Normal)
    {
        if (workoutExerciseId == Guid.Empty)
        {
            throw new ArgumentException("WorkoutExerciseId cannot be empty.", nameof(workoutExerciseId));
        }

        if (setNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(setNumber), "Set number must be greater than 0.");
        }

        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be greater than 0.");
        }

        if (weight > MaxWeightKg)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), $"Weight cannot exceed {MaxWeightKg} kg.");
        }

        if (reps <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(reps), "Reps must be greater than 0.");
        }

        if (reps > MaxReps)
        {
            throw new ArgumentOutOfRangeException(nameof(reps), $"Reps cannot exceed {MaxReps}.");
        }

        if (restSeconds is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(restSeconds), "Rest seconds cannot be negative.");
        }

        if (restSeconds > MaxRestSeconds)
        {
            throw new ArgumentOutOfRangeException(nameof(restSeconds), $"Rest cannot exceed {MaxRestSeconds} seconds.");
        }

        WorkoutExerciseId = workoutExerciseId;
        SetNumber = setNumber;
        WeightKg = weight;
        Reps = reps;
        RestSeconds = restSeconds;
        SetType = setType;
        IsCompleted = false;
    }

    /// <summary>
    ///     Creates a new <see cref="Set" />.
    /// </summary>
    /// <param name="workoutExerciseId">The unique identifier of the workout exercise.</param>
    /// <param name="setNumber">The sequential number of the set.</param>
    /// <param name="weight">The weight used.</param>
    /// <param name="reps">The number of repetitions.</param>
    /// <param name="restSeconds">The rest period in seconds.</param>
    /// <param name="setType">The type of the set.</param>
    /// <returns>A new instance of <see cref="Set" />.</returns>
    public static Set Create(
        Guid workoutExerciseId,
        int setNumber,
        double weight,
        int reps,
        int? restSeconds = null,
        SetType setType = SetType.Normal)
    {
        return new Set(workoutExerciseId, setNumber, weight, reps, restSeconds, setType);
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
    ///     Updates the weight used in the set.
    /// </summary>
    /// <param name="weight">The new weight.</param>
    public void UpdateWeight(double weight)
    {
        if (weight < 0 || weight > MaxWeightKg)
        {
            throw new ArgumentOutOfRangeException(
                nameof(weight),
                weight,
                $"Weight must be between 0 and {MaxWeightKg} kg.");
        }

        WeightKg = weight;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Increases the weight used in the set by a specified amount in kilograms.
    /// </summary>
    /// <param name="amountKg">The amount to increase in kilograms.</param>
    public void IncreaseWeight(double amountKg)
    {
        if (amountKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountKg), amountKg, "Amount to increase must be positive.");
        }

        var newWeightKg = WeightKg + amountKg;
        if (newWeightKg > MaxWeightKg)
        {
            throw new InvalidOperationException(
                $"Resulting weight {newWeightKg} kg would exceed the maximum allowed of {MaxWeightKg} kg.");
        }

        WeightKg = newWeightKg;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Decreases the weight used in the set by a specified amount in kilograms.
    /// </summary>
    /// <param name="amountKg">The amount to decrease in kilograms.</param>
    public void DecreaseWeight(double amountKg)
    {
        if (amountKg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountKg), "Amount to decrease must be greater than zero.");
        }

        var newWeightKg = WeightKg - amountKg;
        if (newWeightKg < 0)
        {
            throw new InvalidOperationException(
                $"Resulting weight cannot be negative. Current: {WeightKg}, Decrease by: {amountKg}");
        }

        WeightKg = newWeightKg;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Updates the number of repetitions performed in the set.
    /// </summary>
    /// <param name="reps">The new number of repetitions.</param>
    public void UpdateReps(int reps)
    {
        if (reps <= 0 || reps > MaxReps)
        {
            throw new ArgumentOutOfRangeException(nameof(reps), reps, $"Reps must be between 1 and {MaxReps}.");
        }

        Reps = reps;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Updates the rest period for the set.
    /// </summary>
    /// <param name="seconds">The new rest period in seconds.</param>
    public void UpdateRest(int? seconds)
    {
        if (seconds is < 0 && seconds.Value >= MaxRestSeconds)
        {
            throw new ArgumentOutOfRangeException(
                nameof(seconds),
                seconds,
                $"Rest seconds must be between 0 and {MaxRestSeconds}.");
        }

        RestSeconds = seconds;
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
    ///     Marks the set as completed.
    /// </summary>
    public void Complete()
    {
        if (IsCompleted)
        {
            return;
        }

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the set as not completed.
    /// </summary>
    public void Uncomplete()
    {
        if (!IsCompleted)
        {
            return;
        }

        IsCompleted = false;
        CompletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Determines whether this set is a personal record compared to previous sets.
    /// </summary>
    /// <param name="previousSets">A collection of previous sets to compare against.</param>
    /// <returns><c>true</c> if this set is a personal record; otherwise, <c>false</c>.</returns>
    public bool IsPR(IEnumerable<Set> previousSets)
    {
        if (previousSets == null || !previousSets.Any())
        {
            return true;
        }

        var maxPreviousWeight = previousSets.Max(s => s.WeightKg);
        return WeightKg > maxPreviousWeight;
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
}