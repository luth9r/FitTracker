namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents an exercise within a specific workout session.
	/// </summary>
	public class WorkoutExerciseEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the workout session.
        /// </summary>
        public Guid WorkoutId
        {
            get; set;
        }

        /// <summary>
        /// ID of the exercise.
        /// </summary>
        public Guid ExerciseId
        {
            get; set;
        }

        /// <summary>
        /// Order index of the exercise in the workout.
        /// </summary>
        public int OrderIndex
        {
            get; set;
        }

        /// <summary>
        /// Optional notes for this exercise.
        /// </summary>
        public string? Notes
        {
            get; set;
        }

        /// <summary>
        /// Navigation to the workout session.
        /// </summary>
        public WorkoutEf? Workout
        {
            get; set;
        }

        /// <summary>
        /// Navigation to the exercise.
        /// </summary>
        public ExerciseEf? Exercise
        {
            get; set;
        }

        /// <summary>
        /// Collection of sets performed for this exercise.
        /// </summary>
        public ICollection<SetEf> Sets { get; set; } = new HashSet<SetEf>();
    }
}
