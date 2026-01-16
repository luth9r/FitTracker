using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Features.User.Common;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Features.User.Commands.GoogleMobileAuth;

/// <summary>
///     Handles the Google mobile authentication command by processing the provided OAuth
///     request and managing user authentication in the system.
/// </summary>
/// <param name="googleOAuthService">The Google OAuth service.</param>
/// <param name="userReadRepository">The user read repository.</param>
/// <param name="userWriteRepository">The user write repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
/// <param name="jwtTokenService">The JWT token service.</param>
/// <param name="mapper">The mapper.</param>
/// <param name="logger">The logger.</param>
public sealed class GoogleMobileAuthCommandHandler(
    IGoogleOAuthService googleOAuthService,
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenService,
    IMapper mapper,
    ILogger<GoogleMobileAuthCommandHandler> logger)
    : IRequestHandler<GoogleMobileAuthCommand, Result<LoginResponse, ValidationResult>>
{
    /// <summary>
    ///     Handles the Google Mobile Authentication command by exchanging the provided code for tokens,
    ///     validating the Google ID token, checking the user's existence in the system, and generating
    ///     a JWT for user authentication.
    /// </summary>
    /// <param name="request">The <see cref="GoogleMobileAuthCommand" /> command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" /> result.</returns>
    public async Task<Result<LoginResponse, ValidationResult>> Handle(
        GoogleMobileAuthCommand request,
        CancellationToken cancellationToken)
    {
        var tokenResponse = await googleOAuthService.ExchangeCodeForTokensAsync(request.Code);

        logger.LogDebug("Attempting to validate Google IdToken.");

        var googlePayload = await googleOAuthService.ValidateAsync(tokenResponse.IdToken);

        if (googlePayload == null)
        {
            logger.LogWarning("Google Token validation failed.");
            return ResultExtensions.ValidationFailure<LoginResponse>(
                nameof(request.Code),
                DomainErrors.Google.InvalidToken);
        }

        var user = await userReadRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken)
                   ?? await userReadRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken);

        if (user == null)
        {
            logger.LogInformation("Registering new Google user: {Email}", googlePayload.Email);

            user = UserEntity.CreateGoogleUser(
                googlePayload.Email,
                googlePayload.GoogleId,
                googlePayload.FirstName,
                googlePayload.LastName);

            await userWriteRepository.AddAsync(user, cancellationToken);
        }
        else if (string.IsNullOrEmpty(user.GoogleProviderId))
        {
            user.SetGoogleProviderId(googlePayload.GoogleId);
            userWriteRepository.Update(user);
        }

        _ = await unitOfWork.SaveChangesAsync(CancellationToken.None);

        var loginToken = jwtTokenService.GenerateToken(user);

        var response = mapper.Map<LoginResponse>(user) with
        {
            Jwt = loginToken,
        };

        return Result.Success<LoginResponse, ValidationResult>(response);
    }
}
