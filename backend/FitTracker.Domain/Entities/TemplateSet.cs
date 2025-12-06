using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a planned set within a workout template exercise.
    /// </summary>
    public class TemplateSet : BaseEntity
    {
        public const decimal MaxWeightKg = 10000m;
        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600;

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

        internal TemplateSet(
            Guid id,
            Guid workoutTemplateExerciseId,
            int setNumber,
            Weight plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = plannedWeight;
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;
        }

        private TemplateSet()
        {
        }

        private TemplateSet(
            Guid workoutTemplateExerciseId,
            int setNumber,
            Weight plannedWeight,
            int plannedReps,
            int? restSeconds,
            SetType setType = SetType.Normal)
        {
            if (workoutTemplateExerciseId == Guid.Empty)
            {
                throw new ArgumentException("WorkoutTemplateExerciseId cannot be empty", nameof(workoutTemplateExerciseId));
            }

            if (setNumber <= 0)
            {
                throw new ArgumentException("Set number must be greater than 0", nameof(setNumber));
            }

            if (plannedWeight == null)
            {
                throw new ArgumentNullException(nameof(plannedWeight));
            }

            if (plannedWeight.ToKilograms() > MaxWeightKg)
            {
                throw new ArgumentException($"Planned weight cannot exceed {MaxWeightKg} kg", nameof(plannedWeight));
            }

            if (plannedReps <= 0)
            {
                throw new ArgumentException("Planned reps must be greater than 0", nameof(plannedReps));
            }

            if (plannedReps > MaxReps)
            {
                throw new ArgumentException($"Planned reps cannot exceed {MaxReps}", nameof(plannedReps));
            }

            if (restSeconds.HasValue && restSeconds.Value < 0)
            {
                throw new ArgumentException("Rest seconds cannot be negative", nameof(restSeconds));
            }

            if (restSeconds.HasValue && restSeconds.Value > MaxRestSeconds)
            {
                throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds", nameof(restSeconds));
            }

            WorkoutTemplateExerciseId = workoutTemplateExerciseId;
            SetNumber = setNumber;
            PlannedWeight = plannedWeight;
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            SetType = setType;
        }

        public static TemplateSet Create(
            Guid workoutTemplateExerciseId,
            int setNumber,
            Weight plannedWeight,
            int plannedReps,
            int? restSeconds = null,
            SetType setType = SetType.Normal)
        {
            return new TemplateSet(workoutTemplateExerciseId, setNumber, plannedWeight, plannedReps, restSeconds, setType);
        }

        public void Update(
            Weight plannedWeight,
            int plannedReps,
            int? restSeconds = null)
        {
            if (plannedWeight == null)
            {
                throw new ArgumentNullException(nameof(plannedWeight));
            }

            if (plannedWeight.ToKilograms() > MaxWeightKg)
            {
                throw new ArgumentException($"Planned weight cannot exceed {MaxWeightKg} kg", nameof(plannedWeight));
            }

            if (plannedReps <= 0)
            {
                throw new ArgumentException("Planned reps must be greater than 0", nameof(plannedReps));
            }

            if (plannedReps > MaxReps)
            {
                throw new ArgumentException($"Planned reps cannot exceed {MaxReps}", nameof(plannedReps));
            }

            if (restSeconds.HasValue && restSeconds.Value < 0)
            {
                throw new ArgumentException("Rest seconds cannot be negative", nameof(restSeconds));
            }

            if (restSeconds.HasValue && restSeconds.Value > MaxRestSeconds)
            {
                throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds", nameof(restSeconds));
            }

            PlannedWeight = plannedWeight;
            PlannedReps = plannedReps;
            RestSeconds = restSeconds;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSetNumber(int newSetNumber)
        {
            if (newSetNumber <= 0)
            {
                throw new ArgumentException("Set number must be greater than 0", nameof(newSetNumber));
            }

            SetNumber = newSetNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeSetType(SetType setType)
        {
            SetType = setType;
            UpdatedAt = DateTime.UtcNow;
        }

        public decimal CalculatePlannedVolume() => PlannedWeight.ToKilograms() * PlannedReps;

        public decimal CalculatePlannedVolumeLbs() => PlannedWeight.ToPounds() * PlannedReps;

        public bool IsWarmupSet() => SetType == SetType.Warmup;

        public bool IsWorkingSet() => SetType == SetType.Normal;

        public bool HasRestPeriod() => RestSeconds.HasValue;
    }
}
