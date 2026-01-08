using FitTracker.Domain.Abstract;

namespace FitTracker.Domain.Entities;

/// <summary>
///     Represents an exercise within a workout session.
/// </summary>
public class WorkoutExercise : BaseEntity
{
    /// <summary>
    ///     The maximum length allowed for the notes.
    /// </summary>
    public const int NotesMaxLength = 500;

    /// <summary>
    ///     The maximum order index allowed for an exercise in a workout.
    /// </summary>
    public const int MaxOrderIndex = 1000;

    /// <summary>
    ///     Gets the unique identifier of the workout.
    /// </summary>
    public Guid WorkoutId { get; private set; }

    /// <summary>
    ///     Gets the unique identifier of the exercise.
    /// </summary>
    public Guid ExerciseId { get; private set; }

    /// <summary>
    ///     Gets the order index of this exercise in the workout.
    /// </summary>
    public int OrderIndex { get; private set; }

    /// <summary>
    ///     Gets the notes for this exercise in the workout.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkoutExercise" /> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="workoutId">The unique identifier of the workout.</param>
    /// <param name="exerciseId">The unique identifier of the exercise.</param>
    /// <param name="orderIndex">The order index.</param>
    /// <param name="notes">The notes.</param>
    /// <param name="createdAt">The date and time of creation.</param>
    /// <param name="updatedAt">The date and time of the last update.</param>
    internal WorkoutExercise(
        Guid id,
        Guid workoutId,
        Guid exerciseId,
        int orderIndex,
        string? notes,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        WorkoutId = workoutId;
        ExerciseId = exerciseId;
        OrderIndex = orderIndex;
        Notes = notes;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkoutExercise" /> class.
    /// </summary>
    private WorkoutExercise()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkoutExercise" /> class.
    /// </summary>
    /// <param name="workoutId">The unique identifier of the workout.</param>
    /// <param name="exerciseId">The unique identifier of the exercise.</param>
    /// <param name="orderIndex">The order index.</param>
    /// <param name="notes">The notes.</param>
    private WorkoutExercise(
        Guid workoutId,
        Guid exerciseId,
        int orderIndex,
        string? notes = null)
    {
        if (workoutId == Guid.Empty)
        {
            throw new ArgumentException("WorkoutId cannot be empty.", nameof(workoutId));
        }

        if (exerciseId == Guid.Empty)
        {
            throw new ArgumentException("ExerciseId cannot be empty.", nameof(exerciseId));
        }

        if (orderIndex < 1 || orderIndex > MaxOrderIndex)
        {
            throw new ArgumentOutOfRangeException(
                nameof(orderIndex),
                orderIndex,
                $"Order index must be between 1 and {MaxOrderIndex}.");
        }

        if (notes?.Length > NotesMaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(notes),
                notes.Length,
                $"Notes length must not exceed {NotesMaxLength} characters.");
        }

        WorkoutId = workoutId;
        ExerciseId = exerciseId;
        OrderIndex = orderIndex;
        Notes = notes;
    }

    /// <summary>
    ///     Creates a new <see cref="WorkoutExercise" />.
    /// </summary>
    /// <param name="workoutId">The unique identifier of the workout.</param>
    /// <param name="exerciseId">The unique identifier of the exercise.</param>
    /// <param name="orderIndex">The order index.</param>
    /// <param name="notes">The notes.</param>
    /// <returns>A new instance of <see cref="WorkoutExercise" />.</returns>
    public static WorkoutExercise Create(
        Guid workoutId,
        Guid exerciseId,
        int orderIndex,
        string? notes = null)
    {
        return new WorkoutExercise(workoutId, exerciseId, orderIndex, notes);
    }

    /// <summary>
    ///     Updates the order of the exercise.
    /// </summary>
    /// <param name="newOrder">The new order index.</param>
    public void UpdateOrder(int newOrder)
    {
        if (newOrder < 1 || newOrder > MaxOrderIndex)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newOrder),
                newOrder,
                $"Order index must be between 1 and {MaxOrderIndex}.");
        }

        OrderIndex = newOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Updates the notes for the exercise.
    /// </summary>
    /// <param name="notes">The new notes.</param>
    public void UpdateNotes(string? notes)
    {
        if (notes?.Length > NotesMaxLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(notes),
                notes.Length,
                $"Notes length must not exceed {NotesMaxLength} characters.");
        }

        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Determines whether this is the first exercise in the workout.
    /// </summary>
    /// <returns><c>true</c> if first; otherwise, <c>false</c>.</returns>
    public bool IsFirstExercise()
    {
        return OrderIndex == 1;
    }

    /// <summary>
    ///     Determines whether this uses the last possible order index.
    /// </summary>
    /// <returns><c>true</c> if last; otherwise, <c>false</c>.</returns>
    public bool IsLastExercise()
    {
        return OrderIndex == MaxOrderIndex;
    }

    /// <summary>
    ///     Moves the exercise up in the order (decrements index).
    /// </summary>
    public void MoveUp()
    {
        if (OrderIndex == 1)
        {
            throw new InvalidOperationException("Cannot move up from the first position.");
        }

        UpdateOrder(OrderIndex - 1);
    }

    /// <summary>
    ///     Moves the exercise down in the order (increments index).
    /// </summary>
    public void MoveDown()
    {
        if (OrderIndex == MaxOrderIndex)
        {
            throw new InvalidOperationException($"Cannot move down from the last position ({MaxOrderIndex}).");
        }

        UpdateOrder(OrderIndex + 1);
    }
}