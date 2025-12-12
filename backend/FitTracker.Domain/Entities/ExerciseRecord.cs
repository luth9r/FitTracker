namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents personal records and statistics for a user's exercise performance.
    /// </summary>
    public class ExerciseRecord : BaseEntity
    {
        /// <summary>
        /// Gets the unique identifier of the user who owns this record.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the exercise associated with this record.
        /// </summary>
        public Guid ExerciseId { get; private set; }

        /// <summary>
        /// Gets the maximum weight lifted for this exercise (1RM or max weight).
        /// </summary>
        public double MaxWeightKg { get; private set; }

        /// <summary>
        /// Gets the maximum number of repetitions achieved in a single set.
        /// </summary>
        public int MaxReps { get; private set; }

        /// <summary>
        /// Gets the maximum volume (weight × reps) achieved in a single set.
        /// </summary>
        public double MaxVolume { get; private set; }

        /// <summary>
        /// Gets the maximum total volume achieved in a single workout session.
        /// </summary>
        public double MaxTotalVolume { get; private set; }

        /// <summary>
        /// Gets the date and time when the maximum weight record was set.
        /// </summary>
        public DateTime MaxWeightDate { get; private set; }

        /// <summary>
        /// Gets the date and time when the maximum reps record was set.
        /// </summary>
        public DateTime MaxRepsDate { get; private set; }

        /// <summary>
        /// Gets the date and time when the maximum volume record was set.
        /// </summary>
        public DateTime MaxVolumeDate { get; private set; }

        /// <summary>
        /// Gets the date and time when the maximum total volume record was set.
        /// </summary>
        public DateTime MaxTotalVolumeDate { get; private set; }

        /// <summary>
        /// Gets the total number of workout sessions where this exercise was performed.
        /// </summary>
        public int TotalWorkouts { get; private set; }

        /// <summary>
        /// Gets the total number of sets performed for this exercise.
        /// </summary>
        public int TotalSets { get; private set; }

        /// <summary>
        /// Gets the total number of repetitions performed for this exercise.
        /// </summary>
        public int TotalReps { get; private set; }

        /// <summary>
        /// Gets the total weight lifted across all sets for this exercise.
        /// </summary>
        public double TotalLifted { get; private set; }

        /// <summary>
        /// Gets the date and time when this exercise was last performed.
        /// </summary>
        public DateTime LastPerformed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseRecord"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="userId">The unique identifier of the user who owns this record.</param>
        /// <param name="exerciseId">The unique identifier of the exercise associated with this record.</param>
        /// <param name="maxWeight">The maximum weight lifted for this exercise.</param>
        /// <param name="maxReps">The maximum number of repetitions achieved in a single set.</param>
        /// <param name="maxVolume">The maximum volume achieved in a single set.</param>
        /// <param name="maxTotalVolume">The maximum total volume achieved in a single workout session.</param>
        /// <param name="maxWeightDate">The date and time when the maximum weight record was set.</param>
        /// <param name="maxRepsDate">The date and time when the maximum reps record was set.</param>
        /// <param name="maxVolumeDate">The date and time when the maximum volume record was set.</param>
        /// <param name="maxTotalVolumeDate">The date and time when the maximum total volume record was set.</param>
        /// <param name="totalWorkouts">The total number of workout sessions where this exercise was performed.</param>
        /// <param name="totalSets">The total number of sets performed for this exercise.</param>
        /// <param name="totalReps">The total number of repetitions performed for this exercise.</param>
        /// <param name="totalLifted">The total weight lifted across all sets for this exercise.</param>
        /// <param name="lastPerformed">The date and time when this exercise was last performed.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
        internal ExerciseRecord(
            Guid id,
            Guid userId,
            Guid exerciseId,
            double maxWeight,
            int maxReps,
            double maxVolume,
            double maxTotalVolume,
            DateTime maxWeightDate,
            DateTime maxRepsDate,
            DateTime maxVolumeDate,
            DateTime maxTotalVolumeDate,
            int totalWorkouts,
            int totalSets,
            int totalReps,
            double totalLifted,
            DateTime lastPerformed,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            MaxWeightKg = maxWeight;
            MaxReps = maxReps;
            MaxVolume = maxVolume;
            MaxTotalVolume = maxTotalVolume;
            MaxWeightDate = maxWeightDate;
            MaxRepsDate = maxRepsDate;
            MaxVolumeDate = maxVolumeDate;
            MaxTotalVolumeDate = maxTotalVolumeDate;
            TotalWorkouts = totalWorkouts;
            TotalSets = totalSets;
            TotalReps = totalReps;
            TotalLifted = totalLifted;
            LastPerformed = lastPerformed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseRecord"/> class.
        /// </summary>
        private ExerciseRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseRecord"/> class with specified user and exercise.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        private ExerciseRecord(
            Guid userId,
            Guid exerciseId)
            : base()
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty", nameof(userId));
            }

            if (exerciseId == Guid.Empty)
            {
                throw new ArgumentException("ExerciseId cannot be empty", nameof(exerciseId));
            }

            UserId = userId;
            ExerciseId = exerciseId;
            MaxWeightKg = 0d;
            MaxReps = 0;
            MaxVolume = 0;
            MaxTotalVolume = 0;
            TotalWorkouts = 0;
            TotalSets = 0;
            TotalReps = 0;
            TotalLifted = 0;

            var now = DateTime.UtcNow;
            MaxWeightDate = now;
            MaxRepsDate = now;
            MaxVolumeDate = now;
            MaxTotalVolumeDate = now;
            LastPerformed = now;
        }

        /// <summary>
        /// Creates a new <see cref="ExerciseRecord"/> for a user and exercise.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <returns>A new instance of <see cref="ExerciseRecord"/>.</returns>
        public static ExerciseRecord Create(Guid userId, Guid exerciseId)
        {
            return new ExerciseRecord(userId, exerciseId);
        }

        /// <summary>
        /// Updates the records with new workout data.
        /// </summary>
        /// <param name="maxSetWeightKg">The maximum weight lifted in a set during the workout.</param>
        /// <param name="maxSetReps">The maximum repetitions performed in a set during the workout.</param>
        /// <param name="maxSetVolume">The maximum volume achieved in a set during the workout.</param>
        /// <param name="workoutTotalVolume">The total volume achieved during the workout.</param>
        /// <param name="workoutSets">The number of sets performed during the workout.</param>
        /// <param name="workoutReps">The number of repetitions performed during the workout.</param>
        /// <param name="workoutLifted">The total weight lifted during the workout.</param>
        /// <returns><c>true</c> if a new personal record was set; otherwise, <c>false</c>.</returns>
        public bool UpdateRecords(
            double maxSetWeightKg,
            int maxSetReps,
            double maxSetVolume,
            double workoutTotalVolume,
            int workoutSets,
            int workoutReps,
            double workoutLifted)
        {
            // Guard clauses
            if (maxSetWeightKg == 0)
            {
                throw new ArgumentNullException(nameof(maxSetWeightKg));
            }

            if (maxSetReps < 0)
            {
                throw new ArgumentException("Max reps cannot be negative", nameof(maxSetReps));
            }

            if (maxSetVolume < 0)
            {
                throw new ArgumentException("Max volume cannot be negative", nameof(maxSetVolume));
            }

            if (workoutTotalVolume < 0)
            {
                throw new ArgumentException("Total volume cannot be negative", nameof(workoutTotalVolume));
            }

            if (workoutSets < 0)
            {
                throw new ArgumentException("Sets cannot be negative", nameof(workoutSets));
            }

            if (workoutReps < 0)
            {
                throw new ArgumentException("Reps cannot be negative", nameof(workoutReps));
            }

            if (workoutLifted < 0)
            {
                throw new ArgumentException("Lifted weight cannot be negative", nameof(workoutLifted));
            }

            bool newRecord = false;
            var now = DateTime.UtcNow;

            // Max Weight PR
            if (maxSetWeightKg > MaxWeightKg)
            {
                MaxWeightKg = maxSetWeightKg;
                MaxWeightDate = now;
                newRecord = true;
            }

            // Max Reps PR
            if (maxSetReps > MaxReps)
            {
                MaxReps = maxSetReps;
                MaxRepsDate = now;
                newRecord = true;
            }

            // Max Volume per Set PR
            if (maxSetVolume > MaxVolume)
            {
                MaxVolume = maxSetVolume;
                MaxVolumeDate = now;
                newRecord = true;
            }

            // Max Total Volume PR
            if (workoutTotalVolume > MaxTotalVolume)
            {
                MaxTotalVolume = workoutTotalVolume;
                MaxTotalVolumeDate = now;
                newRecord = true;
            }

            // Update cumulative stats
            TotalWorkouts++;
            TotalSets += workoutSets;
            TotalReps += workoutReps;
            TotalLifted += workoutLifted;
            LastPerformed = now;
            UpdatedAt = now;

            return newRecord;
        }

        /// <summary>
        /// Calculates the average weight lifted per set.
        /// </summary>
        /// <returns>The average weight per set.</returns>
        public double GetAverageWeightPerSet()
        {
            return TotalSets > 0 ? TotalLifted / TotalSets : 0;
        }

        /// <summary>
        /// Calculates the average repetitions performed per set.
        /// </summary>
        /// <returns>The average repetitions per set.</returns>
        public double GetAverageRepsPerSet()
        {
            return TotalSets > 0 ? (double)TotalReps / TotalSets : 0;
        }

        /// <summary>
        /// Determines whether the user has any records for this exercise.
        /// </summary>
        /// <returns><c>true</c> if there are records; otherwise, <c>false</c>.</returns>
        public bool HasRecords() => TotalWorkouts > 0;

        /// <summary>
        /// Calculates the time elapsed since the exercise was last performed.
        /// </summary>
        /// <returns>The time span since the last performance.</returns>
        public TimeSpan GetTimeSinceLastPerformed()
        {
            return DateTime.UtcNow - LastPerformed;
        }
    }
}
