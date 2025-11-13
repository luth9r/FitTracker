// FitTracker.Domain/Entities/Validators/UserValidator.cs
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FluentValidation;

namespace FitTracker.Domain.Validators
{
    internal class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            Include(new BaseEntityValidator<User>());

            #region Username

            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Validation.User.Username.Required")
                .WithName("username")
                .OverridePropertyName("username");

            #endregion

            #region Email

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Validation.User.Email.Required")
                .WithName("email")
                .OverridePropertyName("email");

            #endregion

            #region PasswordHash

            RuleFor(u => u.PasswordHash)
                .NotEmpty()
                .WithMessage("Validation.User.PasswordHash.Required")
                .WithName("passwordHash")
                .OverridePropertyName("passwordHash");

            #endregion

            #region PreferredUnits

            RuleFor(u => u.PreferredUnits)
                .NotNull()
                .WithMessage("Validation.User.PreferredUnits.Required")
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
                .WithMessage("Validation.User.Username.Length")
                .Matches(@"^[a-zA-Z0-9_-]+$")
                .WithMessage("Validation.User.Username.InvalidCharacters")
                .WithName("username")
                .OverridePropertyName("username");
        }

        private void EmailValidation()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Validation.User.Email.InvalidFormat")
                .MaximumLength(User.EmailMaxLength)
                .WithMessage($"Email cannot exceed {User.EmailMaxLength} characters")
                .Matches("^(?=.{1,254}$)(?=.{1,64}@)[a-zA-Z0-9!#$%&'*+/=?^_{|}~-]+"
                         + @"(?:\.[a-zA-Z0-9!#$%&'*+/=?^_{|}~-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}"
                         + @"[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")
                .WithMessage("Validation.User.Email.InvalidRFC5322")
                .WithName("email")
                .OverridePropertyName("email");
        }

        private void FirstNameValidation()
        {
            RuleFor(u => u.FirstName)
                .MaximumLength(User.FirstNameMaxLength)
                .When(u => !string.IsNullOrEmpty(u.FirstName))
                .WithMessage("Validation.User.FirstName.MaxLength")
                .Matches(@"^[a-zA-Z\s\-'\.]+$")
                .When(u => !string.IsNullOrEmpty(u.FirstName))
                .WithMessage("Validation.User.FirstName.InvalidCharacters")
                .WithName("firstName")
                .OverridePropertyName("firstName");
        }

        private void LastNameValidation()
        {
            RuleFor(u => u.LastName)
                .MaximumLength(User.LastNameMaxLength)
                .When(u => !string.IsNullOrEmpty(u.LastName))
                .WithMessage($"Validation.User.LastName.MaxLength")
                .Matches(@"^[a-zA-Z\s\-'\.]+$")
                .When(u => !string.IsNullOrEmpty(u.LastName))
                .WithMessage("Validation.User.LastName.InvalidCharacters")
                .WithName("lastName")
                .OverridePropertyName("lastName");
        }

        private void BioValidation()
        {
            RuleFor(u => u.Bio)
                .MaximumLength(User.BioMaxLength)
                .When(u => !string.IsNullOrEmpty(u.Bio))
                .WithMessage("Validation.User.Bio.MaxLength")
                .WithName("bio")
                .OverridePropertyName("bio");
        }

        private void AvatarValidation()
        {
            RuleFor(u => u.Avatar)
                .MaximumLength(500)
                .When(u => !string.IsNullOrEmpty(u.Avatar))
                .WithMessage("Validation.User.Avatar.MaxLength")
                .Must(BeValidUrl)
                .When(u => !string.IsNullOrEmpty(u.Avatar))
                .WithMessage("Validation.User.Avatar.InvalidUrl")
                .WithName("avatar")
                .OverridePropertyName("avatar");
        }

        private void PreferredUnitsValidation()
        {
            RuleFor(u => u.PreferredUnits)
                .Must(units => units == UnitSystem.Metric || units == UnitSystem.Imperial)
                .WithMessage("Validation.User.PreferredUnits.Invalid")
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
