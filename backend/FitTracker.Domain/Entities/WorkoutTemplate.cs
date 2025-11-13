using CSharpFunctionalExtensions;
using FitTracker.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Template for creating workouts.
    /// </summary>
    public class WorkoutTemplate : BaseEntity
    {
        #region Constants

        public const int NameMaxLength = 100;
        public const int NameMinLength = 3;
        public const int DescriptionMaxLength = 1000;

        #endregion

        #region Properties

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int UsageCount { get; private set; }
        public DateTime? LastUsedAt { get; private set; }

        #endregion

        #region Constructors

        private WorkoutTemplate()
        {
            // For ORM
        }

        private WorkoutTemplate(Guid userId, string name, string? description = null) : base()
        {
            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;
        }

        public WorkoutTemplate(Guid userId, string name, string? description, int usageCount, DateTime? lastUsedAt)
            : this(userId, name, description)
        {
            UsageCount = usageCount;
            LastUsedAt = lastUsedAt;
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateValidator();
        }

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<WorkoutTemplate>(this));
        }

        private Result<WorkoutTemplate, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<WorkoutTemplate, ValidationResult>(result);

            return Result.Success<WorkoutTemplate, ValidationResult>(this);
        }

        #endregion

        #region Factory

        public static Result<WorkoutTemplate, ValidationResult> Create(Guid userId, string name, string? description = null)
        {
            var template = new WorkoutTemplate(userId, name, description);
            return template.ValidateWithResult();
        }

        #endregion

        #region Domain Methods

        public Result<WorkoutTemplate, ValidationResult> Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Template name cannot be empty", nameof(name));

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            return ValidateWithResult();
        }

        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Builder

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

            public Result<WorkoutTemplate, ValidationResult> Build()
            {
                var template = new WorkoutTemplate(_userId, _name, _description);
                return template.ValidateWithResult();
            }
        }

        #endregion
    }
}
