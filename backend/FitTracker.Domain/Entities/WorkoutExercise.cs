using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout session
    /// </summary>
    public class WorkoutExercise : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int NotesMaxLength = 500;
        public const int MaxOrderIndex = 1000;

        // ============================================
        // Properties
        // ============================================
        public Guid WorkoutId { get; private set; }
        public Guid ExerciseId { get; private set; }
        public int OrderIndex { get; private set; }
        public string? Notes { get; private set; }

        // Navigation Properties
        public Workout? Workout { get; private set; }
        public Exercise? Exercise { get; private set; }
        public ICollection<Set> Sets { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// EF Core constructor
        /// </summary>
        private WorkoutExercise()
        {
            Sets = new HashSet<Set>();
        }

        /// <summary>
        /// Domain constructor
        /// </summary>
        public WorkoutExercise(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            if (workoutId == Guid.Empty)
                throw new ArgumentException("Workout ID cannot be empty");

            if (exerciseId == Guid.Empty)
                throw new ArgumentException("Exercise ID cannot be empty");

            if (orderIndex < 1)
                throw new ArgumentException("Order index must be at least 1");

            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
            Sets = new HashSet<Set>();

            EnsureValid();
        }






        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new WorkoutExerciseValidator();
        }

        // ============================================
        // Builder Pattern
        // ============================================

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

            public WorkoutExercise Build()
            {
                return new WorkoutExercise(_workoutId, _exerciseId, _orderIndex, _notes);
            }
        }

        // ============================================
        // Domain Methods
        // ============================================

        /// <summary>
        /// Update order index
        /// </summary>
        public void UpdateOrder(int newOrder)
        {
            if (newOrder < 1)
                throw new ArgumentException("Order index must be at least 1");

            if (newOrder > MaxOrderIndex)
                throw new ArgumentException($"Order index cannot exceed {MaxOrderIndex}");

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Update notes
        /// </summary>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Add set to exercise
        /// </summary>
        public void AddSet(Set set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            if (Sets.Any(s => s.SetNumber == set.SetNumber))
                throw new InvalidOperationException($"Set number {set.SetNumber} already exists");

            Sets.Add(set);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove set from exercise
        /// </summary>
        public void RemoveSet(Set set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            if (!Sets.Contains(set))
                throw new InvalidOperationException("Set not found in exercise");

            Sets.Remove(set);

            // Reorder remaining sets
            ReorderSets();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Reorder sets after deletion
        /// </summary>
        private void ReorderSets()
        {
            var orderedSets = Sets.OrderBy(s => s.SetNumber).ToList();
            for (int i = 0; i < orderedSets.Count; i++)
            {
                 orderedSets[i].UpdateSetNumber(i + 1);
            }
        }

        /// <summary>
        /// Move set to new position
        /// </summary>
        public void MoveSet(Set set, int newPosition)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            if (!Sets.Contains(set))
                throw new InvalidOperationException("Set not found in exercise");

            if (newPosition < 1 || newPosition > Sets.Count)
                throw new ArgumentException("Invalid position");

            var orderedSets = Sets.OrderBy(s => s.SetNumber).ToList();
            var oldIndex = orderedSets.IndexOf(set);
            var newIndex = newPosition - 1;

            // Remove from old position
            orderedSets.RemoveAt(oldIndex);

            // Insert at new position
            orderedSets.Insert(newIndex, set);

            // Renumber all sets
            for (int i = 0; i < orderedSets.Count; i++)
            {
                orderedSets[i].UpdateSetNumber(i + 1);
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Get total volume for this exercise
        /// </summary>
        public decimal CalculateTotalVolume()
        {
            return Sets.Sum(s => s.CalculateVolume());
        }

        /// <summary>
        /// Get total reps for this exercise
        /// </summary>
        public int GetTotalReps()
        {
            return Sets.Sum(s => s.Reps);
        }

        /// <summary>
        /// Get total sets count
        /// </summary>
        public int GetTotalSets()
        {
            return Sets.Count;
        }

        /// <summary>
        /// Get max weight used
        /// </summary>
        public decimal GetMaxWeight()
        {
            if (!Sets.Any())
                return 0;

            return Sets.Max(s => s.Weight.ToKilograms());
        }

        /// <summary>
        /// Get working sets (exclude warmup)
        /// </summary>
        public IEnumerable<Set> GetWorkingSets()
        {
            return Sets.Where(s => s.SetType != SetType.WarmUp);
        }

        /// <summary>
        /// Get warmup sets
        /// </summary>
        public IEnumerable<Set> GetWarmupSets()
        {
            return Sets.Where(s => s.SetType == SetType.WarmUp);
        }

        /// <summary>
        /// Check if all sets are completed
        /// </summary>
        public bool AreAllSetsCompleted()
        {
            return Sets.Any() && Sets.All(s => s.IsCompleted);
        }

        /// <summary>
        /// Get completion percentage
        /// </summary>
        public decimal GetCompletionPercentage()
        {
            if (!Sets.Any())
                return 0;

            var completedSets = Sets.Count(s => s.IsCompleted);
            return (decimal)completedSets / Sets.Count * 100;
        }
    }
}
