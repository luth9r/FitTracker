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

        // ============================================
        // Constructors
        // ============================================

        private WorkoutTemplate(Guid userId, string name, string? description = null) : base()
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Template name cannot be empty");

            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;

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
                return new WorkoutTemplate(_userId, _name, _description);
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
    }
}
