using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(ILocalizationService localization)
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Username.Required"))
                .MinimumLength(3)
                .WithMessage(_ => localization.GetString("Validation.User.Username.Length"));

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Email.Required"))
                .EmailAddress()
                .WithMessage(_ => localization.GetString("Validation.User.Email.InvalidFormat"));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Password.Required"))
                .MinimumLength(8)
                .WithMessage(_ => localization.GetString("Validation.User.Password.Length"))
                .Matches(@"[a-zA-Z]")
                .WithMessage(_ => localization.GetString("Validation.User.Password.LetterRequired"))
                .Matches(@"\d")
                .WithMessage(_ => localization.GetString("Validation.User.Password.NumberRequired"));
        }
    }
}
