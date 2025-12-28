using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Application.Events;
using FitTracker.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.UseCases.User.Handlers.Events
{
    /// <summary>
    /// Handles <see cref="UserPasswordResetRequestedEvent"/> by sending a localized HTML email with password reset link to the user.
    /// </summary>
    /// <param name="localization">Provides localized email subjects and HTML templates from JSON resources.</param>
    /// <param name="emailService">SMTP email service for sending password reset emails.</param>
    /// <param name="configuration">Provides application configuration including reset password link base URL.</param>
    /// <param name="jwtTokenService">Generates JWT verification tokens for password reset confirmation links.</param>
    /// <param name="logger">Application logger for tracking email delivery success/failure.</param>
    public sealed class UserPasswordResetRequestedHandler(
        ILocalizationService localization,
        IEmailService emailService,
        IConfiguration configuration,
        IJwtTokenGenerator jwtTokenService,
        ILogger<UserPasswordResetRequestedHandler> logger) : INotificationHandler<UserPasswordResetRequestedEvent>
    {
        /// <summary>
        /// Asynchronously sends a localized password reset email to the user.
        /// </summary>
        /// <param name="notification">Contains user ID, email, and username for the password reset email.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        public async Task Handle(UserPasswordResetRequestedEvent notification, CancellationToken cancellationToken)
        {
            var verificationToken = jwtTokenService.GeneratePasswordResetToken(notification.UserId);
            var verificationLinkBase = configuration["App:ResetPasswordLinkBase"];
            var verificationLink = $"{verificationLinkBase}?token={verificationToken}";

            var subject = localization.GetString("Email.ResetPassword.Subject");
            var bodyTemplate = localization.GetString("Email.ResetPassword.Body");

            var emailBody = bodyTemplate
                .Replace("{0}", notification.Username)
                .Replace("{1}", verificationLink);

            await emailService.SendEmailAsync(notification.Email, subject, emailBody, cancellationToken);

            logger.LogInformation("Reset password email sent to {Email}", notification.Email);
        }
    }
}
