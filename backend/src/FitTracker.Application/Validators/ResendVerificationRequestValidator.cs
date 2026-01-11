using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class ResendVerificationRequestValidator : AbstractValidator<ResendVerificationRequest>
{
    public ResendVerificationRequestValidator()
    {
        RuleFor(x => x.Email)
            .WithEmailRules();
    }
}
