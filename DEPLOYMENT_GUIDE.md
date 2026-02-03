# Free Hosting Options for .NET Backend (Truly Free Forever)

## 🏆 Best Free Option: Render (https://render.com)

**100% Free Tier - No credit card required, no time limit**
- ✅ **Completely FREE forever** (not a trial)
- ✅ Deploy from GitHub automatically
- ✅ Native Docker support for .NET
- ✅ Automatic HTTPS
- ✅ Environment variables built-in
- ⚠️ Spins down after 15 minutes of inactivity (cold start ~30 seconds on first request)
- ⚠️ 750 hours/month free (plenty for one app)

**Best for:** Apps with occasional traffic, testing, portfolio projects

### Deployment Steps for Render:
1. Push your code to GitHub (with the Dockerfile already in your project)
2. Sign up at [render.com](https://render.com) - no credit card needed
3. Click "New +" → "Web Service"
4. Connect your GitHub repository
5. Configure:
   - **Name:** your-app-name
   - **Environment:** Docker
   - **Instance Type:** Free
6. Add environment variables (see below)
7. Click "Create Web Service"
8. Render will build and deploy automatically

---

## Alternative 1: Fly.io (https://fly.io)

**Free tier with generous allowances**
- ✅ Free tier: 3 VMs with 256MB RAM each
- ✅ Excellent .NET support
- ✅ CLI-based deployment
- ✅ Fast cold starts
- ⚠️ Requires credit card for verification (but won't charge unless you upgrade)
- ⚠️ More technical setup (CLI required)

### Deployment Steps for Fly.io:
1. Install Fly CLI: `powershell -c "iwr https://fly.io/install.ps1 -useb | iex"`
2. Login: `fly auth login`
3. In your project directory: `fly launch`
4. Follow prompts and set environment variables
5. Deploy: `fly deploy`

---

## Alternative 2: Koyeb (https://koyeb.com)

**Free tier available**
- ✅ Free tier with 1 service
- ✅ Deploy from GitHub
- ✅ Docker support
- ✅ No credit card required
- ⚠️ Smaller free tier resources
- ⚠️ May have geographic restrictions

---

## Alternative 3: Cyclic (https://cyclic.sh)

**Note:** Cyclic is mainly for Node.js, not ideal for .NET

---

## ⚠️ Limited Free Options

### Railway (https://railway.app)
- ❌ Only $5 credit for trial (requires paid Hobby plan after)
- Not truly free long-term

### Azure App Service
- ⚠️ Free tier (F1) has 60 CPU minutes/day limit
- Good for very light usage only

---

## 🎯 RECOMMENDED SETUP: Render

For your use case (Netlify frontend + .NET backend), **Render is the best free option**.

### What to expect:
- **First request after inactivity:** ~30 second delay (cold start)
- **Subsequent requests:** Normal speed
- **Perfect for:** Portfolio projects, learning, low-traffic apps
- **Upgrade path:** Easy to upgrade to paid tier later if needed

---

## Environment Variables Setup (All Platforms)

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

AllowedOrigins__0=https://your-app.netlify.app
AllowedOrigins__1=http://localhost:5173
```

Note: Use double underscores `__` for nested configuration in environment variables.

---

## CORS Configuration

Your app is already configured to read allowed origins from configuration. Once deployed:

1. Get your Netlify app URL (e.g., `https://your-app.netlify.app`)
2. Add it to the `AllowedOrigins__0` environment variable
3. Your API will allow requests from your Netlify frontend
