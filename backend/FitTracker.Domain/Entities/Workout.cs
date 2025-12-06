namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a workout session.
    /// </summary>
    public class Workout : BaseEntity
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int NotesMaxLength = 2000;
        public const int MaxDurationHours = 12;

        public Guid UserId { get; private set; }

        public Guid? WorkoutTemplateId { get; private set; }

        public string Name { get; private set; } = default!;

        public string? Notes { get; private set; }

        public DateTime WorkoutDate { get; private set; }

        public TimeSpan Duration { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsInProgress { get; private set; }

        public DateTime? StartedAt { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public decimal TotalVolumeKg { get; private set; }

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

        private Workout()
        {
        }

        private Workout(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
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

        public static Workout Create(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
        {
            return new Workout(userId, name, workoutDate, workoutTemplateId, notes);
        }

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

        public void SetDuration(TimeSpan duration)
        {
            if (duration.TotalHours > MaxDurationHours)
            {
                throw new ArgumentException($"Duration cannot exceed {MaxDurationHours} hours", nameof(duration));
            }

            Duration = duration;
            UpdatedAt = DateTime.UtcNow;
        }

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

        public TimeSpan GetCurrentDuration()
        {
            if (IsInProgress && StartedAt.HasValue)
            {
                return DateTime.UtcNow - StartedAt.Value;
            }

            return Duration;
        }

        public bool IsToday() => WorkoutDate.Date == DateTime.UtcNow.Date;

        public bool IsPast() => WorkoutDate.Date < DateTime.UtcNow.Date;

        public bool IsFuture() => WorkoutDate.Date > DateTime.UtcNow.Date;

        public bool CanStart() => !IsCompleted && !IsInProgress;

        public bool CanComplete() => IsInProgress || (Duration.TotalHours > 0);
    }
}
