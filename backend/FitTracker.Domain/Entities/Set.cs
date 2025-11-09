// FitTracker.Domain/Entities/Set.cs
using System;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a single set within a workout exercise
    /// </summary>
    public class Set : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600; // 1 hour
        public const decimal MaxWeightKg = 3000m;

        // ============================================
        // Properties
        // ============================================
        public Guid WorkoutExerciseId { get; private set; }
        public int SetNumber { get; private set; }
        public Weight Weight { get; private set; }
        public int Reps { get; private set; }
        public int? RestSeconds { get; private set; }
        public SetType SetType { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // Navigation Properties
        public WorkoutExercise? WorkoutExercise { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// EF Core constructor
        /// </summary>
        private Set()
        {
            Weight = Weight.FromKilograms(0);
            SetType = SetType.Normal;
        }

        /// <summary>
        /// Domain constructor
        /// </summary>
        private Set(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            SetType setType = SetType.Normal)
        {
            if (workoutExerciseId == Guid.Empty)
                throw new ArgumentException("Workout exercise ID cannot be empty");

            if (setNumber <= 0)
                throw new ArgumentException("Set number must be greater than 0");

            if (reps <= 0)
                throw new ArgumentException("Reps must be greater than 0");

            if (reps > MaxReps)
                throw new ArgumentException($"Reps cannot exceed {MaxReps}");

            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
            Reps = reps;
            SetType = setType;
            IsCompleted = false;

            EnsureValid();
        }

        public Set(Guid workoutExerciseId, int setNumber, Weight weight, int reps, int? restSeconds, SetType setType, bool isCompleted, DateTime? completedAt)
        {
            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
            RestSeconds = restSeconds;
            SetType = setType;
            IsCompleted = isCompleted;
            CompletedAt = completedAt;
        }



        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new SetValidator();
        }

        /// <summary>
        /// Creates a new builder for Set
        /// </summary>
        public static SetBuilder CreateBuilder() => new SetBuilder();

        public class SetBuilder
        {
            private Guid _workoutExerciseId;
            private int _setNumber;
            private Weight _weight = Weight.FromKilograms(0);
            private int _reps;
            private int? _restSeconds;
            private bool _isWarmup;
            private SetType _setType = SetType.Normal;


            public SetBuilder WithWorkoutExercise(Guid id)
            {
                _workoutExerciseId = id;
                return this;
            }

            public SetBuilder WithSetNumber(int number)
            {
                _setNumber = number;
                return this;
            }

            public SetBuilder WithWeight(Weight weight)
            {
                _weight = weight ?? throw new ArgumentNullException(nameof(weight));
                return this;
            }

            /// <summary>
            /// Set weight in kilograms
            /// </summary>
            public SetBuilder WithWeightKg(decimal kg)
            {
                _weight = Weight.FromKilograms(kg);
                return this;
            }

            /// <summary>
            /// Set weight in pounds (will be converted to kg)
            /// </summary>
            public SetBuilder WithWeightLbs(decimal lbs)
            {
                _weight = Weight.FromPounds(lbs);
                return this;
            }

            public SetBuilder WithReps(int reps)
            {
                _reps = reps;
                return this;
            }

            public SetBuilder WithRest(int seconds)
            {
                if (seconds < 0)
                    throw new ArgumentException("Rest seconds cannot be negative");

                _restSeconds = seconds;
                return this;
            }

            public SetBuilder AsWarmup(bool isWarmup = true)
            {
                _isWarmup = isWarmup;
                return this;
            }

            /// <summary>
            /// Set the type of set (Normal, Dropset, Superset...)
            /// </summary>
            public SetBuilder WithSetType(SetType setType)
            {
                _setType = setType;
                return this;
            }

            public Set Build()
            {
                var set = new Set(_workoutExerciseId, _setNumber, _weight, _reps)
                {
                    RestSeconds = _restSeconds,
                };
                return set;
            }
        }

        // ============================================
        // Domain Methods
        // ============================================

        /// <summary>
        /// Update set number (used during reordering)
        /// </summary>
        public void UpdateSetNumber(int newSetNumber)
        {
            if (newSetNumber <= 0)
                throw new ArgumentException("Set number must be greater than 0");

            SetNumber = newSetNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Update weight value
        /// </summary>
        public void UpdateWeight(Weight weight)
        {
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
            UpdatedAt = DateTime.UtcNow;
            EnsureValid();
        }

        /// <summary>
        /// Increase weight by specified amount in kilograms
        /// </summary>
        /// <param name="amountKg">Amount to increase in kilograms</param>
        public void IncreaseWeightKg(decimal amountKg)
        {
            if (amountKg < 0)
                throw new ArgumentException("Amount cannot be negative");

            var newWeightKg = Weight.ToKilograms() + amountKg;
            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
            EnsureValid();
        }

        /// <summary>
        /// Decrease weight by specified amount in kilograms
        /// </summary>
        /// <param name="amountKg">Amount to decrease in kilograms</param>
        public void DecreaseWeightKg(decimal amountKg)
        {
            if (amountKg < 0)
                throw new ArgumentException("Amount cannot be negative");

            var newWeightKg = Weight.ToKilograms() - amountKg;

            if (newWeightKg < 0)
                throw new InvalidOperationException("Weight cannot be negative");

            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
            EnsureValid();
        }

        /// <summary>
        /// Update number of reps
        /// </summary>
        public void UpdateReps(int reps)
        {
            if (reps <= 0)
                throw new ArgumentException("Reps must be greater than 0");

            if (reps > MaxReps)
                throw new ArgumentException($"Reps cannot exceed {MaxReps}");

            Reps = reps;
            UpdatedAt = DateTime.UtcNow;
            EnsureValid();
        }

        /// <summary>
        /// Update rest period in seconds
        /// </summary>
        public void UpdateRest(int? seconds)
        {
            if (seconds.HasValue && seconds.Value < 0)
                throw new ArgumentException("Rest seconds cannot be negative");

            if (seconds.HasValue && seconds.Value > MaxRestSeconds)
                throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds");

            RestSeconds = seconds;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Change set type
        /// </summary>
        public void ChangeSetType(SetType setType)
        {
            SetType = setType;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark set as completed
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark set as incomplete
        /// </summary>
        public void Uncomplete()
        {
            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculate volume (weight × reps) in kilograms
        /// </summary>
        /// <returns>Volume in kilograms</returns>
        public decimal CalculateVolume()
        {
            return Weight.ToKilograms() * Reps;
        }

        /// <summary>
        /// Calculate volume in pounds
        /// </summary>
        /// <returns>Volume in pounds</returns>
        public decimal CalculateVolumeLbs()
        {
            return Weight.ToPounds() * Reps;
        }

        /// <summary>
        /// Check if this is a personal record for weight
        /// </summary>
        public bool IsPR(IEnumerable<Set> previousSets)
        {
            if (previousSets == null || !previousSets.Any())
                return true;

            var maxPreviousWeight = previousSets.Max(s => s.Weight.ToKilograms());
            return Weight.ToKilograms() > maxPreviousWeight;
        }
    }
}
