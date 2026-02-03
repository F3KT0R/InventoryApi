using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models
{
    public class CreatePackageRequest
    {
        public required string Id { get; set; }
        public required string Surname { get; set; }
        public double Weight { get; set; }
        public EmailNotificationOptions? EmailNotification { get; set; }
    }

    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }
    }

    public class EmailNotificationOptions
    {
        public bool SendNotification { get; set; }
        public string? NotificationMessage { get; set; }
    }

    public class EmailSettings
    {
        public required string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public required string FromEmail { get; set; }
        public required string FromName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
