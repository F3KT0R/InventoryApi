# Free Hosting Options for .NET Backend

## Recommended: Railway (https://railway.app)

**Best option for your use case**
- ✅ Free tier: $5/month credit (enough for small apps)
- ✅ Easy deployment from GitHub
- ✅ Supports .NET natively
- ✅ Automatic HTTPS
- ✅ Environment variables built-in
- ✅ PostgreSQL database support

### Deployment Steps:
1. Push your code to GitHub
2. Sign up at railway.app
3. Create new project → Deploy from GitHub
4. Select your repository
5. Add environment variables in Railway dashboard
6. Railway auto-detects .NET and builds it

---

## Alternative: Render (https://render.com)

**Good alternative**
- ✅ Free tier available
- ✅ Deploy from GitHub
- ✅ Supports Docker (needed for .NET)
- ✅ Auto-deploy on git push
- ⚠️ Free tier spins down after inactivity (slow cold starts)

### Deployment Steps:
1. Create a `Dockerfile` in project root
2. Push to GitHub
3. Connect Render to your GitHub repo
4. Configure environment variables in Render
5. Deploy

---

## Alternative: Azure App Service (https://azure.microsoft.com)

**Microsoft's solution**
- ✅ Native .NET support (best compatibility)
- ✅ Free tier (F1 - 60 min/day limit)
- ✅ Easy deployment with VS Code Azure extension
- ⚠️ More complex setup
- ⚠️ Limited free tier

---

## Environment Variables Setup

After deploying to any platform, set these environment variables:

```
ConnectionStrings__DefaultConnection=User Id=postgres.zpvzpflggnjpqjmhyrpn;Password=BorisBalintOrescanin;Server=aws-1-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Pooling=false;No Reset On Close=true;

EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__EnableSsl=true
EmailSettings__FromEmail=boris.orescanin.uk@gmail.com
EmailSettings__FromName=Storage Unit Tracker
EmailSettings__Username=boris.orescanin.uk@gmail.com
EmailSettings__Password=tgmf gvdz ojzk ckls
```

Note: Use double underscores `__` for nested configuration in environment variables.

---

## CORS Configuration for Netlify

Update your `Program.cs` CORS settings to allow your Netlify domain:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "https://your-app.netlify.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
```
