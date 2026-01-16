using FitTracker.Application.Constants;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.GoogleLogin;

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
