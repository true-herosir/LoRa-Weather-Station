using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
            BindingContext = new DetailViewModel();
        }

        private void OnPointerEntered(object sender, EventArgs e)
        {
            if (sender is Button button) button.Scale = 1.10;
        }

        private void OnPointerExited(object sender, EventArgs e)
        {
            if (sender is Button button) button.Scale = 1;
        }

        // Example: Trigger ShowData when the page is loaded or when a button is clicked
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Trigger data fetching when the page appears
            if (BindingContext is DetailViewModel viewModel)
            {
                //viewModel.ShowData();
            }
        }

        // Alternatively, you can bind it to a button click
        private async void OnDayDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {

                //Chart.Series = null; // Clear existing series
                var start = DateTime.Now.AddDays(-1);
                var end = DateTime.Now;

                Chart.IsVisible = false;
                await viewModel.ShowData(start, end);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;

            }
        }

        private async void OnWeekDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {

                //Chart.Series = null; // Clear existing series
                var start = DateTime.Now.AddDays(-7);
                var end = DateTime.Now;

                Chart.IsVisible = false;
                await viewModel.ShowData(start, end);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;

            }
        }
    }
}
