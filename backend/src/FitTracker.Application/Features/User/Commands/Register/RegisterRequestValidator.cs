using FitTracker.Application.Extensions;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.Register;

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
