using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly ILocalizationService _localization;

        public LoginRequestValidator(ILocalizationService localization)
        {
            _localization = localization;

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
