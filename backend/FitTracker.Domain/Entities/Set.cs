using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a single set within a workout exercise.
    /// </summary>
    public class Set : BaseEntity
    {
        public const int MaxReps = 1000;
        public const int MaxRestSeconds = 3600; // 1 hour
        public const decimal MaxWeightKg = 10000m;

        /// <summary>
        /// Gets the unique identifier of the workout exercise this set belongs to.
        /// </summary>
        public Guid WorkoutExerciseId { get; private set; }

        /// <summary>
        /// Gets the sequential number of this set within the workout exercise.
        /// </summary>
        public int SetNumber { get; private set; }

        /// <summary>
        /// Gets the weight used for this set.
        /// </summary>
        public Weight Weight { get; private set; }

        /// <summary>
        /// Gets the number of repetitions performed in this set.
        /// </summary>
        public int Reps { get; private set; }

        /// <summary>
        /// Gets the rest period in seconds before the next set, or null if not specified.
        /// </summary>
        public int? RestSeconds { get; private set; }

        /// <summary>
        /// Gets the type of this set (Normal, Dropset, Superset, etc.).
        /// </summary>
        public SetType SetType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this set has been completed.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets the date and time when this set was completed, or null if not yet completed.
        /// </summary>
        public DateTime? CompletedAt { get; private set; }

        internal Set(
            Guid id,
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds,
            SetType setType,
            bool isCompleted,
            DateTime? completedAt,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
            RestSeconds = restSeconds;
            SetType = setType;
            IsCompleted = isCompleted;
            CompletedAt = completedAt;
        }

        private Set()
        {
        }

        private Set(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds,
            SetType setType = SetType.Normal)
        {
            if (workoutExerciseId == Guid.Empty)
            {
                throw new ArgumentException("WorkoutExerciseId cannot be empty", nameof(workoutExerciseId));
            }

            if (setNumber <= 0)
            {
                throw new ArgumentException("Set number must be greater than 0", nameof(setNumber));
            }

            if (weight == null)
            {
                throw new ArgumentNullException(nameof(weight));
            }

            if (weight.ToKilograms() > MaxWeightKg)
            {
                throw new ArgumentException($"Weight cannot exceed {MaxWeightKg} kg", nameof(weight));
            }

            if (reps <= 0)
            {
                throw new ArgumentException("Reps must be greater than 0", nameof(reps));
            }

            if (reps > MaxReps)
            {
                throw new ArgumentException($"Reps cannot exceed {MaxReps}", nameof(reps));
            }

            if (restSeconds.HasValue && restSeconds.Value < 0)
            {
                throw new ArgumentException("Rest seconds cannot be negative", nameof(restSeconds));
            }

            if (restSeconds.HasValue && restSeconds.Value > MaxRestSeconds)
            {
                throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds", nameof(restSeconds));
            }

            WorkoutExerciseId = workoutExerciseId;
            SetNumber = setNumber;
            Weight = weight;
            Reps = reps;
            RestSeconds = restSeconds;
            SetType = setType;
            IsCompleted = false;
        }

        public static Set Create(
            Guid workoutExerciseId,
            int setNumber,
            Weight weight,
            int reps,
            int? restSeconds = null,
            SetType setType = SetType.Normal)
        {
            return new Set(workoutExerciseId, setNumber, weight, reps, restSeconds, setType);
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

        public void UpdateWeight(Weight weight)
        {
            if (weight == null)
            {
                throw new ArgumentNullException(nameof(weight));
            }

            if (weight.ToKilograms() > MaxWeightKg)
            {
                throw new ArgumentException($"Weight cannot exceed {MaxWeightKg} kg", nameof(weight));
            }

            Weight = weight;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseWeight(decimal amountKg)
        {
            if (amountKg <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amountKg));
            }

            var newWeightKg = Weight.ToKilograms() + amountKg;
            if (newWeightKg > MaxWeightKg)
            {
                throw new InvalidOperationException($"New weight would exceed maximum of {MaxWeightKg} kg");
            }

            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecreaseWeight(decimal amountKg)
        {
            if (amountKg <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amountKg));
            }

            var newWeightKg = Weight.ToKilograms() - amountKg;
            if (newWeightKg < 0)
            {
                throw new InvalidOperationException("Weight cannot be negative");
            }

            Weight = Weight.FromKilograms(newWeightKg);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateReps(int reps)
        {
            if (reps <= 0)
            {
                throw new ArgumentException("Reps must be greater than 0", nameof(reps));
            }

            if (reps > MaxReps)
            {
                throw new ArgumentException($"Reps cannot exceed {MaxReps}", nameof(reps));
            }

            Reps = reps;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRest(int? seconds)
        {
            if (seconds.HasValue)
            {
                if (seconds.Value < 0)
                {
                    throw new ArgumentException("Rest seconds cannot be negative", nameof(seconds));
                }

                if (seconds.Value > MaxRestSeconds)
                {
                    throw new ArgumentException($"Rest cannot exceed {MaxRestSeconds} seconds", nameof(seconds));
                }
            }

            RestSeconds = seconds;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeSetType(SetType setType)
        {
            SetType = setType;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (IsCompleted)
            {
                return;
            }

            IsCompleted = true;
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

        public decimal CalculateVolume() => Weight.ToKilograms() * Reps;

        public decimal CalculateVolumeLbs() => Weight.ToPounds() * Reps;

        public bool IsPR(IEnumerable<Set> previousSets)
        {
            if (previousSets == null || !previousSets.Any())
            {
                return true;
            }

            var maxPreviousWeight = previousSets.Max(s => s.Weight.ToKilograms());
            return Weight.ToKilograms() > maxPreviousWeight;
        }

        public bool IsWarmupSet() => SetType == SetType.Warmup;

        public bool IsWorkingSet() => SetType == SetType.Normal;
    }
}
