using FitTracker.Application.Interfaces;

namespace FitTrackerInfrastructure.Tests.TestDoubles
{
    public class FakeEmailService : IEmailService
    {
        public List<SentEmail> SentEmails { get; } = new();

        public bool ShouldThrowException { get; set; }

        public Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            if (ShouldThrowException)
            {
                throw new InvalidOperationException("Email sending failed");
            }

            SentEmails.Add(new SentEmail(to, subject, htmlBody));
            return Task.CompletedTask;
        }

        public void Clear() => SentEmails.Clear();
    }

    public record SentEmail(string To, string Subject, string HtmlBody);
}
