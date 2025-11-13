namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents a completed set in a workout.
    /// </summary>
    public class SetEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the workout exercise.
        /// </summary>
        public Guid WorkoutExerciseId { get; set; }

        /// <summary>
        /// Set number in the exercise sequence.
        /// </summary>
        public int SetNumber { get; set; }

        /// <summary>
        /// Weight used in kilograms.
        /// </summary>
        public decimal WeightKg { get; set; }

        /// <summary>
        /// Number of repetitions completed.
        /// </summary>
        public int Reps { get; set; }

        /// <summary>
        /// Optional rest period in seconds.
        /// </summary>
        public int? RestSeconds { get; set; }

        /// <summary>
        /// Type of the set (normal, warmup, drop set, etc.).
        /// </summary>
        public int SetType { get; set; }

        /// <summary>
        /// Indicates if the set was completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Timestamp when the set was completed.
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Navigation to the workout exercise.
        /// </summary>
        public WorkoutExerciseEf? WorkoutExercise { get; set; }
    }
}
