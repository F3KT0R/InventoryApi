using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface IEmailService
    {
        Task SendPackageArrivalNotificationAsync(string toEmail, string toName, string surname, string packageId, double weight, string? customMessage = null);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendPackageArrivalNotificationAsync(string toEmail, string toName, string surname, string packageId, double weight, string? customMessage = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = "Package Arrival Notification - Storage Unit";

                var bodyBuilder = new BodyBuilder();

                // Create the email body
                var emailBody = $@"
Dear {toName},

A new package has arrived for you at the storage unit:

📦 Package ID: {packageId}
👤 Recipient: {surname}
📊 Weight: {weight:F2} kg
📅 Arrival Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC
📍 Status: In Storage

{(string.IsNullOrEmpty(customMessage) ? "" : $@"
Personal Message:
{customMessage}

")}

Best regards,
Storage Unit Management Team

---
This is an automated notification. Please do not reply to this email.
";

                bodyBuilder.TextBody = emailBody;
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();

                // Connect to the SMTP server
                await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort,
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

                // Authenticate
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                // Send the email
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email notification sent successfully to {Email} for package {PackageId}", toEmail, packageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification to {Email} for package {PackageId}", toEmail, packageId);
                throw;
            }
        }
    }
}
