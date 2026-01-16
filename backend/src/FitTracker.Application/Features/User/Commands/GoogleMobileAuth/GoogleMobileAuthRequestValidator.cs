using FitTracker.Application.Constants;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.GoogleMobileAuth;

public class GoogleMobileAuthRequestValidator : AbstractValidator<GoogleMobileAuthRequest>
{
    public GoogleMobileAuthRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(ValidationKeys.Google.CodeRequired);
    }
}
