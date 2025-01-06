using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		BindingContext = new AboutViewModel();
	}
}