using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Email)
            .WithEmailRules(localization);

        RuleFor(x => x.Password)
            .WithPasswordRules(localization);
    }
}
