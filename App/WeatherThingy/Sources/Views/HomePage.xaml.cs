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

    private void OnPointerEntered(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Color.FromArgb("#005BB5"); // Change background on hover
        }
    }

    private void OnPointerExited(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            button.BackgroundColor = Color.FromArgb("#007AFF"); // Revert background
        }
    }

    private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {

    }
}