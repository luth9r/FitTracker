namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents a physical exercise.
    /// </summary>
    public class ExerciseEf : BaseEntityEf
    {
        /// <summary>
        /// Gets or sets exercise name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets optional exercise description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets optional URL to exercise demonstration image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets optional URL to exercise demonstration video.
        /// </summary>
        public string? VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets primary muscle group targeted by this exercise.
        /// </summary>
        public int MuscleGroup { get; set; }

        /// <summary>
        /// Gets or sets equipment required for this exercise.
        /// </summary>
        public int Equipment { get; set; }

        /// <summary>
        /// Gets or sets iD of the user who created this custom exercise (null for default exercises).
        /// </summary>
        public Guid? CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets navigation to the user (for custom exercises).
        /// </summary>
        public UserEf? CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets collection of workout exercises using this exercise.
        /// </summary>
        public ICollection<WorkoutExerciseEf> WorkoutExercises { get; set; }
    }
}
