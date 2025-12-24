namespace FitTracker.Application.Interfaces
{
    /// <summary>
    /// Service for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="to">The recipient email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="htmlBody">The email body.</param>
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}
