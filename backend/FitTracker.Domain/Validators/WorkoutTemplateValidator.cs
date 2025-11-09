using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
	internal class WorkoutTemplateValidator : AbstractValidator<WorkoutTemplate>
	{
		public WorkoutTemplateValidator()
		{
			#region Required Fields

			RuleFor(t => t.UserId)
				.NotEmpty()
				.WithMessage("User ID is required")
				.WithName("userId")
				.OverridePropertyName("userId");

			RuleFor(t => t.Name)
				.NotEmpty()
				.WithMessage("Template name is required")
				.WithName("name")
				.OverridePropertyName("name");

			#endregion

			// Detailed validations
			NameValidation();
			DescriptionValidation();
			UsageValidation();
		}

		private void NameValidation()
		{
			RuleFor(t => t.Name)
				.Length(WorkoutTemplate.NameMinLength, WorkoutTemplate.NameMaxLength)
				.WithMessage($"Name must be between {WorkoutTemplate.NameMinLength} and {WorkoutTemplate.NameMaxLength} characters")
				.WithName("name")
				.OverridePropertyName("name");
		}

		private void DescriptionValidation()
		{
			RuleFor(t => t.Description)
				.MaximumLength(WorkoutTemplate.DescriptionMaxLength)
				.When(t => !string.IsNullOrEmpty(t.Description))
				.WithMessage($"Description cannot exceed {WorkoutTemplate.DescriptionMaxLength} characters")
				.WithName("description")
				.OverridePropertyName("description");
		}

		private void UsageValidation()
		{
			RuleFor(t => t.UsageCount)
				.GreaterThanOrEqualTo(0)
				.WithMessage("Usage count cannot be negative")
				.WithName("usageCount")
				.OverridePropertyName("usageCount");

			// If usage count > 0, must have last used date
			RuleFor(t => t)
				.Must(t => t.UsageCount == 0 || t.LastUsedAt.HasValue)
				.WithMessage("Templates with usage count must have last used date")
				.WithName("lastUsedAt")
				.OverridePropertyName("lastUsedAt");
		}
	}
}

