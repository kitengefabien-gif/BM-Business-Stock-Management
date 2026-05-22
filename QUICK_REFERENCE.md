# Quick Reference

## Project Commands

```bash
# Restore dependencies
dotnet restore

# Run on Windows
dotnet maui run -f net8.0-windows

# Run on Android
dotnet maui run -f net8.0-android

# Run on iOS
dotnet maui run -f net8.0-ios

# Run tests
dotnet test

# Build release
dotnet maui build -f net8.0-windows -c Release

# Publish
dotnet maui publish -f net8.0-android -c Release
```

## File Structure

```
src/
├── Models/                    # Data models
├── Services/
│   ├── Interfaces/           # Service contracts
│   └── Implementation/        # Service implementations
├── ViewModels/               # MVVM ViewModels
├── Views/                    # XAML Pages
├── Converters.cs             # XAML converters
├── Extensions.cs             # Extension methods
├── App.xaml(.cs)            # Application
├── AppShell.xaml(.cs)       # Navigation
└── MauiProgram.cs           # DI Configuration

sql/
├── 001_initial_schema.sql    # Initial tables
├── 002_stock_schema.sql      # Stock tables
├── 003_finance_schema.sql    # Finance tables
└── 004_notification_schema.sql # Notifications

├── README.md                 # Project overview
├── CONTRIBUTING.md           # Contribution guide
├── DEPLOYMENT.md             # Deployment guide
├── TROUBLESHOOTING.md        # Troubleshooting
├── API_DOCUMENTATION.md      # API reference
└── appsettings.json          # Configuration
```

## Key Classes

**Models:**
- User - User account
- Truck - Truck/voyage entry
- Stock - Inventory
- FinancialTransaction - Financial record
- Notification - System notification

**Services:**
- AuthService - Authentication
- TruckService - Truck operations
- StockService - Inventory management
- FinanceService - Financial operations
- SyncService - Offline/online sync

**ViewModels:**
- LoginViewModel - Authentication flow
- DashboardViewModel - Main screen
- TruckFormViewModel - Truck entry

## Configuration

**appsettings.json:**
```json
{
  "Supabase": {
    "Url": "https://your-project.supabase.co",
    "AnonKey": "your-anon-key"
  },
  "Features": {
    "EnableOfflineMode": true,
    "EnableRealtimeSync": true
  }
}
```

## Common Patterns

**MVVM Pattern:**
```csharp
public class MyViewModel : BaseViewModel
{
    private string _property;
    
    public string Property
    {
        get => _property;
        set => SetProperty(ref _property, value);
    }
}
```

**Async Operations:**
```csharp
public async Task LoadDataAsync()
{
    IsLoading = true;
    try
    {
        Data = await _service.GetDataAsync();
    }
    finally
    {
        IsLoading = false;
    }
}
```

**Dependency Injection:**
```csharp
builder.Services.AddSingleton<IService, Service>();
var service = serviceProvider.GetRequiredService<IService>();
```

