using System;
using System.Collections.Generic;
using System.Text;
using CSharpFunctionalExtensions;
using FitTracker.Application.Events;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Queries;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Queries
{
    /// <summary>
    /// Handler for processing resend verification email queries.
    /// </summary>
    /// <param name="userReadRepository">The <see cref="IUserReadRepository"/>.</param>
    /// <param name="mediator">The <see cref="IMediator"/>.</param>
    /// <param name="logger">The <see cref="ILogger{ResendVerificationEmailQueryHandler}"/>.</param>
    /// <param name="localization">The <see cref="ILocalizationService"/>.</param>
    public sealed class ResendVerificationEmailQueryHandler(
        IUserReadRepository userReadRepository,
        IMediator mediator,
        ILogger<ResendVerificationEmailQueryHandler> logger,
        ILocalizationService localization)
        : IRequestHandler<ResendVerificationEmailQuery, Result<Unit, ValidationResult>>
    {
        /// <summary>
        /// Handles the resend verification email query.
        /// </summary>
        /// <param name="request">The <see cref="ResendVerificationEmailQuery"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Result{Unit, ValidationResult}"/> indicating success or validation failure.</returns>
        public async Task<Result<Unit, ValidationResult>> Handle(
            ResendVerificationEmailQuery request,
            CancellationToken cancellationToken)
        {
            // Retrieve user by email
            var user = await userReadRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);

            if (user == null)
            {
                // Security: Return success to prevent email enumeration
                logger.LogWarning("Resend verification email requested for non-existing email: {Email}", request.Email);
                return Unit.Value;
            }

            // Check if email is already verified
            if (user.IsEmailVerified)
            {
                logger.LogInformation("Resend verification email requested for already verified email: {Email}", request.Email);
                return ResultExtensions.ValidationFailure<Unit>(
                    nameof(request.Email),
                    localization.GetString("User.EmailAlreadyVerified"));
            }

            // Publish event to trigger verification email sending
            await mediator.Publish(new UserRequestedVerificationEvent(user.Id, request.Email, user.Username), cancellationToken);

            logger.LogInformation("Verification email resend requested for user: {UserId}", user.Id);

            return Result.Success<Unit, ValidationResult>(Unit.Value);
        }
    }
}
