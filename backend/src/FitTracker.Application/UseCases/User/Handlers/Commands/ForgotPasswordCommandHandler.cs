using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.Events;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;

namespace FitTracker.Application.UseCases.User.Handlers.Commands
{
    public class ForgotPasswordCommandHandler(
        IUserReadRepository readRepository,
        IMediator mediator) : IRequestHandler<ForgotPasswordCommand, Result>
    {
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await readRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return Result.Success();
            }

            await mediator.Publish(new UserPasswordResetRequestedEvent(user.Id, request.Email, user.Username), cancellationToken);

            return Result.Success();
        }
    }
}
