using FitTracker.Application.Extensions;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.ResendVerificationEmail;

public class ResendVerificationRequestValidator : AbstractValidator<ResendVerificationEmailRequest>
{
    public ResendVerificationRequestValidator()
    {
        RuleFor(x => x.Email)
            .WithEmailRules();
    }
}
