using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using System.Security.Claims;

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class VerifyEmailCommandHandler(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<VerifyEmailCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var claimsPrincipal = jwtTokenGenerator.ValidateToken(request.Token);

            if (claimsPrincipal == null)
            {
                return Fail("Auth.VerifyEmail.InvalidToken");
            }

            var purposeClaim = claimsPrincipal.FindFirst("purpose");
            if (purposeClaim.Value != "email_verification")
            {
                return Fail("Auth.VerifyEmail.WrongPurposeToken");
            }


            var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString.Value, out var userId))
            {
                return Fail("Auth.VerifyEmail.InvalidToken");
            }

            var user = await userRepository.GetByIdReadonlyAsync(userId, cancellationToken);
            if (user == null)
            {
                return Fail("Auth.VerifyEmail.UserNotFound");
            }

            if (user.IsEmailVerified)
            {
                return Fail("Auth.VerifyEmail.AlreadyVerified");
            }
            user.SetEmailVerified();

            userRepository.Update(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }

        private Result<LoginResponse, ValidationResult> Fail(string errorKey)
        {
            var errorMessage = localization.GetString(errorKey);
            var error = new ValidationResult(new[] { new ValidationFailure("", errorMessage) });
            return Result.Failure<LoginResponse, ValidationResult>(error);
        }
    }
}
