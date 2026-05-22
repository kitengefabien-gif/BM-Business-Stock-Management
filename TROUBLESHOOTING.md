# Troubleshooting Guide

## Common Issues and Solutions

### 1. Build Issues

#### Error: "Unable to resolve Supabase package"
```bash
# Solution: Restore packages
dotnet restore

# Or clear cache
dotnet nuget locals all --clear
dotnet restore
```

#### Error: "MAUI workload not installed"
```bash
# Solution: Install MAUI workload
dotnet workload restore
dotnet workload install maui
```

#### Error: "Android SDK not found"
```bash
# Solution: Install Android SDK
# On Windows:
# 1. Open Visual Studio Installer
# 2. Modify your installation
# 3. Select "Mobile development with .NET"
# 4. Click Modify

# Or use command line:
java -version
echo %ANDROID_HOME%
```

### 2. Runtime Issues

#### App crashes on startup
**Symptoms:** Application launches then immediately closes

**Solutions:**
1. Check MauiProgram.cs - ensure all services are registered:
```csharp
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ISyncService, SyncService>();
```

2. Verify database path is accessible:
```csharp
var dbPath = Path.Combine(FileSystem.AppDataDirectory, "bmstock.db");
```

3. Check permissions in AndroidManifest.xml and Info.plist

#### "Unable to connect to Supabase"
**Symptoms:** Network/connectivity errors in logs

**Solutions:**
1. Verify credentials in appsettings.json:
```bash
# Check that URL and key are correct
# URL format: https://xxxx.supabase.co
# Key format: eyJhbGciOiJIUzI1NiIs...
```

2. Test connectivity:
```bash
# Test from terminal
curl -i https://your-project.supabase.co
```

3. Check firewall/proxy settings

4. Ensure Supabase project is active (not paused)

### 3. Database Issues

#### SQLite: "database is locked"
**Symptoms:** Concurrent access errors

**Solutions:**
```csharp
// Use async methods everywhere
var trucks = await _database.Table<Truck>().ToListAsync();

// NOT
var trucks = _database.Table<Truck>().ToList(); // Blocking
```

#### Missing tables in SQLite
**Symptoms:** Table not found errors

**Solutions:**
```csharp
// Ensure migrations run in MauiProgram.cs
private async Task InitializeDatabaseAsync()
{
    await _database.CreateTableAsync<Truck>();
    await _database.CreateTableAsync<Stock>();
    // etc...
}
```

### 4. Authentication Issues

#### JWT token expired
**Symptoms:** "Unauthorized" errors on API calls

**Solutions:**
```csharp
// Token auto-refresh should happen in AuthService
private async Task RefreshTokenAsync()
{
    var result = await _supabase.Auth.RefreshSession();
    if (result != null)
    {
        // Token refreshed successfully
    }
}
```

#### Cannot login
**Symptoms:** Login page stays loading

**Solutions:**
1. Verify email exists in Supabase Auth
2. Check password is correct
3. Ensure email is verified (if required)
4. Check Supabase Auth settings

### 5. UI Issues

#### Layout issues / Text not visible
**Symptoms:** Elements overlap or don't display

**Solutions:**
```xaml
<!-- Ensure proper column/row definitions -->
<Grid ColumnDefinitions="*,*" RowDefinitions="Auto,*">
    <!-- Proper layout -->
</Grid>

<!-- Check padding and margins -->
<VerticalStackLayout Padding="15" Spacing="15">
    <!-- Content -->
</VerticalStackLayout>
```

#### Binding not working
**Symptoms:** Data doesn't update in UI

**Solutions:**
1. Check BindingContext is set:
```csharp
BindingContext = viewModel;
```

2. Verify ViewModel implements INotifyPropertyChanged

3. Use SetProperty method in properties:
```csharp
public string Name
{
    get => _name;
    set => SetProperty(ref _name, value);
}
```

### 6. Offline Mode Issues

#### Sync not working
**Symptoms:** Changes don't sync to cloud after reconnection

**Solutions:**
```csharp
// Ensure sync is triggered on reconnection
Connectivity.ConnectivityChanged += async (sender, e) => {
    if (e.NetworkAccess == NetworkAccess.Internet)
    {
        await _syncService.SyncAsync();
    }
};
```

#### Conflict resolution
**Symptoms:** Data conflicts between local and cloud

**Solutions:**
1. Server version wins (default)
2. Or implement custom merge logic:
```csharp
private async Task MergeConflicts(LocalData local, CloudData cloud)
{
    if (local.UpdatedAt > cloud.UpdatedAt)
    {
        await _database.UpdateAsync(local);
    }
    else
    {
        await _database.UpdateAsync(cloud);
    }
}
```

### 7. Platform-Specific Issues

#### Android

**Issue:** App crashes on Android 12+
**Solution:** Update AndroidManifest.xml with required permissions:
```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
```

**Issue:** SQLite not found on Android
**Solution:** Ensure sqlite-net-pcl package is installed

#### iOS

**Issue:** "Developer Mode not enabled"
**Solution:** 
1. Go to Settings > Developer
2. Toggle Developer Mode ON
3. Restart device

**Issue:** ATS (App Transport Security) errors
**Solution:** Update Info.plist:
```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
    <key>NSExceptionDomains</key>
    <dict>
        <key>supabase.co</key>
        <dict>
            <key>NSIncludesSubdomains</key>
            <true/>
        </dict>
    </dict>
</dict>
```

#### Windows

**Issue:** WebView2 not installed
**Solution:** Install from https://developer.microsoft.com/en-us/microsoft-edge/webview2/

### 8. Performance Issues

#### App is slow
**Solutions:**
1. Use pagination for large lists:
```csharp
var trucks = await _database
    .Table<Truck>()
    .Skip(0)
    .Take(20)
    .ToListAsync();
```

2. Use virtualization in XAML:
```xaml
<CollectionView ItemsSource="{Binding Items}" SelectionMode="Single">
    <!-- CollectionView already virtualizes -->
</CollectionView>
```

3. Implement caching for API calls

#### Memory leaks
**Solutions:**
1. Unsubscribe from events:
```csharp
Connectivity.ConnectivityChanged -= OnConnectivityChanged;
```

2. Dispose resources properly:
```csharp
public void Dispose()
{
    _database?.CloseAsync();
}
```

### 9. Logging and Debugging

#### Enable debug logging
```csharp
var loggerFactory = new LoggerFactory()
    .AddConsole();

builder.Services.AddLogging(configure => {
    configure.AddConsole();
});
```

#### View logs
```bash
# Android
adb logcat | grep "BM-Business-Stock"

# iOS
xcode logs (Device window)
```

### 10. Getting Help

**Documentation:**
- MAUI: https://learn.microsoft.com/dotnet/maui
- Supabase: https://supabase.com/docs
- Stack Overflow: Tag with [.net-maui] [supabase]

**GitHub Issues:**
- https://github.com/kitengefabien-gif/BM-Business-Stock-Management/issues

**Contact:**
- Email: kitengefabien@gmail.com

---

## Tips

1. Always use `async/await` for I/O operations
2. Test offline mode regularly
3. Keep dependencies updated
4. Monitor logs for warnings
5. Use structured logging with correlation IDs
6. Test on real devices, not just emulators
7. Profile memory and performance regularly

