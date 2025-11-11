using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;

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
        public Guid WorkoutTemplateExerciseId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sequential number of this set within the template exercise.
        /// </summary>
        public int SetNumber
        {
            get; private set;
        }

        /// <summary>
        /// Gets the planned weight for this set.
        /// </summary>
        public Weight PlannedWeight
        {
            get; private set;
        }

        /// <summary>
        /// Gets the planned number of repetitions for this set.
        /// </summary>
        public int PlannedReps
        {
            get; private set;
        }

        /// <summary>
        /// Gets the planned rest period in seconds before the next set, or null if not specified.
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
            int restSeconds,
            SetType setType = SetType.Normal
            ) : base()
        {
            WorkoutTemplateExerciseId = templateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring template set from persistence layer.
        /// Use <see cref="TemplateSetBuilder"/> for creating new template sets.
        /// </summary>
        public TemplateSet(
            Guid workoutTemplateExerciseId,
            int setNumber,
            decimal plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType) : base()
        {
            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new TemplateSetValidator();
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the planned parameters for this template set.
        /// </summary>
        /// <param name="plannedWeight">The new planned weight in kilograms.</param>
        /// <param name="plannedReps">The new planned number of repetitions.</param>
        /// <param name="restSeconds">The new planned rest period in seconds (optional).</param>
        public void Update(decimal plannedWeight, int plannedReps, int? restSeconds = null)
        {
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="TemplateSetBuilder"/> instance.
        /// </summary>
        public static TemplateSetBuilder CreateBuilder()
        {
            return new TemplateSetBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="TemplateSet"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="TemplateSet"/> entity.
            /// </summary>
            public TemplateSet Build()
            {
                var set = new TemplateSet(
                    _templateExerciseId,
                    _setNumber,
                    _plannedWeight,
                    _plannedReps,
                    _restSeconds,
                    _setType);

                return set;
            }
        }

        #endregion
    }
}
