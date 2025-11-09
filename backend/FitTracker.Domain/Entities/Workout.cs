// FitTracker.Domain/Entities/Workout.cs
using System;
using System.Collections.Generic;
using System.Linq;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a workout session
    /// </summary>
    public class Workout : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int NotesMaxLength = 2000;
        public const int MaxDurationHours = 12;

        // ============================================
        // Properties
        // ============================================
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

        // Navigation Properties
        public User? User { get; private set; }
        public WorkoutTemplate? WorkoutTemplate { get; private set; }
        public ICollection<WorkoutExercise> Exercises { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// EF Core constructor
        /// </summary>
        private Workout()
        {
            Name = string.Empty;
            WorkoutDate = DateTime.UtcNow;
            Duration = TimeSpan.Zero;
            Exercises = new HashSet<WorkoutExercise>();
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

            Exercises = new HashSet<WorkoutExercise>();

            EnsureValid();
        }

        /// <summary>
        /// Domain constructor
        /// </summary>
        private Workout(
            Guid userId,
            string name,
            DateTime workoutDate,
            Guid? workoutTemplateId = null,
            string? notes = null)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Workout name cannot be empty");

            UserId = userId;
            WorkoutTemplateId = workoutTemplateId;
            Name = name;
            WorkoutDate = workoutDate;
            Notes = notes;
            Duration = TimeSpan.Zero;
            IsCompleted = false;
            IsInProgress = false;
            TotalVolumeKg = 0;
            Exercises = new HashSet<WorkoutExercise>();

            EnsureValid();
        }

        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new WorkoutValidator();
        }

        // ============================================
        // Builder Pattern
        // ============================================

        public static WorkoutBuilder CreateBuilder() => new WorkoutBuilder();

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

            public Workout Build()
            {
                return new Workout(_userId, _name, _workoutDate, _workoutTemplateId, _notes);
            }
        }

        // ============================================
        // Domain Methods - Workout Lifecycle
        // ============================================

        /// <summary>
        /// Start the workout (begins timer)
        /// </summary>
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
        /// Pause the workout (stops timer)
        /// </summary>
        public void Pause()
        {
            if (!IsInProgress)
                throw new InvalidOperationException("Workout is not in progress");

            if (IsCompleted)
                throw new InvalidOperationException("Cannot pause completed workout");

            IsInProgress = false;

            // Calculate actual duration
            if (StartedAt.HasValue)
            {
                var elapsed = DateTime.UtcNow - StartedAt.Value;
                Duration = elapsed;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Resume the workout (continues timer)
        /// </summary>
        public void Resume()
        {
            if (IsInProgress)
                throw new InvalidOperationException("Workout is already in progress");

            if (IsCompleted)
                throw new InvalidOperationException("Cannot resume completed workout");

            IsInProgress = true;
            StartedAt = DateTime.UtcNow - Duration;  // Adjust start time to account for elapsed duration
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Complete the workout (finalizes duration)
        /// </summary>
        public void Complete()
        {
            if (IsCompleted)
                throw new InvalidOperationException("Workout is already completed");

            // Calculate final duration
            if (IsInProgress && StartedAt.HasValue)
            {
                Duration = DateTime.UtcNow - StartedAt.Value;
            }

            IsCompleted = true;
            IsInProgress = false;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            CalculateTotalVolume();
        }

        /// <summary>
        /// Mark workout as incomplete
        /// </summary>
        public void Uncomplete()
        {
            if (!IsCompleted)
                throw new InvalidOperationException("Workout is not completed");

            IsCompleted = false;
            CompletedAt = null;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Get current duration (accounts for in-progress workouts)
        /// </summary>
        public TimeSpan GetCurrentDuration()
        {
            if (IsInProgress && StartedAt.HasValue)
            {
                return DateTime.UtcNow - StartedAt.Value;
            }

            return Duration;
        }

        /// <summary>
        /// Manually set duration (for completed workouts)
        /// </summary>
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

        // ============================================
        // Domain Methods - Other
        // ============================================

        /// <summary>
        /// Update workout details
        /// </summary>
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
        /// Add exercise to workout
        /// </summary>
        public void AddExercise(WorkoutExercise exercise)
        {
            if (exercise == null)
                throw new ArgumentNullException(nameof(exercise));

            if (Exercises.Any(e => e.ExerciseId == exercise.ExerciseId))
                throw new InvalidOperationException("Exercise already exists in workout");

            Exercises.Add(exercise);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove exercise from workout
        /// </summary>
        public void RemoveExercise(WorkoutExercise exercise)
        {
            if (exercise == null)
                throw new ArgumentNullException(nameof(exercise));

            if (!Exercises.Contains(exercise))
                throw new InvalidOperationException("Exercise not found in workout");

            Exercises.Remove(exercise);
            UpdatedAt = DateTime.UtcNow;

            CalculateTotalVolume();
        }

        /// <summary>
        /// Calculate total volume from all exercises
        /// </summary>
        public void CalculateTotalVolume()
        {
            TotalVolumeKg = Exercises
                .SelectMany(e => e.Sets)
                .Sum(s => s.CalculateVolume());

            UpdatedAt = DateTime.UtcNow;
        }

        public int GetTotalSets() => Exercises.Sum(e => e.Sets.Count);

        public int GetTotalReps() => Exercises.SelectMany(e => e.Sets).Sum(s => s.Reps);

        public bool IsToday() => WorkoutDate.Date == DateTime.UtcNow.Date;

        public bool IsPast() => WorkoutDate.Date < DateTime.UtcNow.Date;

        public bool IsFuture() => WorkoutDate.Date > DateTime.UtcNow.Date;
    }
}
