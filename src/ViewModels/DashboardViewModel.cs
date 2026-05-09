using BMBusinessStockManagement.Models;
using BMBusinessStockManagement.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BMBusinessStockManagement.ViewModels
{
    /// <summary>
    /// ViewModel pour le tableau de bord principal
    /// </summary>
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ITruckService _truckService;
        private readonly IFinanceService _financeService;
        private readonly IAuthService _authService;
        private readonly ISyncService _syncService;
        
        private ObservableCollection<Truck> _trucks;
        private decimal _totalRevenue;
        private decimal _totalCosts;
        private decimal _profit;
        private int _totalTrips;
        private User _currentUser;

        public DashboardViewModel(ITruckService truckService, IFinanceService financeService, 
                                 IAuthService authService, ISyncService syncService)
        {
            _truckService = truckService ?? throw new ArgumentNullException(nameof(truckService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            
            Title = "Tableau de Bord";
            _trucks = new ObservableCollection<Truck>();
            
            // S'abonner aux événements de synchronisation
            _syncService.OnSyncCompleted += OnSyncCompleted;
        }

        public ObservableCollection<Truck> Trucks
        {
            get => _trucks;
            set => SetProperty(ref _trucks, value);
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public decimal TotalCosts
        {
            get => _totalCosts;
            set => SetProperty(ref _totalCosts, value);
        }

        public decimal Profit
        {
            get => _profit;
            set => SetProperty(ref _profit, value);
        }

        public int TotalTrips
        {
            get => _totalTrips;
            set => SetProperty(ref _totalTrips, value);
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;
            try
            {
                CurrentUser = await _authService.GetCurrentUserAsync();
                await LoadTrucksAsync();
                await LoadFinancialDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadTrucksAsync()
        {
            var trucks = await _truckService.GetAllTrucksAsync();
            Trucks = new ObservableCollection<Truck>(trucks);
            TotalTrips = trucks.Count;
        }

        private async Task LoadFinancialDataAsync()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var endDate = DateTime.UtcNow;
            
            TotalRevenue = await _financeService.CalculateTotalRevenueAsync(startDate, endDate);
            TotalCosts = await _financeService.CalculateTotalCostsAsync(startDate, endDate);
            Profit = await _financeService.CalculateProfitAsync(startDate, endDate);
        }

        private void OnSyncCompleted(object sender, SyncResult result)
        {
            if (result.Success)
            {
                LoadDataAsync().FireAndForget();
            }
        }
    }
}
