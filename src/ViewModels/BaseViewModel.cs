using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BMBusinessStockManagement.ViewModels
{
    /// <summary>
    /// Classe de base pour tous les ViewModels
    /// Implémente INotifyPropertyChanged pour le binding XAML
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private string _title;
        private bool _isBusy;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
