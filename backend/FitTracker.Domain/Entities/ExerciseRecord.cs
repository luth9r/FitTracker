using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Personal records for exercises
    /// </summary>
    public class ExerciseRecord : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid ExerciseId { get; private set; }

        // Records
        public Weight MaxWeight { get; private set; }           // 1RM or max weight
        public int MaxReps { get; private set; }                // Max reps
        public decimal MaxVolume { get; private set; }          // Max volume for set
        public decimal MaxTotalVolume { get; private set; }     // Max volume for workout

        // When records were set
        public DateTime MaxWeightDate { get; private set; }
        public DateTime MaxRepsDate { get; private set; }
        public DateTime MaxVolumeDate { get; private set; }
        public DateTime MaxTotalVolumeDate { get; private set; }

        // Stats
        public int TotalWorkouts { get; private set; }          // How many times this exercise done
        public int TotalSets { get; private set; }
        public int TotalReps { get; private set; }
        public decimal TotalLifted { get; private set; }        // Total weight lifted
        public DateTime LastPerformed { get; private set; }

        // Navigation
        public User? User { get; private set; }
        public Exercise? Exercise { get; private set; }

        private ExerciseRecord()
        {
            MaxWeight = Weight.FromKilograms(0);
        }

        private ExerciseRecord(Guid userId, Guid exerciseId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (exerciseId == Guid.Empty)
                throw new ArgumentException("Exercise ID cannot be empty");

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

            EnsureValid();
        }

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
            DateTime lastPerformed)
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

            EnsureValid();
        }

        protected override IValidator GetValidator()
        {
            return new ExerciseRecordValidator();
        }

        public static ExerciseRecord Create(Guid userId, Guid exerciseId)
        {
            return new ExerciseRecord(userId, exerciseId);
        }

        /// <summary>
        /// Update records based on workout data
        /// </summary>
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

        public decimal GetAverageWeightPerSet()
        {
            return TotalSets > 0 ? TotalLifted / TotalSets : 0;
        }

        public decimal GetAverageRepsPerSet()
        {
            return TotalSets > 0 ? (decimal)TotalReps / TotalSets : 0;
        }
    }
}
