using System.Net;
using System.Net.Mail;
using FitTracker.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Services
{
    public sealed class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly string _smtpHost = configuration["Email:SmtpHost"] ?? "localhost";
        private readonly int _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "1025");

        /// <inheritdoc/>
        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(),
                EnableSsl = false,
            };

            var message = new MailMessage("no-reply@fittracker.com", to, subject, htmlBody)
            {
                IsBodyHtml = true,
            };

            await client.SendMailAsync(message);
        }
    }
}
