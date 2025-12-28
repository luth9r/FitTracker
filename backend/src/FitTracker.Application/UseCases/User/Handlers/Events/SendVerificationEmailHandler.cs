using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FitTracker.Application.Events;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.User.Handlers.Events
{
    /// <summary>
    /// Handles <see cref="UserRegisteredEvent"/> by sending a localized HTML email with verification link to the newly registered user.
    /// </summary>
    /// <param name="jwtTokenService">Generates JWT verification tokens for email confirmation links.</param>
    /// <param name="configuration">Provides application configuration including verification link base URL.</param>
    /// <param name="emailService">SMTP email service for sending verification emails.</param>
    /// <param name="localization">Provides localized email subjects and HTML templates from JSON resources.</param>
    /// <param name="logger">Application logger for tracking email delivery success/failure.</param>
    public sealed class SendVerificationEmailHandler(
        IJwtTokenGenerator jwtTokenService,
        IConfiguration configuration,
        IEmailService emailService,
        ILocalizationService localization,
        ILogger<SendVerificationEmailHandler> logger
        ) : INotificationHandler<UserRegisteredEvent>
    {
        /// <summary>
        /// Asynchronously sends a localized verification email to the newly registered user.
        /// </summary>
        /// <param name="notification">Contains user ID, email, and username for the verification email.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
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
