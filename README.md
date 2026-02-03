# Inventory API

A .NET 9.0 Web API for managing storage unit inventory with user management and package tracking features.

## Features

- User management (CRUD operations)
- Package tracking with email notifications
- Integration with Supabase PostgreSQL database
- Email notifications via Gmail SMTP

## Local Development

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL (via Supabase)

### Setup

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd InventoryApi
   ```

2. **Create `appsettings.Development.json`**

   Copy the template and add your credentials:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "AllowedOrigins": [
       "http://localhost:5173"
     ],
     "ConnectionStrings": {
       "DefaultConnection": "User Id=YOUR_USER;Password=YOUR_PASSWORD;Server=YOUR_SERVER;Port=6543;Database=postgres;Pooling=false;No Reset On Close=true;"
     },
     "EmailSettings": {
       "SmtpHost": "smtp.gmail.com",
       "SmtpPort": 587,
       "EnableSsl": true,
       "FromEmail": "your-email@gmail.com",
       "FromName": "Storage Unit Tracker",
       "Username": "your-email@gmail.com",
       "Password": "your-app-password"
     }
   }
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5234`

## Deployment to Railway

### Step 1: Prepare for GitHub

Your project is already configured with `.gitignore` to protect sensitive data.

1. **Initialize git (if not already done)**
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   ```

2. **Push to GitHub**
   ```bash
   git remote add origin <your-github-repo-url>
   git branch -M main
   git push -u origin main
   ```

### Step 2: Deploy on Railway

1. Sign up at [railway.app](https://railway.app)

2. Click "New Project" → "Deploy from GitHub repo"

3. Select your repository

4. **Add Environment Variables** in Railway dashboard:

   ```
   ConnectionStrings__DefaultConnection=User Id=YOUR_USER;Password=YOUR_PASSWORD;Server=YOUR_SERVER;Port=6543;Database=postgres;Pooling=false;No Reset On Close=true;

   EmailSettings__SmtpHost=smtp.gmail.com
   EmailSettings__SmtpPort=587
   EmailSettings__EnableSsl=true
   EmailSettings__FromEmail=your-email@gmail.com
   EmailSettings__FromName=Storage Unit Tracker
   EmailSettings__Username=your-email@gmail.com
   EmailSettings__Password=your-app-password

   AllowedOrigins__0=https://your-app.netlify.app
   AllowedOrigins__1=http://localhost:5173
   ```

5. Railway will automatically detect .NET and deploy your app

6. Copy your Railway app URL (e.g., `https://your-app.up.railway.app`)

### Step 3: Update Frontend

Update your Netlify frontend to use the Railway API URL instead of localhost.

## API Endpoints

### Users

- `GET /api/users` - Get all active users
- `POST /api/users` - Create a new user
  ```json
  {
    "name": "John Doe",
    "email": "john@example.com"
  }
  ```

### Packages

- `GET /api/packages` - Get all packages
- `POST /api/packages` - Create a new package with optional email notification

## Environment Variables Reference

| Variable | Description | Example |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | See above |
| `EmailSettings__SmtpHost` | SMTP server host | smtp.gmail.com |
| `EmailSettings__SmtpPort` | SMTP server port | 587 |
| `EmailSettings__EnableSsl` | Enable SSL for SMTP | true |
| `EmailSettings__FromEmail` | Sender email address | your-email@gmail.com |
| `EmailSettings__FromName` | Sender display name | Storage Unit Tracker |
| `EmailSettings__Username` | SMTP username | your-email@gmail.com |
| `EmailSettings__Password` | SMTP password/app password | your-app-password |
| `AllowedOrigins__0` | First allowed CORS origin | https://your-app.netlify.app |
| `AllowedOrigins__1` | Second allowed CORS origin | http://localhost:5173 |

## Security Notes

- Never commit `appsettings.Development.json` to version control
- Use app-specific passwords for Gmail SMTP
- Update CORS origins to match your actual frontend URLs
- Keep your Supabase credentials secure

## Tech Stack

- .NET 9.0
- Entity Framework Core
- PostgreSQL (Supabase)
- Npgsql
- ASP.NET Core Web API
