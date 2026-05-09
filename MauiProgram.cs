using CommunityToolkit.Mvvm;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Supabase;

namespace BM.Business.StockManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp
                .CreateBuilder()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureServices();

            return builder.Build();
        }

        private static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            // Register Services
            builder.Services.AddSingleton<Supabase.Client>(provider =>
            {
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = true,
                    AutoRefreshToken = true,
                    PersistSession = true,
                };

                var url = SecureStorage.GetAsync("supabase_url").Result;
                var key = SecureStorage.GetAsync("supabase_key").Result;

                return new Supabase.Client(url ?? "", key ?? "", options);
            });

            // Register ViewModels
            builder.Services.AddSingleton<ViewModels.LoginViewModel>();
            builder.Services.AddSingleton<ViewModels.DashboardViewModel>();
            builder.Services.AddSingleton<ViewModels.TruckFormViewModel>();
            builder.Services.AddSingleton<ViewModels.StockManagementViewModel>();
            builder.Services.AddSingleton<ViewModels.FinanceDashboardViewModel>();

            // Register Views
            builder.Services.AddSingleton<Views.LoginPage>();
            builder.Services.AddSingleton<Views.DashboardPage>();
            builder.Services.AddSingleton<Views.TruckFormPage>();
            builder.Services.AddSingleton<Views.StockManagementPage>();
            builder.Services.AddSingleton<Views.FinanceDashboardPage>();

            // Register Business Services
            builder.Services.AddSingleton<Services.Auth.AuthService>();
            builder.Services.AddSingleton<Services.Truck.TruckService>();
            builder.Services.AddSingleton<Services.Stock.StockService>();
            builder.Services.AddSingleton<Services.Finance.FinanceService>();
            builder.Services.AddSingleton<Services.Sync.SyncService>();
            builder.Services.AddSingleton<Services.Notification.NotificationService>();

            // Register Data Services
            builder.Services.AddSingleton<Data.SQLite.DatabaseService>();
            builder.Services.AddSingleton<Data.Cloud.SupabaseService>();

            return builder;
        }
    }
}
