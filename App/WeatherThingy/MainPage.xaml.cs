using Microsoft.Maui.Controls;

namespace WeatherThingy
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnExpandButtonClicked(object sender, EventArgs e)
        {
            if(!ExpandableContent.IsVisible) 
            {
                await AnimateMenu(200, !ExpandableContent.IsVisible);
                ExpandableContent.IsVisible = true;
                //await ExpandButton.RotateXTo(90); // magic trick XD
                await ExpandButton.RotateTo(90);
            }
            else
            {
                ExpandableContent.IsVisible = false;
                await AnimateMenu(0, ExpandableContent.IsVisible);
                await ExpandButton.RotateTo(0);

            }
        }
        private async Task AnimateMenu(int final_width, bool Expand)
        {
            double step = (250 / 10);
            while (MenuWidth.Width !=  final_width) 
            {
                if (Expand) MenuWidth.Width = MenuWidth.Width.Value + step;
                else MenuWidth.Width = MenuWidth.Width.Value - step;
                await Task.Delay(25);
            }
        }
    }

}
