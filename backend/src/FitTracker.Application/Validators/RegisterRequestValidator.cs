using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Username)
            .WithUsernameRules(localization);

        RuleFor(x => x.Email)
            .WithEmailRules(localization);

        RuleFor(x => x.Password)
            .WithPasswordRules(localization);
    }
}