using FitTracker.Application.Constants;
using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class GoogleRegisterRequestValidator : AbstractValidator<GoogleRegisterRequest>
{
    public GoogleRegisterRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(ValidationKeys.Google.CodeRequired);

        RuleFor(x => x.CodeVerifier)
            .NotEmpty()
            .WithMessage(ValidationKeys.Google.CodeVerifierRequired);
    }
}
