namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout template.
    /// </summary>
    public class WorkoutTemplateExercise : BaseEntity
    {
        /// <summary>
        /// The maximum length allowed for the notes.
        /// </summary>
        public const int NotesMaxLength = 500;

        /// <summary>
        /// The minimum order index allowed.
        /// </summary>
        public const int MinOrderIndex = 1;

        /// <summary>
        /// The maximum order index allowed.
        /// </summary>
        public const int MaxOrderIndex = 1000;

        /// <summary>
        /// Gets the unique identifier of the workout template.
        /// </summary>
        public Guid WorkoutTemplateId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the exercise.
        /// </summary>
        public Guid ExerciseId { get; private set; }

        /// <summary>
        /// Gets the order index of this exercise in the template.
        /// </summary>
        public int OrderIndex { get; private set; }

        /// <summary>
        /// Gets the notes for this exercise in the template.
        /// </summary>
        public string? Notes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplateExercise"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="workoutTemplateId">The unique identifier of the workout template.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <param name="orderIndex">The order index.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
        internal WorkoutTemplateExercise(
            Guid id,
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            WorkoutTemplateId = workoutTemplateId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplateExercise"/> class.
        /// </summary>
        private WorkoutTemplateExercise()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkoutTemplateExercise"/> class.
        /// </summary>
        /// <param name="workoutTemplateId">The unique identifier of the workout template.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <param name="orderIndex">The order index.</param>
        /// <param name="notes">The notes.</param>
        private WorkoutTemplateExercise(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
            : base()
        {
            if (workoutTemplateId == Guid.Empty)
            {
                throw new ArgumentException("WorkoutTemplateId cannot be empty", nameof(workoutTemplateId));
            }

            if (exerciseId == Guid.Empty)
            {
                throw new ArgumentException("ExerciseId cannot be empty", nameof(exerciseId));
            }

            if (orderIndex < MinOrderIndex || orderIndex > MaxOrderIndex)
            {
                throw new ArgumentException($"Order index must be {MinOrderIndex}-{MaxOrderIndex}", nameof(orderIndex));
            }

            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            WorkoutTemplateId = workoutTemplateId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
        }

        /// <summary>
        /// Creates a new <see cref="WorkoutTemplateExercise"/>.
        /// </summary>
        /// <param name="workoutTemplateId">The unique identifier of the workout template.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <param name="orderIndex">The order index.</param>
        /// <param name="notes">The notes.</param>
        /// <returns>A new instance of <see cref="WorkoutTemplateExercise"/>.</returns>
        public static WorkoutTemplateExercise Create(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            return new WorkoutTemplateExercise(workoutTemplateId, exerciseId, orderIndex, notes);
        }

        /// <summary>
        /// Updates the order of the exercise.
        /// </summary>
        /// <param name="newOrder">The new order index.</param>
        public void UpdateOrder(int newOrder)
        {
            if (newOrder < MinOrderIndex || newOrder > MaxOrderIndex)
            {
                throw new ArgumentException($"Order index must be {MinOrderIndex}-{MaxOrderIndex}", nameof(newOrder));
            }

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the notes for the exercise.
        /// </summary>
        /// <param name="notes">The new notes.</param>
        public void UpdateNotes(string? notes)
        {
            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Determines whether this is the first exercise in the template.
        /// </summary>
        /// <returns><c>true</c> if first; otherwise, <c>false</c>.</returns>
        public bool IsFirstExercise() => OrderIndex == MinOrderIndex;

        /// <summary>
        /// Determines whether this uses the last possible order index.
        /// </summary>
        /// <returns><c>true</c> if last; otherwise, <c>false</c>.</returns>
        public bool IsLastExercise() => OrderIndex == MaxOrderIndex;

        /// <summary>
        /// Moves the exercise up in the order (decrements index).
        /// </summary>
        public void MoveUp() => UpdateOrder(OrderIndex - 1);

        /// <summary>
        /// Moves the exercise down in the order (increments index).
        /// </summary>
        public void MoveDown() => UpdateOrder(OrderIndex + 1);
    }
}
