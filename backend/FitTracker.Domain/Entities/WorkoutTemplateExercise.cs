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
    /// Exercise in workout template
    /// </summary>
    public class WorkoutTemplateExercise : BaseEntity
    {
        public const int NotesMaxLength = 500;

        public Guid WorkoutTemplateId { get; private set; }
        public Guid ExerciseId { get; private set; }
        public int OrderIndex { get; private set; }
        public string? Notes { get; private set; }

        // Navigation
        public WorkoutTemplate? WorkoutTemplate { get; private set; }
        public Exercise? Exercise { get; private set; }
        public ICollection<TemplateSet> PlannedSets { get; private set; }

        private WorkoutTemplateExercise()
        {
            PlannedSets = new HashSet<TemplateSet>();
        }

        public WorkoutTemplateExercise(
            Guid workoutTemplateId,
            Guid exerciseId,
            int orderIndex,
            string? notes = null)
        {
            if (workoutTemplateId == Guid.Empty)
                throw new ArgumentException("Template ID cannot be empty");

            if (exerciseId == Guid.Empty)
                throw new ArgumentException("Exercise ID cannot be empty");

            if (orderIndex < 1)
                throw new ArgumentException("Order index must be at least 1");

            WorkoutTemplateId = workoutTemplateId;
            ExerciseId = exerciseId;
            OrderIndex = orderIndex;
            Notes = notes;
            PlannedSets = new HashSet<TemplateSet>();

            EnsureValid();
        }

        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateExerciseValidator();
        }

        public static WorkoutTemplateExerciseBuilder CreateBuilder()
            => new WorkoutTemplateExerciseBuilder();

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

            public WorkoutTemplateExercise Build()
            {
                return new WorkoutTemplateExercise(_templateId, _exerciseId, _orderIndex, _notes);
            }
        }

        public void AddPlannedSet(TemplateSet set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            PlannedSets.Add(set);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovePlannedSet(TemplateSet set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            PlannedSets.Remove(set);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateOrder(int newOrder)
        {
            if (newOrder < 1)
                throw new ArgumentException("Order must be at least 1");

            OrderIndex = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
