using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.Validators.CommonValidators;
using FluentValidation;

namespace FitTracker.Application.Validators
{
    public class ResetPasswordRequest : AbstractValidator<DTOs.Auth.ResetPasswordRequest>
    {
        public ResetPasswordRequest(ILocalizationService localization)
        {
            RuleFor(x => x.NewPassword)
                .WithPasswordRules(localization);
        }
    }
}
