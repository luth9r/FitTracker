namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a workout session.
    /// </summary>
    public class Workout : BaseEntity
    {
        /// <summary>
        /// The maximum length allowed for the workout name.
        /// </summary>
        public const int NameMaxLength = 100;

        /// <summary>
        /// The minimum length required for the workout name.
        /// </summary>
        public const int NameMinLength = 3;

        /// <summary>
        /// The maximum length allowed for the workout notes.
        /// </summary>
        public const int NotesMaxLength = 2000;

        /// <summary>
        /// The maximum duration allowed for a workout in hours.
        /// </summary>
        public const int MaxDurationHours = 12;

        /// <summary>
        /// Gets the unique identifier of the user who performed the workout.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the workout template used, if any.
        /// </summary>
        public Guid? WorkoutTemplateId { get; private set; }

        /// <summary>
        /// Gets the name of the workout.
        /// </summary>
        public string Name { get; private set; } = default!;

        /// <summary>
        /// Gets the notes for the workout.
        /// </summary>
        public string? Notes { get; private set; }

        /// <summary>
        /// Gets the date of the workout.
        /// </summary>
        public DateTime WorkoutDate { get; private set; }

        /// <summary>
        /// Gets the duration of the workout.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the workout is completed.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the workout is currently in progress.
        /// </summary>
        public bool IsInProgress { get; private set; }

        /// <summary>
        /// Gets the date and time when the workout started.
        /// </summary>
        public DateTime? StartedAt { get; private set; }

        /// <summary>
        /// Gets the date and time when the workout was completed.
        /// </summary>
        public DateTime? CompletedAt { get; private set; }

        /// <summary>
        /// Gets the total volume lifted during the workout in kilograms.
        /// </summary>
        public decimal TotalVolumeKg { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Workout"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the workout.</param>
        /// <param name="workoutDate">The date of the workout.</param>
        /// <param name="workoutTemplateId">The unique identifier of the template.</param>
        /// <param name="notes">The notes for the workout.</param>
        /// <param name="duration">The duration of the workout.</param>
        /// <param name="isCompleted">Whether the workout is completed.</param>
        /// <param name="isInProgress">Whether the workout is in progress.</param>
        /// <param name="startedAt">The start time.</param>
        /// <param name="completedAt">The completion time.</param>
        /// <param name="totalVolumeKg">The total volume in kg.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
        internal Workout(
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
            decimal totalVolumeKg,
            DateTime createdAt,
            DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            Name = name;
            WorkoutDate = workoutDate;
            WorkoutTemplateId = workoutTemplateId;
            Notes = notes;
            Duration = duration;
            IsCompleted = isCompleted;
            IsInProgress = isInProgress;
            StartedAt = startedAt;
            CompletedAt = completedAt;
            TotalVolumeKg = totalVolumeKg;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Workout"/> class.
        /// </summary>
        private Workout()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Workout"/> class.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the workout.</param>
        /// <param name="workoutDate">The date of the workout.</param>
        /// <param name="workoutTemplateId">The unique identifier of the template.</param>
        /// <param name="notes">The notes for the workout.</param>
        private Workout(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
            : base()
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name must be {NameMinLength}-{NameMaxLength} characters", nameof(name));
            }

            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            UserId = userId;
            WorkoutTemplateId = workoutTemplateId;
            Name = name;
            WorkoutDate = workoutDate.Date;
            Notes = notes;
            Duration = TimeSpan.Zero;
            IsCompleted = false;
            IsInProgress = false;
            TotalVolumeKg = 0;
        }

        /// <summary>
        /// Creates a new <see cref="Workout"/>.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="name">The name of the workout.</param>
        /// <param name="workoutDate">The date of the workout.</param>
        /// <param name="workoutTemplateId">The unique identifier of the template.</param>
        /// <param name="notes">The notes for the workout.</param>
        /// <returns>A new instance of <see cref="Workout"/>.</returns>
        public static Workout Create(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
        {
            return new Workout(userId, name, workoutDate, workoutTemplateId, notes);
        }

        /// <summary>
        /// Starts the workout.
        /// </summary>
        public void Start()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("Cannot start a completed workout");
            }

            if (IsInProgress)
            {
                return;
            }

            IsInProgress = true;
            StartedAt = DateTime.UtcNow;
            Duration = TimeSpan.FromSeconds(1);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Pauses the workout.
        /// </summary>
        public void Pause()
        {
            if (!IsInProgress)
            {
                return;
            }

            IsInProgress = false;
            if (StartedAt.HasValue)
            {
                Duration = DateTime.UtcNow - StartedAt.Value;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Resumes the workout.
        /// </summary>
        public void Resume()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("Cannot resume a completed workout");
            }

            if (IsInProgress)
            {
                return;
            }

            IsInProgress = true;
            StartedAt = DateTime.UtcNow - Duration;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Completes the workout.
        /// </summary>
        public void Complete()
        {
            if (IsCompleted)
            {
                return;
            }

            if (IsInProgress && StartedAt.HasValue)
            {
                Duration = DateTime.UtcNow - StartedAt.Value;
            }

            if (Duration.TotalHours > MaxDurationHours)
            {
                throw new InvalidOperationException($"Workout duration cannot exceed {MaxDurationHours} hours");
            }

            IsCompleted = true;
            IsInProgress = false;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the workout as not completed.
        /// </summary>
        public void Uncomplete()
        {
            if (!IsCompleted)
            {
                return;
            }

            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets the duration of the workout manually.
        /// </summary>
        /// <param name="duration">The new duration.</param>
        public void SetDuration(TimeSpan duration)
        {
            if (duration.TotalHours > MaxDurationHours)
            {
                throw new ArgumentException($"Duration cannot exceed {MaxDurationHours} hours", nameof(duration));
            }

            Duration = duration;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the workout details.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <param name="workoutDate">The new date.</param>
        /// <param name="notes">The new notes.</param>
        public void Update(string name, DateTime workoutDate, string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name must be {NameMinLength}-{NameMaxLength} characters", nameof(name));
            }

            if (notes?.Length > NotesMaxLength)
            {
                throw new ArgumentException($"Notes cannot exceed {NotesMaxLength} characters", nameof(notes));
            }

            if (IsCompleted)
            {
                throw new InvalidOperationException("Cannot update a completed workout");
            }

            Name = name;
            WorkoutDate = workoutDate.Date;
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the current duration of the workout, accounting for whether it is currently in progress.
        /// </summary>
        /// <returns>The current duration.</returns>
        public TimeSpan GetCurrentDuration()
        {
            if (IsInProgress && StartedAt.HasValue)
            {
                return DateTime.UtcNow - StartedAt.Value;
            }

            return Duration;
        }

        /// <summary>
        /// Determines whether the workout is scheduled for today.
        /// </summary>
        /// <returns><c>true</c> if the workout is today; otherwise, <c>false</c>.</returns>
        public bool IsToday() => WorkoutDate.Date == DateTime.UtcNow.Date;

        /// <summary>
        /// Determines whether the workout is in the past.
        /// </summary>
        /// <returns><c>true</c> if the workout is in the past; otherwise, <c>false</c>.</returns>
        public bool IsPast() => WorkoutDate.Date < DateTime.UtcNow.Date;

        /// <summary>
        /// Determines whether the workout is in the future.
        /// </summary>
        /// <returns><c>true</c> if the workout is in the future; otherwise, <c>false</c>.</returns>
        public bool IsFuture() => WorkoutDate.Date > DateTime.UtcNow.Date;

        /// <summary>
        /// Determines whether the workout can be started.
        /// </summary>
        /// <returns><c>true</c> if the workout can be started; otherwise, <c>false</c>.</returns>
        public bool CanStart() => !IsCompleted && !IsInProgress;

        /// <summary>
        /// Determines whether the workout can be completed.
        /// </summary>
        /// <returns><c>true</c> if the workout can be completed; otherwise, <c>false</c>.</returns>
        public bool CanComplete() => IsInProgress || (Duration.TotalHours > 0);
    }
}
