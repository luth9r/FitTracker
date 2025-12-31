using FitTracker.Application.Events;
using FitTracker.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.User.Handlers.Events
{
    /// <summary>
    /// Handler for processing user-requested verification email events.
    /// </summary>
    /// <param name="jwtTokenService">Service for generating verification JWT tokens.</param>
    /// <param name="configuration">Application configuration containing verification link base URL.</param>
    /// <param name="emailService">Service for sending emails.</param>
    /// <param name="localization">Service for retrieving localized email content.</param>
    /// <param name="logger">Logger for diagnostic information.</param>
    public class UserVerificationRequestedHandler(
        IJwtTokenGenerator jwtTokenService,
        IConfiguration configuration,
        IEmailService emailService,
        ILocalizationService localization,
        ILogger<UserVerificationRequestedHandler> logger) : INotificationHandler<UserRequestedVerificationEvent>
    {
        /// <summary>
        /// Handles the user requested verification event by sending a verification email.
        /// </summary>
        /// <param name="notification">The event containing user information.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        public async Task Handle(UserRequestedVerificationEvent notification, CancellationToken cancellationToken)
        {
            var verificationToken = jwtTokenService.GenerateVerificationToken(notification.UserId);
            var verificationLinkBase = configuration["App:VerificationLinkBase"];
            var verificationLink = $"{verificationLinkBase}?token={verificationToken}";

            var subject = localization.GetString("Email.Verification.Subject");
            var bodyTemplate = localization.GetString("Email.Verification.Body");

            var emailBody = bodyTemplate
                .Replace("{0}", notification.Username)
                .Replace("{1}", verificationLink);

            await emailService.SendEmailAsync(notification.Email, subject, emailBody, cancellationToken);

            logger.LogInformation("Verification email sent to {Email}", notification.Email);
        }
    }
}
