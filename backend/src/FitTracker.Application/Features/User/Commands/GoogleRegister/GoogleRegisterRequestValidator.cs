using FitTracker.Application.Constants;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.GoogleRegister;

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
