namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout template.
    /// </summary>
    public class WorkoutTemplateExercise : BaseEntity
    {
        public const int NotesMaxLength = 500;
        public const int MinOrderIndex = 1;
        public const int MaxOrderIndex = 1000;

        public Guid WorkoutTemplateId { get; private set; }

        public Guid ExerciseId { get; private set; }

        public int OrderIndex { get; private set; }

        public string? Notes { get; private set; }

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

        private WorkoutTemplateExercise()
        {
        }

        private WorkoutTemplateExercise(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
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

        public static WorkoutTemplateExercise Create(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            return new WorkoutTemplateExercise(workoutTemplateId, exerciseId, orderIndex, notes);
        }

        public void UpdateOrder(int newOrder)
        {
            if (newOrder < MinOrderIndex || newOrder > MaxOrderIndex)
            {
                throw new ArgumentException($"Order index must be {MinOrderIndex}-{MaxOrderIndex}", nameof(newOrder));
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

        public bool IsFirstExercise() => OrderIndex == MinOrderIndex;

        public bool IsLastExercise() => OrderIndex == MaxOrderIndex;

        public void MoveUp() => UpdateOrder(OrderIndex - 1);

        public void MoveDown() => UpdateOrder(OrderIndex + 1);
    }
}
