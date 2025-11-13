namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents user's personal records and statistics for an exercise.
    /// </summary>
    public class ExerciseRecordEf : BaseEntityEf
    {
        /// <summary>
        /// ID of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ID of the exercise.
        /// </summary>
        public Guid ExerciseId { get; set; }

        /// <summary>
        /// Maximum weight lifted in kilograms.
        /// </summary>
        public decimal MaxWeightKilograms { get; set; }

        /// <summary>
        /// Maximum repetitions achieved.
        /// </summary>
        public int MaxReps { get; set; }

        /// <summary>
        /// Maximum volume in a single set (weight × reps).
        /// </summary>
        public decimal MaxVolume { get; set; }

        /// <summary>
        /// Maximum total volume in a single workout.
        /// </summary>
        public decimal MaxTotalVolume { get; set; }

        /// <summary>
        /// Date when max weight was achieved.
        /// </summary>
        public DateTime MaxWeightDate { get; set; }

        /// <summary>
        /// Date when max reps were achieved.
        /// </summary>
        public DateTime MaxRepsDate { get; set; }

        /// <summary>
        /// Date when max volume was achieved.
        /// </summary>
        public DateTime MaxVolumeDate { get; set; }

        /// <summary>
        /// Date when max total volume was achieved.
        /// </summary>
        public DateTime MaxTotalVolumeDate { get; set; }

        /// <summary>
        /// Total number of workouts including this exercise.
        /// </summary>
        public int TotalWorkouts { get; set; }

        /// <summary>
        /// Total number of sets performed.
        /// </summary>
        public int TotalSets { get; set; }

        /// <summary>
        /// Total number of repetitions performed.
        /// </summary>
        public int TotalReps { get; set; }

        /// <summary>
        /// Total weight lifted across all workouts.
        /// </summary>
        public decimal TotalLifted { get; set; }

        /// <summary>
        /// Date when this exercise was last performed.
        /// </summary>
        public DateTime LastPerformed { get; set; }

        /// <summary>
        /// Navigation to the user.
        /// </summary>
        public UserEf? User { get; set; }

        /// <summary>
        /// Navigation to the exercise.
        /// </summary>
        public ExerciseEf? Exercise { get; set; }
    }
}
