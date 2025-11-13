using CSharpFunctionalExtensions;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout session.
    /// </summary>
    public class WorkoutExercise : BaseEntity
    {
        #region Constants

        public const int NotesMaxLength = 500;
        public const int MaxOrderIndex = 1000;

        #endregion

        #region Properties

        public Guid WorkoutId { get; private set; }
        public Guid ExerciseId { get; private set; }
        public int OrderIndex { get; private set; }
        public string? Notes { get; private set; }

        #endregion

        #region Constructors

        private WorkoutExercise()
        {
            // For ORM
        }

        public WorkoutExercise(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null) : base()
        {
            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutExerciseValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<WorkoutExercise>(this));
        }

        private Result<WorkoutExercise, ValidationResult> ValidateWithResult()
        {
            var validationResult = Validate();
            if (!validationResult.IsValid)
                return Result.Failure<WorkoutExercise, ValidationResult>(validationResult);
            return Result.Success<WorkoutExercise, ValidationResult>(this);
        }

        #endregion

        #region Factory

        public static Result<WorkoutExercise, ValidationResult> Create(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            var workoutExercise = new WorkoutExercise(workoutId, exerciseId, orderIndex, notes);
            return workoutExercise.ValidateWithResult();
        }

        #endregion

        #region Domain Methods

        public Result<WorkoutExercise, ValidationResult> UpdateOrder(int newOrder)
        {
            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<WorkoutExercise, ValidationResult> UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        #endregion

        #region Builder

        public static WorkoutExerciseBuilder CreateBuilder() => new WorkoutExerciseBuilder();

        public class WorkoutExerciseBuilder
        {
            private Guid _workoutId;
            private Guid _exerciseId;
            private int _orderIndex = 1;
            private string? _notes;

            public WorkoutExerciseBuilder WithWorkout(Guid workoutId)
            {
                _workoutId = workoutId;
                return this;
            }

            public WorkoutExerciseBuilder WithExercise(Guid exerciseId)
            {
                _exerciseId = exerciseId;
                return this;
            }

            public WorkoutExerciseBuilder WithOrder(int orderIndex)
            {
                _orderIndex = orderIndex;
                return this;
            }

            public WorkoutExerciseBuilder WithNotes(string? notes)
            {
                _notes = notes;
                return this;
            }

            public Result<WorkoutExercise, ValidationResult> Build()
            {
                var workoutExercise = new WorkoutExercise(_workoutId, _exerciseId, _orderIndex, _notes);
                return workoutExercise.ValidateWithResult();
            }
        }

        #endregion
    }
}
