using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequest>
    {
        private readonly ILocalizationService _localization;

        public GoogleLoginRequestValidator(ILocalizationService localization)
        {
            _localization = localization;

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(_ => _localization.GetString("Validation.Google.Code.Required"));

            RuleFor(x => x.CodeVerifier)
                .NotEmpty()
                .WithMessage(_ => _localization.GetString("Validation.Google.CodeVerifier.Required"));
        }
    }
}
