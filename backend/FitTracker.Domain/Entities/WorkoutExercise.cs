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
    /// Represents an exercise within a workout session.
    /// </summary>
    public class WorkoutExercise : BaseEntity
    {
        #region Constants

        public const int NotesMaxLength = 500;
        public const int MaxOrderIndex = 1000;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the workout this exercise belongs to.
        /// </summary>
        public Guid WorkoutId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the unique identifier of the exercise.
        /// </summary>
        public Guid ExerciseId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the order index of this exercise within the workout.
        /// </summary>
        public int OrderIndex
        {
            get; private set;
        }

        /// <summary>
        /// Gets any notes associated with this workout exercise.
        /// </summary>
        public string? Notes
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private WorkoutExercise()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new workout exercises.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        /// <param name="workoutId">The unique identifier of the workout.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <param name="orderIndex">The order index within the workout.</param>
        /// <param name="notes">Optional notes for this exercise.</param>
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

            EnsureValid();
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutExerciseValidator();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="WorkoutExerciseBuilder"/> instance.
        /// </summary>
        public static WorkoutExerciseBuilder CreateBuilder()
        {
            return new WorkoutExerciseBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="WorkoutExercise"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="WorkoutExercise"/> entity.
            /// </summary>
            public WorkoutExercise Build()
            {
                return new WorkoutExercise(_workoutId, _exerciseId, _orderIndex, _notes);
            }
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the order index of this workout exercise.
        /// </summary>
        /// <param name="newOrder">The new order index. Must be between 1 and <see cref="MaxOrderIndex"/>.</param>
        /// <exception cref="ArgumentException">Thrown if the new order is out of valid range.</exception>
        public void UpdateOrder(int newOrder)
        {
            if (newOrder < 1)
                throw new ArgumentException("Order index must be at least 1", nameof(newOrder));

            if (newOrder > MaxOrderIndex)
                throw new ArgumentException($"Order index cannot exceed {MaxOrderIndex}", nameof(newOrder));

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the notes for this workout exercise.
        /// </summary>
        /// <param name="notes">The new notes (optional).</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        #endregion
    }
}
