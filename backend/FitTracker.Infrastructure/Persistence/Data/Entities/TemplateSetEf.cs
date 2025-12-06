namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents a planned set in a workout template.
    /// </summary>
    public class TemplateSetEf : BaseEntityEf
    {
        /// <summary>
        /// Gets or sets iD of the workout template exercise.
        /// </summary>
        public Guid WorkoutTemplateExerciseId { get; set; }

        /// <summary>
        /// Gets or sets set number in the exercise sequence.
        /// </summary>
        public int SetNumber { get; set; }

        /// <summary>
        /// Gets or sets planned weight for this set.
        /// </summary>
        public decimal PlannedWeight { get; set; }

        /// <summary>
        /// Gets or sets planned number of repetitions.
        /// </summary>
        public int PlannedReps { get; set; }

        /// <summary>
        /// Gets or sets optional rest period in seconds.
        /// </summary>
        public int? RestSeconds { get; set; }

        /// <summary>
        /// Gets or sets type of the set (normal, warmup, drop set, etc.).
        /// </summary>
        public int SetType { get; set; }

        /// <summary>
        /// Gets or sets navigation to the workout template exercise.
        /// </summary>
        public WorkoutTemplateExerciseEf? WorkoutTemplateExercise { get; set; } = null;
    }
}
