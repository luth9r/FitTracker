using FitTracker.Application.Constants;
using FitTracker.Application.Extensions;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.ChangePassword;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .WithPasswordRules();

        RuleFor(x => x.NewPassword)
            .WithPasswordRules()
            .NotEqual(x => x.OldPassword)
            .WithMessage(ValidationKeys.User.Password.NotSameAsOld);
    }
}
