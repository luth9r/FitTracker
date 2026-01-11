using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .WithUsernameRules();

        RuleFor(x => x.Email)
            .WithEmailRules();

        RuleFor(x => x.Password)
            .WithPasswordRules();
    }
}
