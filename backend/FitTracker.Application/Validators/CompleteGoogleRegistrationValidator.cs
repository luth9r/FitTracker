using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Validators
{
    public class CompleteGoogleRegistrationValidator : AbstractValidator<CompleteGoogleRegistrationCommand>
    {
        public CompleteGoogleRegistrationValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Request.IdToken).NotEmpty().WithMessage("IdToken is required.");

            RuleFor(x => x.Request.UserName)
                .MinimumLength(3)
                .WithMessage("Validation.User.Username.Length")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Validation.User.Username.InvalidCharacters");
        }
    }
}
