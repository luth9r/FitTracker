using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        private readonly ILocalizationService _localization;

        public RegisterRequestValidator(ILocalizationService localization)
        {
            _localization = localization;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage(_ => _localization.GetString("Validation.User.Username.Required"))
                .MinimumLength(3)
                .WithMessage(_ => _localization.GetString("Validation.User.Username.Length"));

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_ => _localization.GetString("Validation.User.Email.Required"))
                .EmailAddress()
                .WithMessage(_ => _localization.GetString("Validation.User.Email.InvalidFormat"));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_ => _localization.GetString("Validation.User.Password.Required"))
                .MinimumLength(6)
                .WithMessage(_ => _localization.GetString("Validation.User.Password.Length"));
        }
    }
}
