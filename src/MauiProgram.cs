using BMBusinessStockManagement.Services;
using BMBusinessStockManagement.Views;
using BMBusinessStockManagement.ViewModels;
using Supabase;

namespace BMBusinessStockManagement;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Configuration Supabase
        var supabaseUrl = "YOUR_SUPABASE_URL";
        var supabaseKey = "YOUR_SUPABASE_ANON_KEY";
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };
        var supabase = new Client(supabaseUrl, supabaseKey, options);
        await supabase.InitializeAsync();

        // Dependency Injection
        builder.Services.AddSingleton(supabase);
        
        // Services
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ISyncService, SyncService>();
        builder.Services.AddSingleton<ITruckService, TruckService>();
        builder.Services.AddSingleton<IStockService, StockService>();
        builder.Services.AddSingleton<IFinanceService, FinanceService>();

        // ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<TruckFormViewModel>();

        // Views
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<TruckFormPage>();

        // App Shell
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
