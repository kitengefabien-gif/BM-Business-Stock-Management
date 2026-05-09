using BMBusinessStockManagement.Models;
using BMBusinessStockManagement.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMBusinessStockManagement.ViewModels
{
    /// <summary>
    /// ViewModel pour la page de connexion
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private string _email;
        private string _password;
        private string _errorMessage;
        private ICommand _loginCommand;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            Title = "BM BUSINESS STOCK MANAGEMENT";
        }

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand =>
            _loginCommand ??= new AsyncCommand(OnLoginAsync, CanExecuteLogin);

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsBusy;
        }

        private async Task OnLoginAsync()
        {
            if (!CanExecuteLogin())
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _authService.LoginAsync(Email, Password);
                
                if (result.Success)
                {
                    // Naviguer vers le dashboard
                    await Shell.Current.GoToAsync($"dashboard");
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
                IsBusy = false;
            }
        }
    }

    /// <summary>
    /// Commande asynchrone pour les opérations longues
    /// </summary>
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            _isExecuting = true;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
