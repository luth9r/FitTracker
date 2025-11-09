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

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// Domain constructor
        /// </summary>
        public WorkoutExercise(
            Guid workoutId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null) : base()
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
    }
}
