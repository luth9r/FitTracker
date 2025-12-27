using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class ResendVerificationRequestValidator : AbstractValidator<ResendVerificationRequest>
    {
        public ResendVerificationRequestValidator(ILocalizationService localization)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_ => localization.GetString("Validation.User.Email.Required"))
                .EmailAddress()
                .WithMessage(_ => localization.GetString("Validation.User.Email.InvalidFormat"));
        }
    }
}
