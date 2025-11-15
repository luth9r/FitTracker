using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using System.Security.Claims;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

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
                return ResultExtensions.ValidationFailure<LoginResponse>("", "Auth.VerifyEmail.InvalidToken");
            }

            var purposeClaim = claimsPrincipal.FindFirst("purpose");
            if (purposeClaim.Value != "email_verification")
            {
                return ResultExtensions.ValidationFailure<LoginResponse>("", "Auth.VerifyEmail.WrongPurposeToken");
            }


            var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString.Value, out var userId))
            {
                return ResultExtensions.ValidationFailure<LoginResponse>("", "Auth.VerifyEmail.InvalidToken");
            }

            var user = await userRepository.GetByIdReadonlyAsync(userId, cancellationToken);
            if (user == null)
            {
                return ResultExtensions.ValidationFailure<LoginResponse>("", "Auth.VerifyEmail.UserNotFound");
            }

            if (user.IsEmailVerified)
            {
                return ResultExtensions.ValidationFailure<LoginResponse>("", "Auth.VerifyEmail.AlreadyVerified");
            }
            user.SetEmailVerified();

            userRepository.Update(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
