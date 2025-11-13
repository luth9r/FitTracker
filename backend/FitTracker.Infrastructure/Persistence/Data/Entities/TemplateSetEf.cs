namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents a planned set in a workout template.
    /// </summary>
    public class TemplateSetEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the workout template exercise.
        /// </summary>
        public Guid WorkoutTemplateExerciseId { get; set; }

        /// <summary>
        /// Set number in the exercise sequence.
        /// </summary>
        public int SetNumber { get; set; }

        /// <summary>
        /// Planned weight for this set.
        /// </summary>
        public decimal PlannedWeight { get; set; }

        /// <summary>
        /// Planned number of repetitions.
        /// </summary>
        public int PlannedReps { get; set; }

        /// <summary>
        /// Optional rest period in seconds.
        /// </summary>
        public int? RestSeconds { get; set; }

        /// <summary>
        /// Type of the set (normal, warmup, drop set, etc.).
        /// </summary>
        public int SetType { get; set; }

        /// <summary>
        /// Navigation to the workout template exercise.
        /// </summary>
        public WorkoutTemplateExerciseEf? WorkoutTemplateExercise { get; set; }
    }
}
