using System;
using System.Collections.Generic;
using System.Linq;
using FitTracker.Domain.Validators;
using FluentValidation;

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

        /// <summary>
        /// Gets the unique identifier of the user performing the workout.
        /// </summary>
        public Guid UserId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the optional unique identifier of the workout template.
        /// </summary>
        public Guid? WorkoutTemplateId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the workout.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the optional notes for the workout.
        /// </summary>
        public string? Notes
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date when the workout occurred.
        /// </summary>
        public DateTime WorkoutDate
        {
            get; private set;
        }

        /// <summary>
        /// Gets the duration of the workout.
        /// </summary>
        public TimeSpan Duration
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the workout is completed.
        /// </summary>
        public bool IsCompleted
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the workout is currently in progress.
        /// </summary>
        public bool IsInProgress
        {
            get; private set;
        }

        /// <summary>
        /// Gets the timestamp when the workout was started, or null if not started.
        /// </summary>
        public DateTime? StartedAt
        {
            get; private set;
        }

        /// <summary>
        /// Gets the timestamp when the workout was completed, or null if not completed.
        /// </summary>
        public DateTime? CompletedAt
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total volume lifted during the workout, in kilograms.
        /// </summary>
        public decimal TotalVolumeKg
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private Workout()
        {
        }

        /// <summary>
        /// Constructor for restoring workout from persistence layer.
        /// Use <see cref="WorkoutBuilder"/> for creating new workouts.
        /// </summary>
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
            decimal totalVolumeKg) : base()
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

            EnsureValid();
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new workouts.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private Workout(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null) : base()
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

            EnsureValid();
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutValidator();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="WorkoutBuilder"/> instance.
        /// </summary>
        public static WorkoutBuilder CreateBuilder()
        {
            return new WorkoutBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="Workout"/> instances.
        /// </summary>
        public class WorkoutBuilder
        {
            private Guid _userId;
            private string _name = string.Empty;
            private DateTime _workoutDate = DateTime.UtcNow;
            private Guid? _workoutTemplateId;
            private string? _notes;

            public WorkoutBuilder ForUser(Guid userId)
            {
                _userId = userId;
                return this;
            }

            public WorkoutBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public WorkoutBuilder OnDate(DateTime date)
            {
                _workoutDate = date;
                return this;
            }

            public WorkoutBuilder FromTemplate(Guid? templateId)
            {
                _workoutTemplateId = templateId;
                return this;
            }

            public WorkoutBuilder WithNotes(string? notes)
            {
                _notes = notes;
                return this;
            }

            /// <summary>
            /// Builds the <see cref="Workout"/> entity.
            /// </summary>
            public Workout Build()
            {
                return new Workout(_userId, _name, _workoutDate, _workoutTemplateId, _notes);
            }
        }

        #endregion

        #region Domain Methods - Workout Lifecycle

        /// <summary>
        /// Starts the workout (begins timer).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if workout is already in progress or completed.</exception>
        public void Start()
        {
            if (IsInProgress)
                throw new InvalidOperationException("Workout is already in progress");

            if (IsCompleted)
                throw new InvalidOperationException("Cannot start completed workout");

            IsInProgress = true;
            StartedAt = DateTime.UtcNow;
            Duration = TimeSpan.FromSeconds(1);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Pauses the workout (stops timer).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if workout is not in progress or already completed.</exception>
        public void Pause()
        {
            if (!IsInProgress)
                throw new InvalidOperationException("Workout is not in progress");

            if (IsCompleted)
                throw new InvalidOperationException("Cannot pause completed workout");

            IsInProgress = false;

            if (StartedAt.HasValue)
            {
                Duration = DateTime.UtcNow - StartedAt.Value;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Resumes the workout (continues timer).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if workout is already in progress or completed.</exception>
        public void Resume()
        {
            if (IsInProgress)
                throw new InvalidOperationException("Workout is already in progress");

            if (IsCompleted)
                throw new InvalidOperationException("Cannot resume completed workout");

            IsInProgress = true;
            StartedAt = DateTime.UtcNow - Duration;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Completes the workout (finalizes duration).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if workout is already completed.</exception>
        public void Complete()
        {
            if (IsCompleted)
                throw new InvalidOperationException("Workout is already completed");

            if (IsInProgress && StartedAt.HasValue)
            {
                Duration = DateTime.UtcNow - StartedAt.Value;
            }

            IsCompleted = true;
            IsInProgress = false;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            // TODO: Calculate total volume if needed
        }

        /// <summary>
        /// Marks the workout as incomplete.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if workout is not completed.</exception>
        public void Uncomplete()
        {
            if (!IsCompleted)
                throw new InvalidOperationException("Workout is not completed");

            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Domain Methods - Other

        /// <summary>
        /// Gets the current duration, accounting for in-progress workouts.
        /// </summary>
        /// <returns>The current workout duration.</returns>
        public TimeSpan GetCurrentDuration()
        {
            if (IsInProgress && StartedAt.HasValue)
            {
                return DateTime.UtcNow - StartedAt.Value;
            }

            return Duration;
        }

        /// <summary>
        /// Manually sets the workout duration (for completed workouts).
        /// </summary>
        /// <param name="duration">The duration to set.</param>
        /// <exception cref="InvalidOperationException">Thrown if workout is in progress.</exception>
        /// <exception cref="ArgumentException">Thrown if duration is negative or exceeds the maximum allowed duration.</exception>
        public void SetDuration(TimeSpan duration)
        {
            if (IsInProgress)
                throw new InvalidOperationException("Cannot manually set duration while workout is in progress");

            if (duration.TotalHours > MaxDurationHours)
                throw new ArgumentException($"Duration cannot exceed {MaxDurationHours} hours");

            if (duration < TimeSpan.Zero)
                throw new ArgumentException("Duration cannot be negative");

            Duration = duration;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates workout details.
        /// </summary>
        /// <param name="name">The new workout name.</param>
        /// <param name="workoutDate">The new workout date.</param>
        /// <param name="notes">The new notes (optional).</param>
        /// <exception cref="ArgumentException">Thrown if name is null or whitespace.</exception>
        public void Update(string name, DateTime workoutDate, string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Workout name cannot be empty");

            Name = name;
            WorkoutDate = workoutDate;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Returns true if the workout date is today (UTC).
        /// </summary>
        public bool IsToday() => WorkoutDate.Date == DateTime.UtcNow.Date;

        /// <summary>
        /// Returns true if the workout date is in the past (UTC).
        /// </summary>
        public bool IsPast() => WorkoutDate.Date < DateTime.UtcNow.Date;

        /// <summary>
        /// Returns true if the workout date is in the future (UTC).
        /// </summary>
        public bool IsFuture() => WorkoutDate.Date > DateTime.UtcNow.Date;

        #endregion
    }
}
