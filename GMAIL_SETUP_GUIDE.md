# Gmail Setup Guide for Inventory API

## 🔧 Quick Gmail Setup for Development

### Step 1: Enable 2-Factor Authentication
1. Go to [Google Account Security](https://myaccount.google.com/security)
2. Click on "2-Step Verification"
3. Follow the setup process if not already enabled

### Step 2: Generate App Password
1. Go to [Google App Passwords](https://myaccount.google.com/apppasswords)
2. Select "Mail" from the dropdown
3. Click "Generate"
4. Copy the 16-character password (e.g., `abcd efgh ijkl mnop`)

### Step 3: Update Configuration
Replace the email settings in your `appsettings.Development.json`:

```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "EnableSsl": true,
    "FromEmail": "your.email@gmail.com",
    "FromName": "Storage Unit Tracker",
    "Username": "your.email@gmail.com",
    "Password": "your-16-char-app-password"
  }
}
```

## 🔄 Alternative Email Providers

### Option 2: Outlook/Hotmail (Free)
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp-mail.outlook.com",
    "SmtpPort": 587,
    "EnableSsl": true,
    "FromEmail": "your.email@outlook.com",
    "FromName": "Storage Unit Tracker",
    "Username": "your.email@outlook.com",
    "Password": "your-outlook-password"
  }
}
```

### Option 3: SendGrid (Professional - 100 free emails/day)
1. Sign up at [SendGrid](https://sendgrid.com)
2. Create an API key
3. Configuration:
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": 587,
    "EnableSsl": true,
    "FromEmail": "noreply@yourdomain.com",
    "FromName": "Storage Unit Tracker",
    "Username": "apikey",
    "Password": "your-sendgrid-api-key"
  }
}
```

### Option 4: Mailtrap (Development Only)
Perfect for testing without sending real emails:
1. Sign up at [Mailtrap](https://mailtrap.io)
2. Create an inbox
3. Use provided SMTP credentials

## 🚀 Quick Test Setup

### For Gmail (Recommended for immediate testing):
1. Use your personal Gmail account
2. Generate app password as described above
3. Update settings and test immediately

### For Production:
- Consider SendGrid or dedicated email service
- Use environment variables for credentials
- Implement proper error handling

## 🏭 Gmail for Production: 300 Emails/Day Analysis

### ✅ **YES, Gmail CAN Handle 300 Emails/Day**

**Gmail SMTP Limits:**
- **Daily limit**: 500 emails per day for personal Gmail accounts
- **Rate limit**: 2-3 emails per second maximum
- **Monthly limit**: No additional restrictions beyond daily
- **Your usage**: 300 emails/day = **Well within limits** ✅

### 📊 **Cost Comparison for 300 Emails/Day**

| Provider | Daily Limit | Monthly Cost | Notes |
|----------|-------------|--------------|-------|
| **Gmail** | 500 emails | **FREE** 🎯 | Personal account |
| SendGrid | 100 free, then paid | ~$15/month | Professional |
| AWS SES | No free tier | ~$3/month | Pay per email |
| Mailgun | 100 free, then paid | ~$15/month | Developer-friendly |

### 🎯 **Gmail Production Advantages**
✅ **Completely Free** - No monthly costs
✅ **Reliable Infrastructure** - Google's 99.9%+ uptime
✅ **Good Deliverability** - Established reputation
✅ **Simple Setup** - No complex API keys
✅ **Familiar Interface** - Easy to monitor in Gmail

### ⚠️ **Gmail Production Considerations**

**Potential Issues:**
1. **Account Suspension Risk** - If Google detects "unusual activity"
2. **Professional Appearance** - Recipients see your Gmail address
3. **Limited Analytics** - No detailed delivery reports
4. **Support** - No dedicated business support
5. **Branding** - Can't use custom domain easily

### 🛡️ **How to Safely Use Gmail in Production**

#### 1. **Create a Dedicated Gmail Account**
```
Example: storage.unit.notifications@gmail.com
```
- Don't use your personal account
- Use a descriptive name
- Enable 2FA immediately

#### 2. **Implement Rate Limiting**
```csharp
// Add to your EmailService
private readonly SemaphoreSlim _rateLimiter = new(2, 2); // Max 2 concurrent emails

public async Task SendPackageArrivalNotificationAsync(...)
{
    await _rateLimiter.WaitAsync();
    try
    {
        // Your existing email sending code
        await Task.Delay(500); // 500ms delay between emails
    }
    finally
    {
        _rateLimiter.Release();
    }
}
```

#### 3. **Monitor Email Sending**
```csharp
// Enhanced logging in EmailService
_logger.LogInformation("Daily email count: {Count}/500", dailyEmailCount);
if (dailyEmailCount > 400)
{
    _logger.LogWarning("Approaching Gmail daily limit!");
}
```

#### 4. **Backup Plan Configuration**
```json
{
  "EmailSettings": {
    "Primary": {
      "SmtpHost": "smtp.gmail.com",
      "FromEmail": "storage.notifications@gmail.com"
    },
    "Fallback": {
      "SmtpHost": "smtp.sendgrid.net",
      "FromEmail": "backup@yourdomain.com"
    }
  }
}
```

## 🔍 Testing Your Setup

After configuration, test with a simple API call:
```bash
curl -X POST "http://localhost:5000/api/packages" \
-H "Content-Type: application/json" \
-d '{
  "id": "TEST001",
  "surname": "TestUser",
  "weight": 1.0,
  "emailNotification": {
    "sendNotification": true,
    "notificationMessage": "Test email setup"
  }
}'
```

## ⚠️ Security Notes

- Never commit real passwords to version control
- Use environment variables in production
- App passwords are safer than your main password
- Monitor email sending logs for issues

## 🎯 **Final Recommendation for 300 Emails/Day**

### **Option 1: Start with Gmail (Recommended)**
- **Cost**: FREE
- **Setup Time**: 5 minutes
- **Risk**: Low for your volume
- **When to switch**: If you get account warnings or need professional features

### **Option 2: Hybrid Approach**
- **Development**: Gmail
- **Production**: Start with Gmail, migrate to SendGrid if needed
- **Cost**: $0 initially, $15/month if you need to upgrade

### **Option 3: Go Professional Immediately**
- **SendGrid**: 100 free + $15/month for unlimited
- **Better**: Professional appearance, analytics, support
- **When**: If this is a business application

## 💡 **Quick Decision Matrix**

**Choose Gmail if:**
- Budget is tight (free)
- Low business risk
- Can tolerate Gmail branding
- Technical person managing it

**Choose SendGrid if:**
- Professional appearance matters
- Need delivery analytics
- Business-critical application
- Want dedicated support

## 🚀 **Next Steps**

1. **Start with Gmail** for immediate testing
2. **Monitor usage** and delivery rates
3. **Keep SendGrid as backup option**
4. **Switch if you hit issues or need professional features**

Your 300 emails/day is well within Gmail's limits, so you can definitely start free and scale up later if needed!
