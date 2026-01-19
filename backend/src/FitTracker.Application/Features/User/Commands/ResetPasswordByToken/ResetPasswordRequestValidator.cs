using FitTracker.Application.Extensions;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.ResetPasswordByToken;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.NewPassword)
            .WithPasswordRules();
    }
}
