using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents an exercise within a workout template.
    /// </summary>
    public class WorkoutTemplateExercise : BaseEntity
    {
        #region Constants

        public const int NotesMaxLength = 500;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the workout template this exercise belongs to.
        /// </summary>
        public Guid WorkoutTemplateId
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
        /// Gets the order index of this exercise within the workout template.
        /// </summary>
        public int OrderIndex
        {
            get; private set;
        }

        /// <summary>
        /// Gets any optional notes associated with this workout template exercise.
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
        private WorkoutTemplateExercise()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new workout template exercises.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        /// <param name="workoutTemplateId">The unique identifier of the workout template.</param>
        /// <param name="exerciseId">The unique identifier of the exercise.</param>
        /// <param name="orderIndex">The order index of the exercise.</param>
        /// <param name="notes">Optional notes about the exercise.</param>
        public WorkoutTemplateExercise(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null) : base()
        {
            WorkoutTemplateId = workoutTemplateId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;

            EnsureValid();
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateExerciseValidator();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="WorkoutTemplateExerciseBuilder"/> instance.
        /// </summary>
        public static WorkoutTemplateExerciseBuilder CreateBuilder()
        {
            return new WorkoutTemplateExerciseBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="WorkoutTemplateExercise"/> instances.
        /// </summary>
        public class WorkoutTemplateExerciseBuilder
        {
            private Guid _templateId;
            private Guid _exerciseId;
            private int _orderIndex = 1;
            private string? _notes;

            public WorkoutTemplateExerciseBuilder WithTemplate(Guid templateId)
            {
                _templateId = templateId;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithExercise(Guid exerciseId)
            {
                _exerciseId = exerciseId;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithOrder(int orderIndex)
            {
                _orderIndex = orderIndex;
                return this;
            }

            public WorkoutTemplateExerciseBuilder WithNotes(string? notes)
            {
                _notes = notes;
                return this;
            }

            /// <summary>
            /// Builds the <see cref="WorkoutTemplateExercise"/> entity.
            /// </summary>
            public WorkoutTemplateExercise Build()
            {
                return new WorkoutTemplateExercise(_templateId, _exerciseId, _orderIndex, _notes);
            }
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the order index of this workout template exercise.
        /// </summary>
        /// <param name="newOrder">The new order index; must be at least 1.</param>
        /// <exception cref="ArgumentException">Thrown if the new order is less than 1.</exception>
        public void UpdateOrder(int newOrder)
        {
            if (newOrder < 1)
                throw new ArgumentException("Order must be at least 1", nameof(newOrder));

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the notes for this workout template exercise.
        /// </summary>
        /// <param name="notes">The new notes; can be null.</param>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
