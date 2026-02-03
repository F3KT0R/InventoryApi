using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Services;
using InventoryApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Email Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

// CORS Configuration - MUST be before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontends", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrWhiteSpace(origin))
                return false;

            try
            {
                var uri = new Uri(origin);

                // Allow production Netlify (HTTPS only)
                if (uri.Host == "storage-unit.netlify.app" && uri.Scheme == "https")
                    return true;

                // Allow localhost (HTTP)
                if (uri.Host == "localhost" && uri.Scheme == "http")
                    return true;

                // Allow local network IPs (HTTP and HTTPS for phone testing)
                if (uri.Host.StartsWith("192.168.") && (uri.Scheme == "http" || uri.Scheme == "https"))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection in production (Render provides HTTPS)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// ⚠️ CRITICAL: Order matters! UseCors MUST come FIRST
app.UseCors("AllowFrontends");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
