using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Application.Features.User.Common;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.Features.User.Commands.Login;

/// <summary>
///     Handler for processing login commands.
/// </summary>
/// <param name="userReadRepository">The <see cref="IUserReadRepository" />.</param>
/// <param name="mapper">The <see cref="IMapper" />.</param>
/// <param name="hasher">The <see cref="IPasswordHasher" />.</param>
/// <param name="jwtTokenService">The <see cref="IJwtTokenGenerator" />.</param>
/// <param name="logger">The <see cref="ILogger{LoginCommandHandler}" />.</param>
public sealed class LoginCommandHandler(
    IUserReadRepository userReadRepository,
    IMapper mapper,
    IPasswordHasher hasher,
    IJwtTokenGenerator jwtTokenService,
    ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, Result<LoginResponse, ValidationResult>>
{
    /// <summary>
    ///     Handles the login command.
    /// </summary>
    /// <param name="request">The <see cref="LoginCommand" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The <see cref="LoginResponse" /> result.</returns>
    public async Task<Result<LoginResponse, ValidationResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting login process for email: {Email}", request.Email);

        var user = await userReadRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("Login failed for email: {Email}. User not found or email not verified.", request.Email);
            return ResultExtensions.ValidationFailure<LoginResponse>(
                ErrorKeys.General,
                DomainErrors.Auth.InvalidCredentials);
        }

        var checkPassword = hasher.VerifyPassword(
            request.Password,
            user.PasswordHash ?? string.Empty); // If PasswordHash is null (e.g., social login), treat as invalid

        if (!checkPassword)
        {
            logger.LogWarning("Login failed for email: {Email}. Invalid password.", request.Email);
            return ResultExtensions.ValidationFailure<LoginResponse>(
                ErrorKeys.General,
                DomainErrors.Auth.InvalidCredentials);
        }

        if (!user.IsEmailVerified)
        {
            return ResultExtensions.ValidationFailure<LoginResponse>(
                ErrorKeys.Email,
                DomainErrors.User.EmailNotVerified);
        }

        var loginToken = jwtTokenService.GenerateToken(user);

        var response = mapper.Map<LoginResponse>(user) with
        {
            Jwt = loginToken,
        };

        logger.LogInformation("Login process completed successfully for email: {Email}", request.Email);

        return Result.Success<LoginResponse, ValidationResult>(response);
    }
}
