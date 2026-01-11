using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .WithEmailRules();
    }
}
