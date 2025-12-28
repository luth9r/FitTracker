using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators.CommonValidators
{
    /// <summary>
    /// Extension methods for FluentValidation rule builders providing common validation rules.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> WithPasswordRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            ILocalizationService localization)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Password.Required"))
                .MinimumLength(8)
                .WithMessage(_ => localization.GetString("Validation.User.Password.Length"))
                .Matches(@"[a-zA-Z]")
                .WithMessage(_ => localization.GetString("Validation.User.Password.LetterRequired"))
                .Matches(@"\d")
                .WithMessage(_ => localization.GetString("Validation.User.Password.NumberRequired"));
        }

        public static IRuleBuilderOptions<T, string> WithEmailRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            ILocalizationService localization)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Email.Required"))
                .EmailAddress()
                .WithMessage(_ => localization.GetString("Validation.User.Email.InvalidFormat"));
        }

        public static IRuleBuilderOptions<T, string> WithUsernameRules<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            ILocalizationService localization)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Username.Required"))
                .MinimumLength(3)
                .WithMessage(_ => localization.GetString("Validation.User.Username.Length"));
        }
    }
}
