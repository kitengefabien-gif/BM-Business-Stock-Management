using BMBusinessStockManagement.Models;
using BMBusinessStockManagement.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMBusinessStockManagement.ViewModels
{
    /// <summary>
    /// ViewModel pour l'enregistrement des camions
    /// Gère le formulaire avec auto-complétion
    /// </summary>
    public class TruckFormViewModel : BaseViewModel
    {
        private readonly ITruckService _truckService;
        private readonly ISyncService _syncService;
        
        private string _driverName;
        private string _licensePlate;
        private string _phoneNumber;
        private string _countryCode = "CD";
        private TruckDestination _selectedDestination;
        private CementType _selectedCementType;
        private int _cementQuantity;
        private decimal _travelAmount;
        private decimal _transportCost;
        
        private ObservableCollection<string> _licensePlateSuggestions;
        private ObservableCollection<string> _phoneNumberSuggestions;
        private ObservableCollection<string> _driverNameSuggestions;
        
        private ICommand _saveTruckCommand;
        private ICommand _validatePhoneCommand;

        public TruckFormViewModel(ITruckService truckService, ISyncService syncService)
        {
            _truckService = truckService ?? throw new ArgumentNullException(nameof(truckService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            
            Title = "Enregistrement Camion";
            _licensePlateSuggestions = new ObservableCollection<string>();
            _phoneNumberSuggestions = new ObservableCollection<string>();
            _driverNameSuggestions = new ObservableCollection<string>();
        }

        #region Properties

        public string DriverName
        {
            get => _driverName;
            set
            {
                SetProperty(ref _driverName, value?.ToUpper());
                LoadDriverNameSuggestionsAsync(value).FireAndForget();
            }
        }

        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                SetProperty(ref _licensePlate, value);
                LoadLicensePlateSuggestionsAsync(value).FireAndForget();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                SetProperty(ref _phoneNumber, value);
                LoadPhoneNumberSuggestionsAsync(value).FireAndForget();
            }
        }

        public string CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }

        public TruckDestination SelectedDestination
        {
            get => _selectedDestination;
            set => SetProperty(ref _selectedDestination, value);
        }

        public CementType SelectedCementType
        {
            get => _selectedCementType;
            set => SetProperty(ref _selectedCementType, value);
        }

        public int CementQuantity
        {
            get => _cementQuantity;
            set => SetProperty(ref _cementQuantity, value);
        }

        public decimal TravelAmount
        {
            get => _travelAmount;
            set => SetProperty(ref _travelAmount, value);
        }

        public decimal TransportCost
        {
            get => _transportCost;
            set => SetProperty(ref _transportCost, value);
        }

        public ObservableCollection<string> LicensePlateSuggestions
        {
            get => _licensePlateSuggestions;
            set => SetProperty(ref _licensePlateSuggestions, value);
        }

        public ObservableCollection<string> PhoneNumberSuggestions
        {
            get => _phoneNumberSuggestions;
            set => SetProperty(ref _phoneNumberSuggestions, value);
        }

        public ObservableCollection<string> DriverNameSuggestions
        {
            get => _driverNameSuggestions;
            set => SetProperty(ref _driverNameSuggestions, value);
        }

        public ICommand SaveTruckCommand =>
            _saveTruckCommand ??= new AsyncCommand(OnSaveTruckAsync, CanSaveTruck);

        public ICommand ValidatePhoneCommand =>
            _validatePhoneCommand ??= new AsyncCommand(OnValidatePhoneAsync);

        #endregion

        #region Methods

        private bool CanSaveTruck()
        {
            return !string.IsNullOrWhiteSpace(DriverName) &&
                   !string.IsNullOrWhiteSpace(LicensePlate) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   CementQuantity > 0 &&
                   IsValidPhoneNumber(PhoneNumber) &&
                   !IsBusy;
        }

        private async Task OnSaveTruckAsync()
        {
            IsBusy = true;

            try
            {
                // Vérifier si la plaque existe déjà
                if (await _truckService.LicensePlateExistsAsync(LicensePlate))
                {
                    await App.Current.MainPage.DisplayAlert("Erreur", 
                        "Cette plaque minéralogique existe déjà", "OK");
                    return;
                }

                var truck = new Truck
                {
                    DriverName = DriverName,
                    LicensePlate = LicensePlate,
                    PhoneNumber = PhoneNumber,
                    CountryCode = CountryCode,
                    Destination = SelectedDestination,
                    CementType = SelectedCementType,
                    CementQuantity = CementQuantity,
                    TravelAmount = TravelAmount,
                    TransportCost = TransportCost
                };

                var result = await _truckService.CreateTruckAsync(truck);
                
                await App.Current.MainPage.DisplayAlert("Succès", 
                    $"Camion enregistré avec le numéro de voyage: {result.VoyageNumber}", "OK");
                
                // Réinitialiser le formulaire
                ResetForm();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnValidatePhoneAsync()
        {
            if (IsValidPhoneNumber(PhoneNumber))
            {
                await App.Current.MainPage.DisplayAlert("Valide", "Numéro de téléphone valide", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Invalide", 
                    $"Format attendu: +{(CountryCode == "CD" ? "243" : "260")} XXXXXXXXX", "OK");
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Format: +243 XXXXXXXXX ou +260 XXXXXXXXX
            var prefix = CountryCode == "CD" ? "+243" : "+260";
            return phoneNumber.StartsWith(prefix) && phoneNumber.Length == prefix.Length + 9;
        }

        private async Task LoadLicensePlateSuggestionsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            var suggestions = await _truckService.GetLicensePlateHistoryAsync(searchTerm);
            LicensePlateSuggestions = new ObservableCollection<string>(suggestions);
        }

        private async Task LoadPhoneNumberSuggestionsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            var suggestions = await _truckService.GetPhoneNumberHistoryAsync(searchTerm);
            PhoneNumberSuggestions = new ObservableCollection<string>(suggestions);
        }

        private async Task LoadDriverNameSuggestionsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            var suggestions = await _truckService.GetDriverNameHistoryAsync(searchTerm);
            DriverNameSuggestions = new ObservableCollection<string>(suggestions);
        }

        private void ResetForm()
        {
            DriverName = string.Empty;
            LicensePlate = string.Empty;
            PhoneNumber = string.Empty;
            CementQuantity = 0;
            TravelAmount = 0;
            TransportCost = 0;
            CountryCode = "CD";
        }

        #endregion
    }
}
