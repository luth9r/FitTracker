// FitTracker.Domain/Entities/Validators/UserValidator.cs
using System;
using FluentValidation;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Validators
{
    internal class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            #region Username

            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .WithName("username")
                .OverridePropertyName("username");

            #endregion

            #region Email

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .WithName("email")
                .OverridePropertyName("email");

            #endregion

            #region PasswordHash

            RuleFor(u => u.PasswordHash)
                .NotEmpty()
                .WithMessage("Password hash is required")
                .WithName("passwordHash")
                .OverridePropertyName("passwordHash");

            #endregion

            #region PreferredUnits

            RuleFor(u => u.PreferredUnits)
                .NotNull()
                .WithMessage("Preferred units are required")
                .WithName("preferredUnits")
                .OverridePropertyName("preferredUnits");

            #endregion

            // Detailed validations
            UsernameValidation();
            EmailValidation();
            FirstNameValidation();
            LastNameValidation();
            BioValidation();
            AvatarValidation();
            PreferredUnitsValidation();
        }

        private void UsernameValidation()
        {
            RuleFor(u => u.Username)
                .Length(3, User.UsernameMaxLength)
                .WithMessage($"Username must be between 3 and {User.UsernameMaxLength} characters")
                .Matches(@"^[a-zA-Z0-9_-]+$")
                .WithMessage("Username can only contain letters, numbers, underscores and hyphens")
                .WithName("username")
                .OverridePropertyName("username");
        }

        private void EmailValidation()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid email address")
                .MaximumLength(User.EmailMaxLength)
                .WithMessage($"Email cannot exceed {User.EmailMaxLength} characters")
                .Matches("^(?=.{1,254}$)(?=.{1,64}@)[a-zA-Z0-9!#$%&'*+/=?^_{|}~-]+"
                         + @"(?:\.[a-zA-Z0-9!#$%&'*+/=?^_{|}~-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}"
                         + @"[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")
                .WithMessage("Email must be a valid RFC 5322 email address")
                .WithName("email")
                .OverridePropertyName("email");
        }

        private void FirstNameValidation()
        {
            RuleFor(u => u.FirstName)
                .MaximumLength(User.FirstNameMaxLength)
                .When(u => !string.IsNullOrEmpty(u.FirstName))
                .WithMessage($"First name cannot exceed {User.FirstNameMaxLength} characters")
                .Matches(@"^[a-zA-Z\s\-'\.]+$")
                .When(u => !string.IsNullOrEmpty(u.FirstName))
                .WithMessage("First name can only contain letters, spaces, hyphens, apostrophes and periods")
                .WithName("firstName")
                .OverridePropertyName("firstName");
        }

        private void LastNameValidation()
        {
            RuleFor(u => u.LastName)
                .MaximumLength(User.LastNameMaxLength)
                .When(u => !string.IsNullOrEmpty(u.LastName))
                .WithMessage($"Last name cannot exceed {User.LastNameMaxLength} characters")
                .Matches(@"^[a-zA-Z\s\-'\.]+$")
                .When(u => !string.IsNullOrEmpty(u.LastName))
                .WithMessage("Last name can only contain letters, spaces, hyphens, apostrophes and periods")
                .WithName("lastName")
                .OverridePropertyName("lastName");
        }

        private void BioValidation()
        {
            RuleFor(u => u.Bio)
                .MaximumLength(User.BioMaxLength)
                .When(u => !string.IsNullOrEmpty(u.Bio))
                .WithMessage($"Bio cannot exceed {User.BioMaxLength} characters")
                .WithName("bio")
                .OverridePropertyName("bio");
        }

        private void AvatarValidation()
        {
            RuleFor(u => u.Avatar)
                .MaximumLength(500)
                .When(u => !string.IsNullOrEmpty(u.Avatar))
                .WithMessage("Avatar URL cannot exceed 500 characters")
                .Must(BeValidUrl)
                .When(u => !string.IsNullOrEmpty(u.Avatar))
                .WithMessage("Avatar must be a valid URL")
                .WithName("avatar")
                .OverridePropertyName("avatar");
        }

        private void PreferredUnitsValidation()
        {
            RuleFor(u => u.PreferredUnits)
                .Must(units => units == UnitSystem.Metric || units == UnitSystem.Imperial)
                .WithMessage("Preferred units must be Metric or Imperial")
                .WithName("preferredUnits")
                .OverridePropertyName("preferredUnits");

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
