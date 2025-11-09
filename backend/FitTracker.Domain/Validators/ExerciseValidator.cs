using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
	internal class ExerciseValidator : AbstractValidator<Exercise>
	{
		public ExerciseValidator()
		{
			#region Required Fields

			RuleFor(e => e.Name)
				.NotEmpty()
				.WithMessage("Exercise name is required")
				.WithName("name")
				.OverridePropertyName("name");

			RuleFor(e => e.MuscleGroup)
				.NotEmpty()
				.WithMessage("Muscle group is required")
				.WithName("muscleGroup")
				.OverridePropertyName("muscleGroup");

			RuleFor(e => e.Equipment)
				.NotEmpty()
				.WithMessage("Equipment is required")
				.WithName("equipment")
				.OverridePropertyName("equipment");

			#endregion

			// Detailed validations
			NameValidation();
			DescriptionValidation();
			MuscleGroupValidation();
			EquipmentValidation();
			ImageUrlValidation();
			VideoUrlValidation();
			CustomExerciseValidation();
		}

		private void NameValidation()
		{
			RuleFor(e => e.Name)
				.Length(2, Exercise.NameMaxLength)
				.WithMessage($"Exercise name must be between 2 and {Exercise.NameMaxLength} characters")
				.WithName("name")
				.OverridePropertyName("name");
		}

		private void DescriptionValidation()
		{
			RuleFor(e => e.Description)
				.MaximumLength(Exercise.DescriptionMaxLength)
				.When(e => !string.IsNullOrEmpty(e.Description))
				.WithMessage($"Description cannot exceed {Exercise.DescriptionMaxLength} characters")
				.WithName("description")
				.OverridePropertyName("description");
		}

		private void MuscleGroupValidation()
		{
			RuleFor(e => e.MuscleGroup.ToString())
				.MaximumLength(Exercise.MuscleGroupMaxLength)
				.WithMessage($"Muscle group cannot exceed {Exercise.MuscleGroupMaxLength} characters")
				.WithName("muscleGroup")
				.OverridePropertyName("muscleGroup");
		}

		private void EquipmentValidation()
		{
			RuleFor(e => e.Equipment.ToString())
				.MaximumLength(Exercise.EquipmentMaxLength)
				.WithMessage($"Equipment cannot exceed {Exercise.EquipmentMaxLength} characters")
				.WithName("equipment")
				.OverridePropertyName("equipment");
		}

		private void ImageUrlValidation()
		{
			RuleFor(e => e.ImageUrl)
				.MaximumLength(Exercise.ImageUrlMaxLength)
				.When(e => !string.IsNullOrEmpty(e.ImageUrl))
				.WithMessage($"Image URL cannot exceed {Exercise.ImageUrlMaxLength} characters")
				.Must(BeValidUrl)
				.When(e => !string.IsNullOrEmpty(e.ImageUrl))
				.WithMessage("Image URL must be a valid URL")
				.WithName("imageUrl")
				.OverridePropertyName("imageUrl");
		}

		private void VideoUrlValidation()
		{
			RuleFor(e => e.VideoUrl)
				.MaximumLength(Exercise.VideoUrlMaxLength)
				.When(e => !string.IsNullOrEmpty(e.VideoUrl))
				.WithMessage($"Video URL cannot exceed {Exercise.VideoUrlMaxLength} characters")
				.Must(BeValidUrl)
				.When(e => !string.IsNullOrEmpty(e.VideoUrl))
				.WithMessage("Video URL must be a valid URL")
				.WithName("videoUrl")
				.OverridePropertyName("videoUrl");
		}

		private void CustomExerciseValidation()
		{
			// If custom, must have UserId
			RuleFor(e => e)
				.Must(e => !e.IsCustom || e.UserId.HasValue)
				.WithMessage("Custom exercises must have a user ID")
				.WithName("userId")
				.OverridePropertyName("userId");

			// If not custom, UserId must be null
			RuleFor(e => e)
				.Must(e => e.IsCustom || !e.UserId.HasValue)
				.WithMessage("Standard exercises cannot have a user ID")
				.WithName("userId")
				.OverridePropertyName("userId");
		}

		private bool BeValidUrl(string? url)
		{
			if (string.IsNullOrEmpty(url))
				return true;

			return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
		}
	}
}
