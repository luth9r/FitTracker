using CSharpFunctionalExtensions;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a single set within a workout exercise.
    /// </summary>
    public class Set : BaseEntity
    {
        #region Constants

        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600; // 1 hour
        public const decimal MaxWeightKg = 3000m;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the workout exercise this set belongs to.
        /// </summary>
        public Guid WorkoutExerciseId { get; private set; }

        /// <summary>
        /// Gets the sequential number of this set within the workout exercise.
        /// </summary>
        public int SetNumber { get; private set; }

        /// <summary>
        /// Gets the weight used for this set.
        /// </summary>
        public Weight Weight { get; private set; }

        /// <summary>
        /// Gets the number of repetitions performed in this set.
        /// </summary>
        public int Reps { get; private set; }

        /// <summary>
        /// Gets the rest period in seconds before the next set, or null if not specified.
        /// </summary>
        public int? RestSeconds { get; private set; }

        /// <summary>
        /// Gets the type of this set (Normal, Dropset, Superset, etc.).
        /// </summary>
        public SetType SetType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this set has been completed.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets the date and time when this set was completed, or null if not yet completed.
        /// </summary>
        public DateTime? CompletedAt { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private Set()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new sets.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private Set(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds,
            SetType setType = SetType.Normal) : base()
        {
            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
            Reps = reps;
            RestSeconds = restSeconds;
            SetType = setType;
            IsCompleted = false;

        }

        /// <summary>
        /// Constructor for restoring set from persistence layer.
        /// Use <see cref="Create"/> for creating new sets.
        /// </summary>
        public Set(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds,
            SetType setType,
            bool isCompleted,
            DateTime? completedAt) : base()
        {
            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
            RestSeconds = restSeconds;
            SetType = setType;
            IsCompleted = isCompleted;
            CompletedAt = completedAt;

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new SetValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<Set>(this));
        }

        private Result<Set, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<Set, ValidationResult>(result);

            return Result.Success<Set, ValidationResult>(this);
        }

        #endregion

        #region Factory

        /// <summary>
        /// Creates a new set with validation.
        /// </summary>
        public static Result<Set, ValidationResult> Create(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds,
            SetType setType = SetType.Normal)
        {
            var set = new Set(workoutExerciseId, setNumber, weight, reps, restSeconds, setType);
            return set.ValidateWithResult();
        }

        #endregion

        #region Domain Methods

        public Result<Set, ValidationResult> UpdateSetNumber(int newSetNumber)
        {
            if (newSetNumber <= 0)
                throw new ArgumentException("Set number must be greater than 0");

            SetNumber = newSetNumber;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();

        }

        public Result<Set, ValidationResult> UpdateWeight(Weight weight)
        {
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<Set, ValidationResult> IncreaseWeightKg(decimal amountKg)
        {
            if (amountKg < 0)
                throw new ArgumentException("Amount cannot be negative");

            var newWeightKg = Weight.ToKilograms() + amountKg;
            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<Set, ValidationResult> DecreaseWeightKg(decimal amountKg)
        {
            if (amountKg < 0)
                throw new ArgumentException("Amount cannot be negative");

            var newWeightKg = Weight.ToKilograms() - amountKg;
            if (newWeightKg < 0)
                throw new InvalidOperationException("Weight cannot be negative");

            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<Set, ValidationResult> UpdateReps(int reps)
        {
            if (reps <= 0)
                throw new ArgumentException("Reps must be greater than 0");

            if (reps > MaxReps)
                throw new ArgumentException($"Reps cannot exceed {MaxReps}");

            Reps = reps;
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<Set, ValidationResult> UpdateRest(int? seconds)
        {
            if (seconds.HasValue)
            {
                if (seconds.Value < 0)
                    throw new ArgumentException("Rest seconds cannot be negative");

                if (seconds.Value > MaxRestSeconds)
                    throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds");
            }

            RestSeconds = seconds;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public void ChangeSetType(SetType setType)
        {
            SetType = setType;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Uncomplete()
        {
            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public decimal CalculateVolume()
        {
            return Weight.ToKilograms() * Reps;
        }

        public decimal CalculateVolumeLbs()
        {
            return Weight.ToPounds() * Reps;
        }

        public bool IsPR(IEnumerable<Set> previousSets)
        {
            if (previousSets == null || !previousSets.Any())
                return true;

            var maxPreviousWeight = previousSets.Max(s => s.Weight.ToKilograms());
            return Weight.ToKilograms() > maxPreviousWeight;
        }

        #endregion

        #region Builder

        public static SetBuilder CreateBuilder() => new SetBuilder();

        public class SetBuilder
        {
            private Guid _workoutExerciseId;
            private int _setNumber;
            private Weight _weight = Weight.FromKilograms(0);
            private int _reps;
            private int? _restSeconds;
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
                _weight = weight;
                return this;
            }

            public SetBuilder WithWeightKg(decimal kg)
            {
                _weight = Weight.FromKilograms(kg);
                return this;
            }

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
                _restSeconds = seconds;
                return this;
            }

            public SetBuilder WithSetType(SetType setType)
            {
                _setType = setType;
                return this;
            }

            public Result<Set, ValidationResult> Build()
            {
                var set = new Set(_workoutExerciseId, _setNumber, _weight, _reps, _restSeconds, _setType);

                return set.ValidateWithResult();
            }
        }

        #endregion
    }
}
