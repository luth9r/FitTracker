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
    /// Template for creating workouts
    /// </summary>
    public class WorkoutTemplate : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int DescriptionMaxLength = 1000;

        // ============================================
        // Properties
        // ============================================
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int UsageCount { get; private set; }
        public DateTime? LastUsedAt { get; private set; }

        // Navigation
        public User? User { get; private set; }
        public ICollection<WorkoutTemplateExercise> Exercises { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        private WorkoutTemplate()
        {
            Name = string.Empty;
            Exercises = new HashSet<WorkoutTemplateExercise>();
        }

        private WorkoutTemplate(Guid userId, string name, string? description = null)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Template name cannot be empty");

            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;
            Exercises = new HashSet<WorkoutTemplateExercise>();

            EnsureValid();
        }

        public WorkoutTemplate(Guid userId, string name, string? description, int usageCount, DateTime? lastUsedAt) : this(userId, name, description)
        {
            UsageCount = usageCount;
            LastUsedAt = lastUsedAt;
        }



        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateValidator();
        }

        // ============================================
        // Builder
        // ============================================

        public static WorkoutTemplateBuilder CreateBuilder() => new WorkoutTemplateBuilder();

        public class WorkoutTemplateBuilder
        {
            private Guid _userId;
            private string _name = string.Empty;
            private string? _description;

            public WorkoutTemplateBuilder ForUser(Guid userId)
            {
                _userId = userId;
                return this;
            }

            public WorkoutTemplateBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public WorkoutTemplateBuilder WithDescription(string? description)
            {
                _description = description;
                return this;
            }


            public WorkoutTemplate Build()
            {
                var template = new WorkoutTemplate(_userId, _name, _description)
                {
                };
                return template;
            }
        }

        // ============================================
        // Domain Methods
        // ============================================

        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Template name cannot be empty");

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }


        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddExercise(WorkoutTemplateExercise exercise)
        {
            if (exercise == null)
                throw new ArgumentNullException(nameof(exercise));

            Exercises.Add(exercise);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveExercise(WorkoutTemplateExercise exercise)
        {
            if (exercise == null)
                throw new ArgumentNullException(nameof(exercise));

            Exercises.Remove(exercise);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Create workout from this template
        /// </summary>
        public Workout CreateWorkout()
        {
            var workout = Workout.CreateBuilder()
                .ForUser(UserId)
                .WithName(Name)
                .OnDate(DateTime.UtcNow)
                .FromTemplate(Id)
                .WithNotes($"Created from template: {Name}")
                .Build();

            // Copy exercises from template
            foreach (var templateExercise in Exercises.OrderBy(e => e.OrderIndex))
            {
                var workoutExercise = WorkoutExercise.CreateBuilder()
                    .WithWorkout(workout.Id)
                    .WithExercise(templateExercise.ExerciseId)
                    .WithOrder(templateExercise.OrderIndex)
                    .WithNotes(templateExercise.Notes)
                    .Build();

                // Copy planned sets
                foreach (var templateSet in templateExercise.PlannedSets.OrderBy(s => s.SetNumber))
                {
                    var set = Set.CreateBuilder()
                        .WithWorkoutExercise(workoutExercise.Id)
                        .WithSetNumber(templateSet.SetNumber)
                        .WithWeightKg(templateSet.PlannedWeight.ValueInKg)
                        .WithReps(templateSet.PlannedReps)
                        .WithRest(templateSet.RestSeconds ?? 0)
                        .WithSetType(templateSet.SetType)
                        .Build();

                    workoutExercise.AddSet(set);
                }

                workout.AddExercise(workoutExercise);
            }

            RecordUsage();
            return workout;
        }

        public int GetTotalExercises() => Exercises.Count;

        public int GetTotalPlannedSets() => Exercises.Sum(e => e.PlannedSets.Count);
    }
}
