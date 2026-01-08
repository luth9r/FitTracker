using FitTracker.Application.Interfaces;
using FitTracker.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Messaging.Consumers;

/// <summary>
/// Consumer class that listens for the <see cref="UserRegisteredEvent"/> and processes user registration events.
/// This consumer is responsible for generating a verification token, constructing a verification link,
/// and sending the verification email to the newly registered user.
/// </summary>
/// <param name="jwtTokenService">Dependency for generating JWT tokens, including verification tokens.</param>
/// <param name="configuration">Dependency for accessing application configuration values.</param>
/// <param name="emailService">Dependency for sending email notifications.</param>
/// <param name="localization">Dependency for retrieving localized strings for email subject and body.</param>
/// <param name="logger">Dependency for logging events and errors during message consumption.</param>
public sealed class UserRegisteredConsumer(
    IJwtTokenGenerator jwtTokenService,
    IConfiguration configuration,
    IEmailService emailService,
    ILocalizationService localization,
    ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredEvent>
{
    /// <summary>
    /// Consumes the UserRegisteredEvent message, generates a verification token, and sends a verification email
    /// to the user based on the event data.
    /// </summary>
    /// <param name="context"> The consume context containing the UserRegisteredEvent message and related metadata. </param>
    /// <returns> A task representing the asynchronous operation. </returns>
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
            var notification = context.Message;

            var verificationToken = jwtTokenService.GenerateVerificationToken(notification.UserId);
            var verificationLinkBase = configuration["App:VerificationLinkBase"];
            var verificationLink = $"{verificationLinkBase}?token={verificationToken}";

            var subject = localization.GetString("Email.Verification.Subject", notification.Culture);
            var bodyTemplate = localization.GetString("Email.Verification.Body", notification.Culture);

            var emailBody = bodyTemplate
                .Replace("{0}", notification.Username)
                .Replace("{1}", verificationLink);

            await emailService.SendEmailAsync(notification.Email, subject, emailBody, context.CancellationToken);

            logger.LogInformation("Verification email sent to {Email} via RabbitMQ Consumer", notification.Email);
    }
}
