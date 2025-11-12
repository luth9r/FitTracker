using System;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;
using CSharpFunctionalExtensions;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout template.
    /// </summary>
    public class WorkoutTemplateExercise : BaseEntity
    {
        #region Constants

        public const int NotesMaxLength = 500;

        #endregion

        #region Properties

        public Guid WorkoutTemplateId { get; private set; }
        public Guid ExerciseId { get; private set; }
        public int OrderIndex { get; private set; }
        public string? Notes { get; private set; }

        #endregion

        #region Constructors

        private WorkoutTemplateExercise()
        {
            // For ORM
        }

        public WorkoutTemplateExercise(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null) : base()
        {
            WorkoutTemplateId = workoutTemplateId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateExerciseValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<WorkoutTemplateExercise>(this));
        }

        private Result<WorkoutTemplateExercise, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<WorkoutTemplateExercise, ValidationResult>(result);
            return Result.Success<WorkoutTemplateExercise, ValidationResult>(this);
        }

        #endregion

        #region Factory

        public static Result<WorkoutTemplateExercise, ValidationResult> Create(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            var workoutTemplateExercise = new WorkoutTemplateExercise(workoutTemplateId, exerciseId, orderIndex, notes);
            return workoutTemplateExercise.ValidateWithResult();
        }

        #endregion

        #region Domain Methods

        public Result<WorkoutTemplateExercise, ValidationResult> UpdateOrder(int newOrder)
        {
            if (newOrder < 1)
                throw new ArgumentException("Order must be at least 1", nameof(newOrder));

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<WorkoutTemplateExercise, ValidationResult> UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        #endregion

        #region Builder

        public static WorkoutTemplateExerciseBuilder CreateBuilder() => new WorkoutTemplateExerciseBuilder();

        public class WorkoutTemplateExerciseBuilder
        {
            private Guid _templateId;
            private Guid _exerciseId;
            private int _orderIndex = 1;
            private string? _notes;

            public WorkoutTemplateExerciseBuilder WithTemplate(Guid templateId)
            {
                _templateId = templateId;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithExercise(Guid exerciseId)
            {
                _exerciseId = exerciseId;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithOrder(int orderIndex)
            {
                _orderIndex = orderIndex;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithNotes(string? notes)
            {
                _notes = notes;
                return this;
            }

            public Result<WorkoutTemplateExercise, ValidationResult> Build()
            {
                var workoutTemplateExercise = new WorkoutTemplateExercise(_templateId, _exerciseId, _orderIndex, _notes);
                return workoutTemplateExercise.ValidateWithResult();
            }
        }

        #endregion
    }
}
