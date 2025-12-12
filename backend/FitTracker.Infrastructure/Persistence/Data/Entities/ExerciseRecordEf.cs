namespace FitTracker.Infrastructure.Persistence.Data.Entities
{
    /// <summary>
    /// Represents user's personal records and statistics for an exercise.
    /// </summary>
    public class ExerciseRecordEf : BaseEntityEf
    {
        /// <summary>
        /// Gets or sets iD of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets iD of the exercise.
        /// </summary>
        public Guid ExerciseId { get; set; }

        /// <summary>
        /// Gets or sets maximum weight lifted in kilograms.
        /// </summary>
        public double MaxWeightKg { get; set; }

        /// <summary>
        /// Gets or sets maximum repetitions achieved.
        /// </summary>
        public int MaxReps { get; set; }

        /// <summary>
        /// Gets or sets maximum volume in a single set (weight × reps).
        /// </summary>
        public double MaxVolumeKg { get; set; }

        /// <summary>
        /// Gets or sets maximum total volume in a single workout.
        /// </summary>
        public double MaxTotalVolumeKg { get; set; }

        /// <summary>
        /// Gets or sets date when max weight was achieved.
        /// </summary>
        public DateTime MaxWeightDate { get; set; }

        /// <summary>
        /// Gets or sets date when max reps were achieved.
        /// </summary>
        public DateTime MaxRepsDate { get; set; }

        /// <summary>
        /// Gets or sets date when max volume was achieved.
        /// </summary>
        public DateTime MaxVolumeDate { get; set; }

        /// <summary>
        /// Gets or sets date when max total volume was achieved.
        /// </summary>
        public DateTime MaxTotalVolumeDate { get; set; }

        /// <summary>
        /// Gets or sets total number of workouts including this exercise.
        /// </summary>
        public int TotalWorkouts { get; set; }

        /// <summary>
        /// Gets or sets total number of sets performed.
        /// </summary>
        public int TotalSets { get; set; }

        /// <summary>
        /// Gets or sets total number of repetitions performed.
        /// </summary>
        public int TotalReps { get; set; }

        /// <summary>
        /// Gets or sets total weight lifted across all workouts.
        /// </summary>
        public double TotalLiftedKg { get; set; }

        /// <summary>
        /// Gets or sets date when this exercise was last performed.
        /// </summary>
        public DateTime LastPerformed { get; set; }

        /// <summary>
        /// Gets or sets navigation to the user.
        /// </summary>
        public UserEf? User { get; set; } = null!;

        /// <summary>
        /// Gets or sets navigation to the exercise.
        /// </summary>
        public ExerciseEf? Exercise { get; set; } = null!;
    }
}
