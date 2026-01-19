using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.Features.User.Commands.ChangePassword;

/// <summary>
///     Handles the operation to change a user's password in the system.
/// </summary>
/// <param name="readRepository">
///     A repository for reading user data from the data store.
/// </param>
/// <param name="writeRepository">
///     A repository for updating user data in the data store.
/// </param>
/// <param name="hasher">
///     A utility for hashing and verifying passwords.
/// </param>
/// <param name="unitOfWork">
///     A unit of work pattern implementation to commit changes as a single transaction.
/// </param>
public sealed class ChangePasswordCommandHandler(
    IUserReadRepository readRepository,
    IUserWriteRepository writeRepository,
    IPasswordHasher hasher,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Result<Unit, ValidationResult>>
{
    public async Task<Result<Unit, ValidationResult>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await readRepository.GetByIdReadonlyAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return ResultExtensions.ValidationFailure<Unit>(string.Empty, DomainErrors.User.NotFound);
        }

        if (!user.IsEmailVerified)
        {
            return ResultExtensions.ValidationFailure<Unit>(string.Empty, DomainErrors.User.EmailNotVerified);
        }

        var verifyPassword = hasher.VerifyPassword(request.OldPassword, user.PasswordHash ?? string.Empty);

        if (!verifyPassword)
        {
            return ResultExtensions.ValidationFailure<Unit>(string.Empty, DomainErrors.User.InvalidPassword);
        }

        var newPasswordHash = hasher.HashPassword(request.NewPassword);

        user.UpdatePasswordHash(newPasswordHash);

        writeRepository.Update(user);

        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        return Result.Success<Unit, ValidationResult>(Unit.Value);
    }
}
