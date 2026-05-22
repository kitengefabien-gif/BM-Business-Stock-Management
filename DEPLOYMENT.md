# Deployment Guide

## Environment Setup

### Prerequisites
- .NET 8.0 SDK installed
- Supabase account and project
- Target platform (Android, iOS, Windows, macOS)
- Appropriate platform SDKs

## Step 1: Configure Environment

### 1.1 Update appsettings.json

```json
{
  "Supabase": {
    "Url": "https://your-project.supabase.co",
    "AnonKey": "your-anon-key",
    "ServiceKey": "your-service-key"
  },
  "Features": {
    "EnableOfflineMode": true,
    "EnableRealtimeSync": true,
    "EnableWhatsAppNotifications": true
  }
}
```

### 1.2 Setup Supabase Database

1. Create new Supabase project
2. Go to SQL Editor
3. Run migrations from `sql/` folder:
   - 001_initial_schema.sql
   - 002_stock_schema.sql
   - 003_finance_schema.sql
   - 004_notification_schema.sql

## Step 2: Build Application

### 2.1 For Windows Desktop

```bash
dotnet maui build -f net8.0-windows --configuration Release
```

### 2.2 For Android

```bash
# Requires Android SDK API 30+
dotnet maui build -f net8.0-android --configuration Release

# Or direct to APK
dotnet maui publish -f net8.0-android --configuration Release
```

### 2.3 For iOS

```bash
# Requires Xcode 14+
dotnet maui build -f net8.0-ios --configuration Release

# For App Store
dotnet maui publish -f net8.0-ios --configuration Release
```

## Step 3: Publishing

### 3.1 Android (Google Play Store)

1. Generate keystore:
```bash
keytool -genkey -v -keystore release.keystore -alias release -keyalg RSA -keysize 2048 -validity 10000
```

2. Build release APK:
```bash
dotnet maui publish -f net8.0-android -c Release
```

3. Upload to Google Play Console

### 3.2 iOS (Apple App Store)

1. Set up certificates in Xcode
2. Build release package:
```bash
dotnet maui publish -f net8.0-ios -c Release
```

3. Upload via App Store Connect

### 3.3 Windows (Microsoft Store)

```bash
dotnet maui publish -f net8.0-windows -c Release
```

## Step 4: Testing Before Deployment

### 4.1 Unit Tests
```bash
dotnet test --configuration Release
```

### 4.2 Manual Testing Checklist
- [ ] Login/Register flow
- [ ] Offline mode functionality
- [ ] Truck entry and editing
- [ ] Stock management
- [ ] Financial calculations
- [ ] Data sync on reconnection
- [ ] WhatsApp notifications
- [ ] GPS tracking (if available)

## Step 5: Monitoring

### 5.1 Enable Logging

In appsettings.json:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### 5.2 Setup Error Tracking
- Consider using Sentry or Application Insights
- Monitor Supabase logs
- Track app crashes

## Troubleshooting

### Issue: Supabase connection fails
- Verify URL and key in appsettings.json
- Check network connectivity
- Ensure Supabase project is active

### Issue: Database migrations fail
- Ensure SQL syntax is correct
- Check for duplicate table names
- Verify RLS policies

### Issue: App crashes on startup
- Check dependency injection configuration
- Verify all services are registered
- Check local database permissions

## Performance Optimization

1. **Database**: Add indexes for common queries (already done)
2. **API Calls**: Implement caching for frequently accessed data
3. **UI**: Use virtualization for large lists
4. **Network**: Compress data transfer, implement pagination

## Security Checklist

- [ ] HTTPS enforced
- [ ] JWT tokens secured
- [ ] SQL injection prevention (parameterized queries)
- [ ] XSS prevention in UI
- [ ] Data encryption for sensitive fields
- [ ] Regular security updates
- [ ] Code obfuscation for release builds

## Maintenance

### Regular Tasks
- Monitor error logs
- Update dependencies monthly
- Review performance metrics
- Backup database

### Version Updates
- Update .NET Runtime
- Update MAUI framework
- Update Supabase SDK
- Update third-party packages
