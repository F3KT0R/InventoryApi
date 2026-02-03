# Render Deployment Checklist

## ✅ Step-by-Step Deployment Guide

### 1. Verify Your Code is Ready
- [x] Dockerfile exists in project root
- [x] .gitignore protects sensitive files
- [x] CORS configured to read from environment variables
- [x] Program.cs has proper CORS and HTTPS configuration

### 2. Push to GitHub
```bash
git add .
git commit -m "Ready for Render deployment"
git push origin main
```

### 3. Create Render Web Service

1. Go to [render.com](https://render.com)
2. Sign up / Log in (no credit card needed)
3. Click **"New +"** → **"Web Service"**
4. Connect your GitHub account and select your repository
5. Configure the service:
   - **Name:** `inventory-api` (or your preferred name)
   - **Region:** Choose closest to you
   - **Branch:** `main`
   - **Root Directory:** Leave empty (or `InventoryApi` if needed)
   - **Environment:** **Docker**
   - **Instance Type:** **Free**

### 4. Add Environment Variables on Render

Click **"Advanced"** and add these environment variables:

```
ASPNETCORE_ENVIRONMENT=Production

ConnectionStrings__DefaultConnection=User Id=postgres.zpvzpflggnjpqjmhyrpn;Password=BorisBalintOrescanin;Server=aws-1-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Pooling=false;No Reset On Close=true;

EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__EnableSsl=true
EmailSettings__FromEmail=boris.orescanin.uk@gmail.com
EmailSettings__FromName=Storage Unit Tracker
EmailSettings__Username=boris.orescanin.uk@gmail.com
EmailSettings__Password=tgmf gvdz ojzk ckls

AllowedOrigins__0=https://storage-unit.netlify.app
AllowedOrigins__1=http://localhost:5173
AllowedOrigins__2=http://192.168.0.27:5173
AllowedOrigins__3=http://192.168.137.1:5173
```

**Important Notes:**
- Replace `https://storage-unit.netlify.app` with your actual Netlify URL
- Add your phone's IP addresses (check with `ipconfig` on Windows)
- NO trailing slashes on URLs

### 5. Deploy

1. Click **"Create Web Service"**
2. Wait for build to complete (3-5 minutes)
3. Once deployed, copy your Render URL (e.g., `https://inventory-api.onrender.com`)

### 6. Update Your Frontend (Netlify)

Add this environment variable on Netlify:

```
VITE_API_URL=https://your-app.onrender.com
```

Replace with your actual Render URL (no trailing slash).

### 7. Test Your Deployment

#### Test from Browser Console:
```javascript
fetch('https://your-app.onrender.com/api/users')
  .then(r => r.json())
  .then(console.log)
```

#### Test CORS:
```javascript
fetch('https://your-app.onrender.com/api/users', {
  headers: { 'Origin': 'https://storage-unit.netlify.app' }
})
  .then(r => r.json())
  .then(console.log)
```

## 🔧 Troubleshooting

### "Could not connect to backend"

1. **Check Render Dashboard:**
   - Is the service "Live" (green)?
   - Check logs for errors

2. **Test API Directly:**
   - Visit `https://your-app.onrender.com/api/users` in browser
   - Should return JSON (or error message)

3. **Check CORS:**
   - Verify `AllowedOrigins__0` matches your Netlify URL exactly
   - No trailing slashes
   - HTTPS for Netlify, HTTP for localhost

4. **Check Frontend Environment Variable:**
   - Netlify: `VITE_API_URL` should match Render URL
   - Rebuild Netlify after adding env vars

5. **First Request After Sleep:**
   - Render free tier spins down after 15 minutes
   - First request takes ~30 seconds (cold start)
   - Be patient!

### "CORS Error"

- Verify allowed origins in Render environment variables
- Check browser console for exact error
- Ensure CORS policy is before `UseAuthorization` in Program.cs

### "500 Internal Server Error"

- Check Render logs for database connection issues
- Verify connection string is correct
- Check Supabase database is accessible

## 📱 Testing from Phone

1. Make sure your phone is on the same WiFi network
2. Find your computer's IP: `ipconfig` (Windows) or `ifconfig` (Mac/Linux)
3. Add that IP to `AllowedOrigins__2` on Render (e.g., `http://192.168.0.27:5173`)
4. Redeploy Render service
5. Run frontend with: `npm run dev -- --host`
6. Access from phone: `http://192.168.0.27:5173`

## 🚀 After Successful Deployment

Your app is now live at:
- **Backend (Render):** `https://your-app.onrender.com`
- **Frontend (Netlify):** `https://storage-unit.netlify.app`

Remember: Free tier spins down after inactivity. First request may be slow!
