using CSharpFunctionalExtensions;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a workout session.
    /// </summary>
    public class Workout : BaseEntity
    {
        #region Constants

        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int NotesMaxLength = 2000;
        public const int MaxDurationHours = 12;

        #endregion

        #region Properties

        public Guid UserId { get; private set; }
        public Guid? WorkoutTemplateId { get; private set; }
        public string Name { get; private set; }
        public string? Notes { get; private set; }
        public DateTime WorkoutDate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsInProgress { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public decimal TotalVolumeKg { get; private set; }

        #endregion

        #region Constructors

        private Workout()
        {
            // For ORM
        }

        public Workout(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
            : base()
        {
            UserId = userId;
            WorkoutTemplateId = workoutTemplateId;
            Name = name;
            WorkoutDate = workoutDate;
            Notes = notes;
            Duration = TimeSpan.Zero;
            IsCompleted = false;
            IsInProgress = false;
            TotalVolumeKg = 0;
        }

        public Workout(
            Guid id,
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId,
            string? notes,
            TimeSpan duration,
            bool isCompleted,
            bool isInProgress,
            DateTime? startedAt,
            DateTime? completedAt,
            decimal totalVolumeKg)
            : base()
        {
            Id = id;
            UserId = userId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            WorkoutDate = workoutDate;
            WorkoutTemplateId = workoutTemplateId;
            Notes = notes;
            Duration = duration;
            IsCompleted = isCompleted;
            IsInProgress = isInProgress;
            StartedAt = startedAt;
            CompletedAt = completedAt;
            TotalVolumeKg = totalVolumeKg;
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<Workout>(this));
        }

        private Result<Workout, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<Workout, ValidationResult>(result);

            return Result.Success<Workout, ValidationResult>(this);
        }

        #endregion

        #region Factory

        public static Result<Workout, ValidationResult> Create(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
        {
            var workout = new Workout(userId, name, workoutDate, workoutTemplateId, notes);
            return workout.ValidateWithResult();
        }

        #endregion

        #region Domain Methods - Lifecycle

        public Result<Workout, ValidationResult> Start()
        {

            IsInProgress = true;
            StartedAt = DateTime.UtcNow;
            Duration = TimeSpan.FromSeconds(1);
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<Workout, ValidationResult> Pause()
        {

            IsInProgress = false;

            if (StartedAt.HasValue)
                Duration = DateTime.UtcNow - StartedAt.Value;

            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<Workout, ValidationResult> Resume()
        {

            IsInProgress = true;
            StartedAt = DateTime.UtcNow - Duration;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<Workout, ValidationResult> Complete()
        {

            if (IsInProgress && StartedAt.HasValue)
                Duration = DateTime.UtcNow - StartedAt.Value;

            IsCompleted = true;
            IsInProgress = false;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<Workout, ValidationResult> Uncomplete()
        {

            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        #endregion

        #region Domain Methods - Other

        public TimeSpan GetCurrentDuration()
        {
            if (IsInProgress && StartedAt.HasValue)
                return DateTime.UtcNow - StartedAt.Value;

            return Duration;
        }

        public Result<Workout, ValidationResult> SetDuration(TimeSpan duration)
        {
            Duration = duration;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public Result<Workout, ValidationResult> Update(string name, DateTime workoutDate, string? notes = null)
        {
            Name = name;
            WorkoutDate = workoutDate;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public bool IsToday() => WorkoutDate.Date == DateTime.UtcNow.Date;

        public bool IsPast() => WorkoutDate.Date < DateTime.UtcNow.Date;

        public bool IsFuture() => WorkoutDate.Date > DateTime.UtcNow.Date;

        #endregion
    }
}
