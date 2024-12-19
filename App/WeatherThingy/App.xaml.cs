using WeatherThingy.Pages;
using WeatherThingy.Sources.Views;

namespace WeatherThingy
{
    public partial class App : Application
    {
     

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
