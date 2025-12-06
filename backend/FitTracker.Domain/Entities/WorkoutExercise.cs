namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout session.
    /// </summary>
    public class WorkoutExercise : BaseEntity
    {
        public const int NotesMaxLength = 500;
        public const int MaxOrderIndex = 1000;

        public Guid WorkoutId { get; private set; }

        public Guid ExerciseId { get; private set; }

        public int OrderIndex { get; private set; }

        public string? Notes { get; private set; }

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

        private WorkoutExercise()
        {
        }

        private WorkoutExercise(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            if (workoutId == Guid.Empty)
            {
                throw new ArgumentException("WorkoutId cannot be empty", nameof(workoutId));
            }

            if (exerciseId == Guid.Empty)
            {
                throw new ArgumentException("ExerciseId cannot be empty", nameof(exerciseId));
            }

            if (orderIndex <= 0 || orderIndex > MaxOrderIndex)
            {
                throw new ArgumentException($"Order index must be 1-{MaxOrderIndex}", nameof(orderIndex));
            }

            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
        }

        public static WorkoutExercise Create(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            return new WorkoutExercise(workoutId, exerciseId, orderIndex, notes);
        }

        public void UpdateOrder(int newOrder)
        {
            if (newOrder <= 0 || newOrder > MaxOrderIndex)
            {
                throw new ArgumentException($"Order index must be 1-{MaxOrderIndex}", nameof(newOrder));
            }

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string? notes)
        {
            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsFirstExercise() => OrderIndex == 1;

        public bool IsLastExercise() => OrderIndex == MaxOrderIndex;

        public void MoveUp() => UpdateOrder(OrderIndex - 1);

        public void MoveDown() => UpdateOrder(OrderIndex + 1);
    }
}
