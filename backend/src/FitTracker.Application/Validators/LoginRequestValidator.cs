using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(ILocalizationService localization)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Email.Required"))
                .EmailAddress()
                .WithMessage(_ => localization.GetString("Validation.User.Email.InvalidFormat"));

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Password.Required"))
                .MinimumLength(6)
                .WithMessage(_ => localization.GetString("Validation.User.Password.Length"));
        }
    }
}
