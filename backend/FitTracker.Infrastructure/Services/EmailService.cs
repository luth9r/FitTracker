using FitTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Services
{
    public class EmailService(ILogger<EmailService> logger) : IEmailService
    {
        /// <inheritdoc/>
        public Task SendEmailAsync(string to, string subject, string body)
        {
            logger.LogInformation("--- NEW EMAIL (SIMULATION) ---");
            logger.LogInformation("To: {To}", to);
            logger.LogInformation("Subject: {Subject}", subject);
            logger.LogInformation("Body: {Body}", body);
            logger.LogInformation("--- END OF EMAIL ---");

            return Task.CompletedTask;
        }
    }
}
