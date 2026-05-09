using BMBusinessStockManagement.ViewModels;

namespace BMBusinessStockManagement.Views;

public partial class TruckFormPage : ContentPage
{
    public TruckFormPage(TruckFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
