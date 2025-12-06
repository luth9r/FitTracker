using FitTracker.Domain.ValueObjects;

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
        public Weight MaxWeight { get; private set; }

        /// <summary>
        /// Gets the maximum number of repetitions achieved in a single set.
        /// </summary>
        public int MaxReps { get; private set; }

        /// <summary>
        /// Gets the maximum volume (weight × reps) achieved in a single set.
        /// </summary>
        public decimal MaxVolume { get; private set; }

        /// <summary>
        /// Gets the maximum total volume achieved in a single workout session.
        /// </summary>
        public decimal MaxTotalVolume { get; private set; }

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
        public decimal TotalLifted { get; private set; }

        /// <summary>
        /// Gets the date and time when this exercise was last performed.
        /// </summary>
        public DateTime LastPerformed { get; private set; }

        internal ExerciseRecord(
            Guid id,
            Guid userId,
            Guid exerciseId,
            Weight maxWeight,
            int maxReps,
            decimal maxVolume,
            decimal maxTotalVolume,
            DateTime maxWeightDate,
            DateTime maxRepsDate,
            DateTime maxVolumeDate,
            DateTime maxTotalVolumeDate,
            int totalWorkouts,
            int totalSets,
            int totalReps,
            decimal totalLifted,
            DateTime lastPerformed,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            MaxWeight = maxWeight;
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

        private ExerciseRecord()
        {
        }

        private ExerciseRecord(Guid userId, Guid exerciseId)
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
            MaxWeight = Weight.FromKilograms(0);
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

        public static ExerciseRecord Create(Guid userId, Guid exerciseId)
        {
            return new ExerciseRecord(userId, exerciseId);
        }

        public bool UpdateRecords(
            Weight maxSetWeight,
            int maxSetReps,
            decimal maxSetVolume,
            decimal workoutTotalVolume,
            int workoutSets,
            int workoutReps,
            decimal workoutLifted)
        {
            // Guard clauses
            if (maxSetWeight == null)
            {
                throw new ArgumentNullException(nameof(maxSetWeight));
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
            if (maxSetWeight.ToKilograms() > MaxWeight.ToKilograms())
            {
                MaxWeight = maxSetWeight;
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

        public decimal GetAverageWeightPerSet()
        {
            return TotalSets > 0 ? TotalLifted / TotalSets : 0;
        }

        public decimal GetAverageRepsPerSet()
        {
            return TotalSets > 0 ? (decimal)TotalReps / TotalSets : 0;
        }

        public bool HasRecords() => TotalWorkouts > 0;

        public TimeSpan GetTimeSinceLastPerformed()
        {
            return DateTime.UtcNow - LastPerformed;
        }
    }
}
