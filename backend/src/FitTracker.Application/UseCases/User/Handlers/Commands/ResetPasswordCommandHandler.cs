using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Commands;

/// <summary>
///     Handler for processing password reset commands.
/// </summary>
/// <param name="userReadRepository">The user read repository.</param>
/// <param name="userWriteRepository">The user write repository.</param>
/// <param name="unitOfWork">The unit of work.</param>
/// <param name="jwtTokenValidator">The JWT token validator.</param>
/// <param name="hasher">The password hasher.</param>
/// <param name="logger">The logger.</param>
public sealed class ResetPasswordCommandHandler(
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IJwtTokenValidator jwtTokenValidator,
    IPasswordHasher hasher,
    ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommand, Result<Unit, ValidationResult>>
{
    public async Task<Result<Unit, ValidationResult>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = jwtTokenValidator.ValidatePurposeToken(request.Token, TokenPurposes.PasswordReset);

        if (validationResult.IsFailure)
        {
            logger.LogWarning("Password reset failed: {Error}", validationResult.Error);
            return ResultExtensions.ValidationFailure<Unit>(string.Empty, DomainErrors.Auth.InvalidToken);
        }

        var userId = validationResult.Value;
        var user = await userReadRepository.GetByIdReadonlyAsync(userId, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("User not found for email verification. UserId: {UserId}", userId);
            return ResultExtensions.ValidationFailure<Unit>(string.Empty, DomainErrors.User.NotFound);
        }

        var passwordHash = hasher.HashPassword(request.NewPassword);

        user.ChangePassword(passwordHash);

        userWriteRepository.Update(user);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<Unit, ValidationResult>(Unit.Value);
    }
}
