using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;
using InventoryApi.Services;
using Npgsql;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<PackagesController> _logger;

        public PackagesController(ApiDbContext context, IEmailService emailService, ILogger<PackagesController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: api/packages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> GetPackages()
        {
            return await _context.Packages.OrderByDescending(p => p.ArrivalDate).ToListAsync();
        }

        // POST: api/packages
        [HttpPost]
        public async Task<ActionResult<Package>> PostPackage(CreatePackageRequest request)
        {
            // --- START: ROBUST VALIDATION ---
            // Case-insensitive check to see if the user exists.
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == request.Surname.ToLower());

            if (user == null)
            {
                // If the user doesn't exist, reject the request with a clear error message.
                return BadRequest(new { message = $"Cannot add package. The user '{request.Surname}' does not exist." });
            }
            // --- END: ROBUST VALIDATION ---

            // Create the package object
            var package = new Package
            {
                Id = request.Id,
                Surname = request.Surname,
                Weight = request.Weight,
                ArrivalDate = DateTime.UtcNow,
                Status = "In Storage"
            };

            _context.Packages.Add(package);

            try
            {
                await _context.SaveChangesAsync();

                // Send email notification if requested
                if (request.EmailNotification?.SendNotification == true && !string.IsNullOrEmpty(user.Email))
                {
                    try
                    {
                        await _emailService.SendPackageArrivalNotificationAsync(
                            user.Email,
                            user.Name,
                            request.Surname,
                            request.Id,
                            request.Weight,
                            request.EmailNotification.NotificationMessage
                        );
                        _logger.LogInformation("Email notification sent for package {PackageId} to {Email}", request.Id, user.Email);
                    }
                    catch (Exception emailEx)
                    {
                        // Log the email error but don't fail the package creation
                        _logger.LogWarning(emailEx, "Failed to send email notification for package {PackageId} to {Email}. Package was created successfully.", request.Id, user.Email);
                    }
                }
                else if (request.EmailNotification?.SendNotification == true && string.IsNullOrEmpty(user.Email))
                {
                    _logger.LogWarning("Email notification requested for package {PackageId} but user {UserName} has no email address", request.Id, user.Name);
                }
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
            {
                // This specifically handles the case where a package with the same ID (primary key) already exists.
                return Conflict(new { message = $"A package with the ID '{package.Id}' already exists." });
            }

            // If everything is successful, return a 201 Created response.
            return CreatedAtAction(nameof(GetPackages), new { id = package.Id }, package);
        }

        // PUT: api/packages/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePackageStatus(string id, [FromBody] StatusUpdateDto statusUpdate)
        {
            var package = await _context.Packages.FindAsync(id);

            if (package == null)
            {
                return NotFound();
            }

            package.Status = statusUpdate.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class StatusUpdateDto
    {
        public required string Status { get; set; }
    }
}
