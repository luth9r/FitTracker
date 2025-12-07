using FitTracker.Application.DTOs.Auth.Google;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class GoogleRegisterRequestValidator : AbstractValidator<GoogleRegisterRequest>
    {
        public GoogleRegisterRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Validation.Google.Code.Required");

            RuleFor(x => x.CodeVerifier)
                .NotEmpty()
                .WithMessage("Validation.Google.CodeVerifier.Required");
        }
    }
}
