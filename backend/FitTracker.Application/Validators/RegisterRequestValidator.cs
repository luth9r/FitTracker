using FitTracker.Application.DTOs.Auth;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Validation.User.Username.Required")
                .MinimumLength(3)
                .WithMessage("Validation.User.Username.Length");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Validation.User.Email.Required")
                .EmailAddress()
                .WithMessage("Validation.User.Email.InvalidFormat");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Validation.User.Password.Required")
                .MinimumLength(6)
                .WithMessage("Validation.User.Password.Length");
        }
    }
}
