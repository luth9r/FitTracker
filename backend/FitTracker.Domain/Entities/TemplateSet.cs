using CSharpFunctionalExtensions;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a planned set within a workout template exercise.
    /// </summary>
    public class TemplateSet : BaseEntity
    {
        #region Constants

        public const decimal MaxWeightKg = 3000m;
        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the workout template exercise this set belongs to.
        /// </summary>
        public Guid WorkoutTemplateExerciseId { get; private set; }

        /// <summary>
        /// Gets the sequential number of this set within the template exercise.
        /// </summary>
        public int SetNumber { get; private set; }

        /// <summary>
        /// Gets the planned weight for this set.
        /// </summary>
        public Weight PlannedWeight { get; private set; }

        /// <summary>
        /// Gets the planned number of repetitions for this set.
        /// </summary>
        public int PlannedReps { get; private set; }

        /// <summary>
        /// Gets the planned rest period in seconds before the next set, or null if not specified.
        /// </summary>
        public int? RestSeconds { get; private set; }

        /// <summary>
        /// Gets the type of this set (Normal, Dropset, Superset, etc.).
        /// </summary>
        public SetType SetType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private TemplateSet()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new template sets.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private TemplateSet(
            Guid templateExerciseId,
            int setNumber,
            decimal plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType = SetType.Normal) : base()
        {
            WorkoutTemplateExerciseId = templateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;
        }

        /// <summary>
        /// Constructor for restoring template set from persistence layer.
        /// Use <see cref="Create"/> for creating new template sets.
        /// </summary>
        public TemplateSet(
            Guid workoutTemplateExerciseId,
            int setNumber,
            Weight plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType) : base()
        {
            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = plannedWeight;
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new TemplateSetValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<TemplateSet>(this));
        }

        private Result<TemplateSet, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<TemplateSet, ValidationResult>(result);

            return Result.Success<TemplateSet, ValidationResult>(this);
        }

        #endregion

        #region Factory

        public static Result<TemplateSet, ValidationResult> Create(
            Guid workoutTemplateExerciseId,
            int setNumber,
            decimal plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType = SetType.Normal)
        {
            var templateSet = new TemplateSet(workoutTemplateExerciseId, setNumber, plannedWeight, plannedReps, restSeconds, setType);
            return templateSet.ValidateWithResult();
        }

        #endregion

        #region Domain Methods

        public Result<TemplateSet, ValidationResult> Update(
            decimal plannedWeight,
            int plannedReps,
            int? restSeconds = null)
        {
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        #endregion

        #region Builder

        public static TemplateSetBuilder CreateBuilder() => new TemplateSetBuilder();

        public class TemplateSetBuilder
        {
            private Guid _templateExerciseId;
            private int _setNumber;
            private decimal _plannedWeight;
            private int _plannedReps;
            private int? _restSeconds;
            private SetType _setType = SetType.Normal;

            public TemplateSetBuilder WithTemplateExercise(Guid id)
            {
                _templateExerciseId = id;
                return this;
            }

            public TemplateSetBuilder WithSetNumber(int number)
            {
                _setNumber = number;
                return this;
            }

            public TemplateSetBuilder WithPlannedWeight(decimal weight)
            {
                _plannedWeight = weight;
                return this;
            }

            public TemplateSetBuilder WithPlannedReps(int reps)
            {
                _plannedReps = reps;
                return this;
            }

            public TemplateSetBuilder WithRest(int? seconds)
            {
                _restSeconds = seconds;
                return this;
            }

            public TemplateSetBuilder WithSetType(SetType type)
            {
                _setType = type;
                return this;
            }

            public Result<TemplateSet, ValidationResult> Build()
            {
                var set = new TemplateSet(
                    _templateExerciseId,
                    _setNumber,
                    _plannedWeight,
                    _plannedReps,
                    _restSeconds,
                    _setType);

                return set.ValidateWithResult();
            }
        }

        #endregion
    }
}
