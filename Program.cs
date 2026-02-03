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

// Add CORS services and define a policy
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        corsBuilder =>
        {
            corsBuilder.WithOrigins(allowedOrigins)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
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

// CRITICAL: UseCors MUST come before UseAuthorization and MapControllers
app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
