using FitTracker.Application.Extensions;
using FluentValidation;

namespace FitTracker.Application.Features.User.Commands.Login;

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
