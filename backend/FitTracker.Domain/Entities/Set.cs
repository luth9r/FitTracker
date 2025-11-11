// FitTracker.Domain/Entities/Set.cs
using System;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;

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
        public Guid WorkoutExerciseId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sequential number of this set within the workout exercise.
        /// </summary>
        public int SetNumber
        {
            get; private set;
        }

        /// <summary>
        /// Gets the weight used for this set.
        /// </summary>
        public Weight Weight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the number of repetitions performed in this set.
        /// </summary>
        public int Reps
        {
            get; private set;
        }

        /// <summary>
        /// Gets the rest period in seconds before the next set, or null if not specified.
        /// </summary>
        public int? RestSeconds
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of this set (Normal, Dropset, Superset, etc.).
        /// </summary>
        public SetType SetType
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether this set has been completed.
        /// </summary>
        public bool IsCompleted
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when this set was completed, or null if not yet completed.
        /// </summary>
        public DateTime? CompletedAt
        {
            get; private set;
        }

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

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring set from persistence layer.
        /// Use <see cref="SetBuilder"/> for creating new sets.
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

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the set number during reordering operations.
        /// </summary>
        /// <param name="newSetNumber">The new sequential number for this set.</param>
        /// <exception cref="ArgumentException">Thrown when set number is less than or equal to 0.</exception>
        public void UpdateSetNumber(int newSetNumber)
        {
            if (newSetNumber <= 0)
                throw new ArgumentException("Set number must be greater than 0");

            SetNumber = newSetNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the weight value for this set.
        /// </summary>
        /// <param name="weight">The new weight value.</param>
        /// <exception cref="ArgumentNullException">Thrown when weight is null.</exception>
        public void UpdateWeight(Weight weight)
        {
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
            UpdatedAt = DateTime.UtcNow;
            EnsureValid();
        }

        /// <summary>
        /// Increases the weight by the specified amount in kilograms.
        /// </summary>
        /// <param name="amountKg">The amount to increase in kilograms.</param>
        /// <exception cref="ArgumentException">Thrown when amount is negative.</exception>
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
        /// Decreases the weight by the specified amount in kilograms.
        /// </summary>
        /// <param name="amountKg">The amount to decrease in kilograms.</param>
        /// <exception cref="ArgumentException">Thrown when amount is negative.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the resulting weight would be negative.</exception>
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
        /// Updates the number of repetitions for this set.
        /// </summary>
        /// <param name="reps">The new number of repetitions.</param>
        /// <exception cref="ArgumentException">Thrown when reps is less than or equal to 0, or exceeds <see cref="MaxReps"/>.</exception>
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
        /// Updates the rest period in seconds before the next set.
        /// </summary>
        /// <param name="seconds">The rest period in seconds, or null to clear.</param>
        /// <exception cref="ArgumentException">Thrown when seconds is negative or exceeds <see cref="MaxRestSeconds"/>.</exception>
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
        /// Changes the type of this set.
        /// </summary>
        /// <param name="setType">The new set type.</param>
        public void ChangeSetType(SetType setType)
        {
            SetType = setType;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks this set as completed.
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks this set as incomplete.
        /// </summary>
        public void Uncomplete()
        {
            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculates the volume (weight × reps) in kilograms.
        /// </summary>
        /// <returns>The volume in kilograms.</returns>
        public decimal CalculateVolume()
        {
            return Weight.ToKilograms() * Reps;
        }

        /// <summary>
        /// Calculates the volume (weight × reps) in pounds.
        /// </summary>
        /// <returns>The volume in pounds.</returns>
        public decimal CalculateVolumeLbs()
        {
            return Weight.ToPounds() * Reps;
        }

        /// <summary>
        /// Determines whether this set represents a personal record for weight.
        /// </summary>
        /// <param name="previousSets">Collection of previous sets to compare against.</param>
        /// <returns><c>true</c> if this set's weight exceeds all previous sets or no previous sets exist; otherwise, <c>false</c>.</returns>
        public bool IsPR(IEnumerable<Set> previousSets)
        {
            if (previousSets == null || !previousSets.Any())
                return true;

            var maxPreviousWeight = previousSets.Max(s => s.Weight.ToKilograms());
            return Weight.ToKilograms() > maxPreviousWeight;
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="SetBuilder"/> instance.
        /// </summary>
        public static SetBuilder CreateBuilder()
        {
            return new SetBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="Set"/> instances.
        /// </summary>
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
                _weight = weight ?? throw new ArgumentNullException(nameof(weight));
                return this;
            }

            /// <summary>
            /// Sets the weight in kilograms.
            /// </summary>
            /// <param name="kg">The weight in kilograms.</param>
            public SetBuilder WithWeightKg(decimal kg)
            {
                _weight = Weight.FromKilograms(kg);
                return this;
            }

            /// <summary>
            /// Sets the weight in pounds (will be converted to kilograms).
            /// </summary>
            /// <param name="lbs">The weight in pounds.</param>
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

            /// <summary>
            /// Sets the rest period in seconds.
            /// </summary>
            /// <param name="seconds">The rest period in seconds.</param>
            /// <exception cref="ArgumentException">Thrown when seconds is negative.</exception>
            public SetBuilder WithRest(int seconds)
            {
                if (seconds < 0)
                    throw new ArgumentException("Rest seconds cannot be negative");

                _restSeconds = seconds;
                return this;
            }

            /// <summary>
            /// Sets the type of set (Normal, Dropset, Superset, etc.).
            /// </summary>
            /// <param name="setType">The set type.</param>
            public SetBuilder WithSetType(SetType setType)
            {
                _setType = setType;
                return this;
            }

            /// <summary>
            /// Builds the <see cref="Set"/> entity.
            /// </summary>
            public Set Build()
            {
                var set = new Set(_workoutExerciseId, _setNumber, _weight, _reps, _restSeconds, _setType);
                return set;
            }
        }

        #endregion
    }
}
