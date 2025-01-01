using LiveChartsCore;
using WeatherThingy.Sources.Services;
using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        // Create an instance of the service
        var weatherThingyService = new WeatherThingyService();
        BindingContext = new HomeViewModel(weatherThingyService);
    }
}