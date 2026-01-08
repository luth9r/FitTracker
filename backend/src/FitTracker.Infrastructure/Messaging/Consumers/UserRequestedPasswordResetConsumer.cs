using FitTracker.Application.Interfaces;
using FitTracker.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace FitTracker.Infrastructure.Messaging.Consumers;

/// <summary>
///     Handles the processing of user password reset requests by consuming
///     <see cref="UserRequestedPasswordResetEvent" /> messages.
/// </summary>
/// <param name="emailService">Dependency for sending email notifications.</param>
/// <param name="tokenGenerator">Dependency for generating JWT tokens, including verification tokens.</param>
/// <param name="configuration">Dependency for accessing application configuration values.</param>
/// <param name="localization">Dependency for retrieving localized strings for email subject and body.</param>
public class UserRequestedPasswordResetConsumer(
    IEmailService emailService,
    IJwtTokenGenerator tokenGenerator,
    IConfiguration configuration,
    ILocalizationService localization) : IConsumer<UserRequestedPasswordResetEvent>
{
    /// <summary>
    ///     Consumes the <see cref="UserRequestedPasswordResetEvent" /> and handles the process of sending
    ///     a password reset email to the user, including generating the reset token and constructing the reset link.
    /// </summary>
    /// <param name="context">
    ///     The consume context containing the message of type <see cref="UserRequestedPasswordResetEvent" />
    ///     and associated metadata.
    /// </param>
    /// <returns> A task representing the asynchronous operation. </returns>
    public async Task Consume(ConsumeContext<UserRequestedPasswordResetEvent> context)
    {
        var verificationToken = tokenGenerator.GeneratePasswordResetToken(context.Message.UserId);
        var verificationLinkBase = configuration["App:ResetPasswordLinkBase"];
        var verificationLink = $"{verificationLinkBase}?token={verificationToken}";

        var subject = localization.GetString("Email.ResetPassword.Subject");
        var bodyTemplate = localization.GetString("Email.ResetPassword.Body");

        var emailBody = bodyTemplate
            .Replace("{0}", context.Message.Username)
            .Replace("{1}", verificationLink);

        await emailService.SendEmailAsync(context.Message.Email, subject, emailBody, context.CancellationToken);
    }
}