using FitTracker.Application.Validators.Extensions;
using FluentValidation;

namespace FitTracker.Application.Validators;

public class ResetPasswordRequest : AbstractValidator<DTOs.Auth.ResetPasswordRequest>
{
    public ResetPasswordRequest()
    {
        RuleFor(x => x.NewPassword)
            .WithPasswordRules();
    }
}
