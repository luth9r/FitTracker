using FitTracker.Application.Constants;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequest>
{
    public GoogleLoginRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(ValidationKeys.Google.CodeRequired);

        RuleFor(x => x.CodeVerifier)
            .NotEmpty()
            .WithMessage(ValidationKeys.Google.CodeVerifierRequired);
    }
}
