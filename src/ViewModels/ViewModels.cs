using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BMBusinessStockManagement.Models;
using BMBusinessStockManagement.Services;

namespace BMBusinessStockManagement.ViewModels
{
    /// <summary>
    /// ViewModel pour la page de connexion
    /// Pattern MVVM
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _email;
        private string _password;
        private bool _isLoading;
        private string _errorMessage;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new Command(async () => await OnLoginAsync(), CanLogin);
        }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) 
                && !string.IsNullOrWhiteSpace(Password) 
                && !IsLoading;
        }

        private async Task OnLoginAsync()
        {
            if (!CanLogin())
                return;

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _authService.LoginAsync(Email, Password);

                if (result.Success)
                {
                    // Navigation vers le dashboard
                    await Shell.Current.GoToAsync($"/{nameof(Views.Dashboard)}");
                }
                else
                {
                    ErrorMessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// ViewModel pour le tableau de bord principal
    /// </summary>
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ITruckService _truckService;
        private readonly IFinanceService _financeService;
        private readonly ISyncService _syncService;
        
        private ObservableCollection<Truck> _trucks;
        private Truck _selectedTruck;
        private int _totalTrips;
        private decimal _totalRevenue;
        private decimal _totalCosts;
        private decimal _profit;
        private bool _isRefreshing;

        public ObservableCollection<Truck> Trucks
        {
            get => _trucks;
            set => SetProperty(ref _trucks, value);
        }

        public Truck SelectedTruck
        {
            get => _selectedTruck;
            set => SetProperty(ref _selectedTruck, value);
        }

        public int TotalTrips
        {
            get => _totalTrips;
            set => SetProperty(ref _totalTrips, value);
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

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddTruckCommand { get; }
        public ICommand EditTruckCommand { get; }
        public ICommand DeleteTruckCommand { get; }

        public DashboardViewModel(
            ITruckService truckService,
            IFinanceService financeService,
            ISyncService syncService)
        {
            _truckService = truckService ?? throw new ArgumentNullException(nameof(truckService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));

            Trucks = new ObservableCollection<Truck>();

            RefreshCommand = new Command(async () => await LoadDataAsync());
            AddTruckCommand = new Command(async () => await AddTruckAsync());
            EditTruckCommand = new Command<Truck>(async (truck) => await EditTruckAsync(truck));
            DeleteTruckCommand = new Command<Truck>(async (truck) => await DeleteTruckAsync(truck));
        }

        public async Task LoadDataAsync()
        {
            IsRefreshing = true;

            try
            {
                var trucks = await _truckService.GetAllTrucksAsync();
                Trucks = new ObservableCollection<Truck>(trucks);
                TotalTrips = trucks.Count;

                var today = DateTime.UtcNow.Date;
                var revenue = await _financeService.CalculateTotalRevenueAsync(today, today.AddDays(1));
                var costs = await _financeService.CalculateTotalCostsAsync(today, today.AddDays(1));

                TotalRevenue = revenue;
                TotalCosts = costs;
                Profit = revenue - costs;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task AddTruckAsync()
        {
            await Shell.Current.GoToAsync($"/{nameof(Views.TruckForm)}");
        }

        private async Task EditTruckAsync(Truck truck)
        {
            if (truck == null) return;
            await Shell.Current.GoToAsync($"/{nameof(Views.TruckForm)}?id={truck.Id}");
        }

        private async Task DeleteTruckAsync(Truck truck)
        {
            if (truck == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmation",
                $"Êtes-vous sûr de vouloir supprimer le voyage de {truck.DriverName}?",
                "Oui", "Non");

            if (confirm)
            {
                await _truckService.DeleteTruckAsync(truck.Id);
                await LoadDataAsync();
            }
        }
    }

    /// <summary>
    /// ViewModel pour le formulaire d'enregistrement de camions
    /// </summary>
    public class TruckFormViewModel : BaseViewModel
    {
        private readonly ITruckService _truckService;
        private Truck _truck;
        private string _selectedPlate;
        private string _selectedPhone;
        private string _selectedDriverName;
        private ObservableCollection<string> _plateSuggestions;
        private ObservableCollection<string> _phoneSuggestions;
        private ObservableCollection<string> _driverNameSuggestions;
        private bool _isLoading;

        public Truck Truck
        {
            get => _truck;
            set => SetProperty(ref _truck, value);
        }

        public string SelectedPlate
        {
            get => _selectedPlate;
            set
            {
                SetProperty(ref _selectedPlate, value);
                LoadPlateSuggestionsAsync(value).FireAndForget();
            }
        }

        public string SelectedPhone
        {
            get => _selectedPhone;
            set
            {
                SetProperty(ref _selectedPhone, value);
                LoadPhoneSuggestionsAsync(value).FireAndForget();
            }
        }

        public string SelectedDriverName
        {
            get => _selectedDriverName;
            set
            {
                SetProperty(ref _selectedDriverName, value);
                LoadDriverNameSuggestionsAsync(value).FireAndForget();
            }
        }

        public ObservableCollection<string> PlateSuggestions
        {
            get => _plateSuggestions;
            set => SetProperty(ref _plateSuggestions, value);
        }

        public ObservableCollection<string> PhoneSuggestions
        {
            get => _phoneSuggestions;
            set => SetProperty(ref _phoneSuggestions, value);
        }

        public ObservableCollection<string> DriverNameSuggestions
        {
            get => _driverNameSuggestions;
            set => SetProperty(ref _driverNameSuggestions, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TruckFormViewModel(ITruckService truckService)
        {
            _truckService = truckService ?? throw new ArgumentNullException(nameof(truckService));
            
            Truck = new Truck();
            PlateSuggestions = new ObservableCollection<string>();
            PhoneSuggestions = new ObservableCollection<string>();
            DriverNameSuggestions = new ObservableCollection<string>();

            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task LoadPlateSuggestionsAsync(string searchTerm)
        {
            var suggestions = await _truckService.GetLicensePlateHistoryAsync(searchTerm);
            PlateSuggestions = new ObservableCollection<string>(suggestions.Take(5));
        }

        private async Task LoadPhoneSuggestionsAsync(string searchTerm)
        {
            var suggestions = await _truckService.GetPhoneNumberHistoryAsync(searchTerm);
            PhoneSuggestions = new ObservableCollection<string>(suggestions.Take(5));
        }

        private async Task LoadDriverNameSuggestionsAsync(string searchTerm)
        {
            var suggestions = await _truckService.GetDriverNameHistoryAsync(searchTerm);
            DriverNameSuggestions = new ObservableCollection<string>(suggestions.Take(5));
        }

        private async Task SaveAsync()
        {
            IsLoading = true;

            try
            {
                Truck.LicensePlate = SelectedPlate;
                Truck.PhoneNumber = SelectedPhone;
                Truck.DriverName = SelectedDriverName?.ToUpper();

                if (string.IsNullOrEmpty(Truck.Id))
                    await _truckService.CreateTruckAsync(Truck);
                else
                    await _truckService.UpdateTruckAsync(Truck);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
