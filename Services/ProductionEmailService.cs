using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using InventoryApi.Models;
using System.Threading;

namespace InventoryApi.Services
{
    public class ProductionEmailService : IEmailService, IDisposable
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<ProductionEmailService> _logger;
        private readonly SemaphoreSlim _rateLimiter;
        private static int _dailyEmailCount = 0;
        private static DateTime _lastResetDate = DateTime.UtcNow.Date;

        public ProductionEmailService(IOptions<EmailSettings> emailSettings, ILogger<ProductionEmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _rateLimiter = new SemaphoreSlim(2, 2); // Max 2 concurrent emails
        }

        public async Task SendPackageArrivalNotificationAsync(string toEmail, string toName, string surname, string packageId, double weight, string? customMessage = null)
        {
            // Reset daily counter if new day
            if (DateTime.UtcNow.Date > _lastResetDate)
            {
                _dailyEmailCount = 0;
                _lastResetDate = DateTime.UtcNow.Date;
                _logger.LogInformation("Daily email counter reset for new day");
            }

            // Check daily limit (Gmail allows 500, we'll be conservative at 450)
            if (_dailyEmailCount >= 450)
            {
                _logger.LogError("Daily email limit reached ({Count}/450). Email not sent to {Email}", _dailyEmailCount, toEmail);
                throw new InvalidOperationException("Daily email limit reached. Please try again tomorrow.");
            }

            await _rateLimiter.WaitAsync();
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = "📦 Package Arrival Notification - Storage Unit";

                var bodyBuilder = new BodyBuilder();

                // Create the email body with better formatting
                var emailBody = $@"Dear {toName},

🎉 Great news! A new package has arrived for you at the storage unit.

📦 PACKAGE DETAILS:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
• Package ID: {packageId}
• Recipient: {surname}
• Weight: {weight:F2} kg
• Arrival Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC
• Status: In Storage
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

{(string.IsNullOrEmpty(customMessage) ? "" : $@"💬 PERSONAL MESSAGE:
{customMessage}

")}📍 NEXT STEPS:
Please collect your package at your earliest convenience during our operating hours.

If you have any questions, please contact the storage facility directly.

Best regards,
Storage Unit Management Team

---
🤖 This is an automated notification from Storage Unit Tracker.
📧 This email was sent to: {toEmail}
🕒 Sent on: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC

Please do not reply to this email.
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

                // Increment counter and log success
                Interlocked.Increment(ref _dailyEmailCount);
                _logger.LogInformation("Email notification sent successfully to {Email} for package {PackageId}. Daily count: {Count}/450",
                    toEmail, packageId, _dailyEmailCount);

                // Warning if approaching limit
                if (_dailyEmailCount > 400)
                {
                    _logger.LogWarning("Approaching Gmail daily limit! Current count: {Count}/450", _dailyEmailCount);
                }

                // Add small delay to respect rate limits
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification to {Email} for package {PackageId}. Daily count: {Count}",
                    toEmail, packageId, _dailyEmailCount);
                throw;
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        public void Dispose()
        {
            _rateLimiter?.Dispose();
        }
    }
}
