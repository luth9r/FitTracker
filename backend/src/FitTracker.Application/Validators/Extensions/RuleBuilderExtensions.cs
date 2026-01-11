using FitTracker.Application.Constants;
using FluentValidation;

namespace FitTracker.Application.Validators.Extensions;

/// <summary>
///     Extension methods for FluentValidation rule builders providing common validation rules.
/// </summary>
public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> WithPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ValidationKeys.User.Password.Required)
            .MinimumLength(8)
            .WithMessage(ValidationKeys.User.Password.Length)
            .Matches(@"[a-zA-Z]")
            .WithMessage(ValidationKeys.User.Password.LetterRequired)
            .Matches(@"\d")
            .WithMessage(ValidationKeys.User.Password.NumberRequired);
    }

    public static IRuleBuilderOptions<T, string> WithEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ValidationKeys.User.Email.Required)
            .EmailAddress()
            .WithMessage(ValidationKeys.User.Email.InvalidFormat);
    }

    public static IRuleBuilderOptions<T, string> WithUsernameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(ValidationKeys.User.Username.Required)
            .MinimumLength(3)
            .WithMessage(ValidationKeys.User.Username.Length);
    }
}
