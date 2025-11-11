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

        /// <summary>
        /// Gets the unique identifier of the user who owns this template.
        /// </summary>
        public Guid UserId
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the workout template.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the optional description of the workout template.
        /// </summary>
        public string? Description
        {
            get; private set;
        }

        /// <summary>
        /// Gets the number of times this template has been used.
        /// </summary>
        public int UsageCount
        {
            get; private set;
        }

        /// <summary>
        /// Gets the date and time when this template was last used.
        /// </summary>
        public DateTime? LastUsedAt
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private WorkoutTemplate()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new workout templates.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        /// <param name="userId">The unique identifier of the template's owner.</param>
        /// <param name="name">The name of the workout template.</param>
        /// <param name="description">Optional description of the workout template.</param>
        private WorkoutTemplate(Guid userId, string name, string? description = null) : base()
        {
            UserId = userId;
            Name = name;
            Description = description;
            UsageCount = 0;

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring workout template from persistence layer.
        /// Use <see cref="WorkoutTemplateBuilder"/> for creating new templates.
        /// </summary>
        public WorkoutTemplate(Guid userId, string name, string? description, int usageCount, DateTime? lastUsedAt) : this(userId, name, description)
        {
            UsageCount = usageCount;
            LastUsedAt = lastUsedAt;

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new WorkoutTemplateValidator();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="WorkoutTemplateBuilder"/> instance.
        /// </summary>
        public static WorkoutTemplateBuilder CreateBuilder()
        {
            return new WorkoutTemplateBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="WorkoutTemplate"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="WorkoutTemplate"/> entity.
            /// </summary>
            public WorkoutTemplate Build()
            {
                return new WorkoutTemplate(_userId, _name, _description);
            }
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the workout template details.
        /// </summary>
        /// <param name="name">The new name for the template.</param>
        /// <param name="description">The new description of the template (optional).</param>
        /// <exception cref="ArgumentException">Thrown when the name is null or whitespace.</exception>
        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Template name cannot be empty", nameof(name));

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Records usage of this workout template, updating usage count and last used time.
        /// </summary>
        public void RecordUsage()
        {
            UsageCount++;
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
