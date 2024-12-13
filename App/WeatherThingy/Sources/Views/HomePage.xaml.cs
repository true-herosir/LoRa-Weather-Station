using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}