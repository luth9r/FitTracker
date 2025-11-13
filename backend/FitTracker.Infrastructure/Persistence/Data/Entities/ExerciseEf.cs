namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
	/// Represents a physical exercise.
	/// </summary>
	public class ExerciseEf : BaseEntityEf
    {
        /// <summary>
        /// Exercise name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Optional exercise description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Optional URL to exercise demonstration image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Optional URL to exercise demonstration video.
        /// </summary>
        public string? VideoUrl { get; set; }

        /// <summary>
        /// Primary muscle group targeted by this exercise.
        /// </summary>
        public int MuscleGroup { get; set; }

        /// <summary>
        /// Equipment required for this exercise.
        /// </summary>
        public int Equipment { get; set; }

        /// <summary>
        /// Indicates if this is a user-created custom exercise.
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// ID of the user who created this custom exercise (null for default exercises).
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Navigation to the user (for custom exercises).
        /// </summary>
        public UserEf? User { get; set; }

        /// <summary>
        /// Collection of workout exercises using this exercise.
        /// </summary>
        public ICollection<WorkoutExerciseEf> WorkoutExercises { get; set; }
    }
}
