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
    /// Planned set in template
    /// </summary>
    public class TemplateSet : BaseEntity
    {
        public const decimal MaxWeightKg = 3000m;
        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600;

        public Guid WorkoutTemplateExerciseId { get; private set; }
        public int SetNumber { get; private set; }
        public Weight PlannedWeight { get; private set; }
        public int PlannedReps { get; private set; }
        public int? RestSeconds { get; private set; }
        public SetType SetType { get; private set; }

        private TemplateSet(
            Guid templateExerciseId,
            int setNumber,
            decimal plannedWeight,
            int plannedReps,
            SetType setType = SetType.Normal) : base()
        {
            if (templateExerciseId == Guid.Empty)
                throw new ArgumentException("Template exercise ID cannot be empty");

            if (setNumber < 1)
                throw new ArgumentException("Set number must be at least 1");

            if (plannedReps < 1)
                throw new ArgumentException("Reps must be at least 1");

            WorkoutTemplateExerciseId = templateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            SetType = setType;

            EnsureValid();
        }

        public TemplateSet(Guid workoutTemplateExerciseId, int setNumber, decimal plannedWeight, int plannedReps, int? restSeconds, SetType setType) : base()
        {
            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;
        }

        protected override IValidator GetValidator()
        {
            return new TemplateSetValidator();
        }

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

            public TemplateSet Build()
            {
                return new TemplateSet(
                    _templateExerciseId,
                    _setNumber,
                    _plannedWeight,
                    _plannedReps,
                    _restSeconds,
                    _setType);
            }
        }

        public void Update(decimal plannedWeight, int plannedReps, int? restSeconds = null)
        {
            if (plannedWeight < 0)
                throw new ArgumentException("Weight cannot be negative");

            if (plannedReps < 1)
                throw new ArgumentException("Reps must be at least 1");

            PlannedWeight = Weight.FromKilograms(plannedWeight);
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }
    }
}
