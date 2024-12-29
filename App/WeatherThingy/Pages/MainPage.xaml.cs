//using Microsoft.Maui.Controls;

using WeatherThingy.Sources.Views;

namespace WeatherThingy.Pages
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

        private void OnPointerEntered(object sender, EventArgs e)
        {
            if (sender is ImageButton im_button) im_button.Scale = 1.01;
            if (sender is Button button) button.BackgroundColor = Color.FromArgb("#415679");
            if (sender is HorizontalStackLayout h_layout) h_layout.BackgroundColor = Color.FromArgb("#415679");
        }

        private void OnPointerExited(object sender, EventArgs e)
        {
            if (sender is ImageButton im_button) im_button.Scale = 1.00;
            if (sender is Button button) button.BackgroundColor = Color.FromArgb("#4A628A");
            if (sender is HorizontalStackLayout h_layout) h_layout.BackgroundColor = Color.FromArgb("#4A628A");
        }

        private async void OnEnschedeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EnschedePage());
        }

        //private async void OnGronauButtonClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new DetailPage());
        //}

        private async void OnWierdenButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WierdenPage());
        }

        //private async void OnEnschedeClicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new EnschedePage());
        //}

        //private async void OnGronauClicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new GronauPage());
        //}

        //private async void OnWeirdenClicked(object sender, EventArgs e)
        //{
        //    //await Navigation.PushAsync(new WierdenPage());
        //}
    }

}
