using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .WithEmailRules();

        RuleFor(x => x.Password)
            .WithPasswordRules();
    }
}
