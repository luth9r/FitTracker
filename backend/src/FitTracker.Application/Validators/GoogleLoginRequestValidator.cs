using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequest>
{
    public GoogleLoginRequestValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(_ => localization.GetString("Validation.Google.Code.Required"));

        RuleFor(x => x.CodeVerifier)
            .NotEmpty()
            .WithMessage(_ => localization.GetString("Validation.Google.CodeVerifier.Required"));
    }
}
