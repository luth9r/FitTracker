using FitTracker.Application.Interfaces;
using FitTracker.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace FitTracker.Infrastructure.Messaging.Consumers;

/// <summary>
///     Handles the consumption of <see cref="UserRequestedVerificationEvent" /> messages.
/// </summary>
/// <param name="emailService">Dependency for sending email notifications.</param>
/// є
/// <param name="tokenGenerator">Dependency for generating JWT tokens, including verification tokens.</param>
/// <param name="configuration">Dependency for accessing application configuration values.</param>
/// <param name="localization">Dependency for retrieving localized strings for email subject and body.</param>
/// <param name="logger">Dependency for logging events and errors during message consumption.</param>
public sealed class UserVerificationRequestedConsumer(
    IEmailService emailService,
    IJwtTokenGenerator tokenGenerator,
    IConfiguration configuration,
    ILocalizationService localization) : IConsumer<UserRequestedVerificationEvent>
{
    public async Task Consume(ConsumeContext<UserRequestedVerificationEvent> context)
    {
        var notification = context.Message;

        // Generate verification token
        var verificationToken = tokenGenerator.GenerateVerificationToken(notification.UserId);

        // Create verification link
        var verificationLinkBase = configuration["App:VerificationLinkBase"];
        var verificationLink = $"{verificationLinkBase}?token={verificationToken}";

        // Localize email body
        var subject = localization.GetString("Email.Verification.Subject", notification.Culture);
        var bodyTemplate = localization.GetString("Email.Verification.Body", notification.Culture);

        var emailBody = bodyTemplate
            .Replace("{0}", notification.Username)
            .Replace("{1}", verificationLink);

        // Send verification email
        // If email sending fails, MassTransit will retry the message
        await emailService.SendEmailAsync(notification.Email, subject, emailBody, context.CancellationToken);
    }
}