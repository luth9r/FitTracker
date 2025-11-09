using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FluentValidation;

namespace FitTracker.Application.Validators
{
	public class LoginDtoValidator : AbstractValidator<LoginDto>
	{
		private readonly ILocalizationService _localization;
		public LoginDtoValidator(ILocalizationService localization)
		{
			_localization = localization;

			RuleFor(x => x.Email)
				.NotEmpty()
				.WithMessage(_localization.GetString("Auth.Login.EmailRequired"))
				.EmailAddress()
				.WithMessage(_localization.GetString("Auth.Login.InvalidEmail"));

			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage(_localization.GetString("Auth.Login.PasswordRequired"))
				.MinimumLength(6)
				.WithMessage(_localization.GetString("Auth.Login.PasswordTooShort"));
		}
	}
}
