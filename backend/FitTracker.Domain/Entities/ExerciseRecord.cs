using CSharpFunctionalExtensions;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents personal records and statistics for a user's exercise performance.
    /// </summary>
    public class ExerciseRecord : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier of the user who owns this record.
        /// </summary>
        public Guid UserId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the unique identifier of the exercise associated with this record.
        /// </summary>
        public Guid ExerciseId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum weight lifted for this exercise (1RM or max weight).
        /// </summary>
        public Weight MaxWeight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum number of repetitions achieved in a single set.
        /// </summary>
        public int MaxReps
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum volume (weight × reps) achieved in a single set.
        /// </summary>
        public decimal MaxVolume
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum total volume achieved in a single workout session.
        /// </summary>
        public decimal MaxTotalVolume
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when the maximum weight record was set.
        /// </summary>
        public DateTime MaxWeightDate
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when the maximum reps record was set.
        /// </summary>
        public DateTime MaxRepsDate
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when the maximum volume record was set.
        /// </summary>
        public DateTime MaxVolumeDate
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when the maximum total volume record was set.
        /// </summary>
        public DateTime MaxTotalVolumeDate
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total number of workout sessions where this exercise was performed.
        /// </summary>
        public int TotalWorkouts
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total number of sets performed for this exercise.
        /// </summary>
        public int TotalSets
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total number of repetitions performed for this exercise.
        /// </summary>
        public int TotalReps
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total weight lifted across all sets for this exercise.
        /// </summary>
        public decimal TotalLifted
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when this exercise was last performed.
        /// </summary>
        public DateTime LastPerformed
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private ExerciseRecord()
        {
        }

        /// <summary>
        /// Domain constructor used by Factory for creating new exercise records.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        public ExerciseRecord(Guid userId, Guid exerciseId) : base()
        {
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
            MaxWeightDate = DateTime.UtcNow;
            MaxRepsDate = DateTime.UtcNow;
            MaxVolumeDate = DateTime.UtcNow;
            MaxTotalVolumeDate = DateTime.UtcNow;
            LastPerformed = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor for restoring exercise record from persistence layer.
        /// Use <see cref="Create"/> for creating new exercise records.
        /// </summary>
        public ExerciseRecord(
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
            DateTime lastPerformed) : base()
        {
            UserId = userId;
            ExerciseId = exerciseId;
            MaxWeight = maxWeight ?? Weight.FromKilograms(0);
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

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new ExerciseRecordValidator();
        }

        private Result<ExerciseRecord, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<ExerciseRecord, ValidationResult>(result);

            return Result.Success<ExerciseRecord, ValidationResult>(this);
        }

        #endregion

        #region Factory

        /// <summary>
        /// Creates a new exercise record for the specified user and exercise.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <returns>A new <see cref="ExerciseRecord"/> instance with initialized values.</returns>
        public static ExerciseRecord Create(Guid userId, Guid exerciseId)
        {
            return new ExerciseRecord(userId, exerciseId);
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the exercise records based on workout performance data.
        /// </summary>
        /// <param name="maxSetWeight">The maximum weight lifted in a single set during the workout.</param>
        /// <param name="maxSetReps">The maximum number of reps achieved in a single set during the workout.</param>
        /// <param name="maxSetVolume">The maximum volume achieved in a single set during the workout.</param>
        /// <param name="workoutTotalVolume">The total volume for the entire workout session.</param>
        /// <param name="workoutSets">The total number of sets performed in the workout.</param>
        /// <param name="workoutReps">The total number of reps performed in the workout.</param>
        /// <param name="workoutLifted">The total weight lifted during the workout.</param>
        /// <returns><c>true</c> if any personal record was improved; otherwise, <c>false</c>.</returns>
        public bool UpdateRecords(
            Weight maxSetWeight,
            int maxSetReps,
            decimal maxSetVolume,
            decimal workoutTotalVolume,
            int workoutSets,
            int workoutReps,
            decimal workoutLifted)
        {
            bool newRecord = false;

            // Max Weight PR
            if (maxSetWeight.ToKilograms() > MaxWeight.ToKilograms())
            {
                MaxWeight = maxSetWeight;
                MaxWeightDate = DateTime.UtcNow;
                newRecord = true;
            }

            // Max Reps PR
            if (maxSetReps > MaxReps)
            {
                MaxReps = maxSetReps;
                MaxRepsDate = DateTime.UtcNow;
                newRecord = true;
            }

            // Max Volume per Set PR
            if (maxSetVolume > MaxVolume)
            {
                MaxVolume = maxSetVolume;
                MaxVolumeDate = DateTime.UtcNow;
                newRecord = true;
            }

            // Max Total Volume PR
            if (workoutTotalVolume > MaxTotalVolume)
            {
                MaxTotalVolume = workoutTotalVolume;
                MaxTotalVolumeDate = DateTime.UtcNow;
                newRecord = true;
            }

            // Update stats
            TotalWorkouts++;
            TotalSets += workoutSets;
            TotalReps += workoutReps;
            TotalLifted += workoutLifted;
            LastPerformed = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            return newRecord;
        }

        /// <summary>
        /// Calculates the average weight lifted per set.
        /// </summary>
        /// <returns>The average weight per set, or 0 if no sets have been performed.</returns>
        public decimal GetAverageWeightPerSet()
        {
            return TotalSets > 0 ? TotalLifted / TotalSets : 0;
        }

        /// <summary>
        /// Calculates the average number of repetitions per set.
        /// </summary>
        /// <returns>The average reps per set, or 0 if no sets have been performed.</returns>
        public decimal GetAverageRepsPerSet()
        {
            return TotalSets > 0 ? (decimal)TotalReps / TotalSets : 0;
        }

        #endregion
    }
}
