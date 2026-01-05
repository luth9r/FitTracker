using CSharpFunctionalExtensions;
using FitTracker.Application.Events;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;

namespace FitTracker.Application.UseCases.User.Handlers.Commands;

/// <summary>
///     Handles the <see cref="ForgotPasswordCommand" /> to initiate password reset process.
///     Always returns success (<see cref="Result.Success()" />) regardless of user existence for security.
/// </summary>
/// <param name="readRepository">Repository for readonly user queries by email.</param>
/// <param name="mediator">MediatR instance for publishing domain events.</param>
public class ForgotPasswordCommandHandler(
    IUserReadRepository readRepository,
    IMediator mediator) : IRequestHandler<ForgotPasswordCommand, Result>
{
    /// <summary>
    ///     Processes the forgot password request asynchronously.
    /// </summary>
    /// <param name="request">Forgot password command containing user email.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>Success result. Publishes password reset event if user exists.</returns>
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await readRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result.Success();
        }

        await mediator.Publish(
            new UserPasswordResetRequestedEvent(user.Id, request.Email, user.Username),
            cancellationToken);

        return Result.Success();
    }
}
