namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents an exercise within a specific workout session.
    /// </summary>
    public class WorkoutExerciseEf : BaseEntityEf
    {
        /// <summary>
        /// Gets or sets iD of the workout session.
        /// </summary>
        public Guid WorkoutId { get; set; }

        /// <summary>
        /// Gets or sets iD of the exercise.
        /// </summary>
        public Guid ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets order index of the exercise in the workout.
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// Gets or sets optional notes for this exercise.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets navigation to the workout session.
        /// </summary>
        public WorkoutEf? Workout { get; set; }

        /// <summary>
        /// Gets or sets navigation to the exercise.
        /// </summary>
        public ExerciseEf? Exercise { get; set; }

        /// <summary>
        /// Gets or sets collection of sets performed for this exercise.
        /// </summary>
        public ICollection<SetEf> Sets { get; set; } = new HashSet<SetEf>();
    }
}
