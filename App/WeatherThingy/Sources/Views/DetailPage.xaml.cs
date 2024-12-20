using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views
{
    public partial class DetailPage : ContentPage
    {
        public List<string> node_ids { get; set; } = new() { "lht-gronau", "mkr-saxion" };
        public DetailPage()
        {
            InitializeComponent();
            BindingContext = new DetailViewModel();
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
            //Chart.Series = null; // Clear existing series
            var start = DateTime.Now.AddDays(-1);
            var end = DateTime.Now;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnWeekDataClicked(object sender, EventArgs e)
        {
            //Chart.Series = null; // Clear existing series
            var start = DateTime.Now.AddDays(-7);
            var end = DateTime.Now;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnThreeDayDataClicked(object sender, EventArgs e)
        {
            //Chart.Series = null; // Clear existing series
            var start = DateTime.Now.AddDays(-3);
            var end = DateTime.Now;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnTwoWeekDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                //Chart.Series = null; // Clear existing series
                var start = DateTime.Now.AddDays(-14);
                var end = DateTime.Now;

                await OnDataExtract(sender, e, start, end);

            }
        }
        private async void OnMonthDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                var start = DateTime.Now.AddMonths(-1);
                var end = DateTime.Now;

                await OnDataExtract(sender, e, start, end);

            }
        }

        private async void OnCustomRangeClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                var start = viewModel.LowDate;
                var end = viewModel.HighDate;
                if (start > end) { return; } //TODO: ADD LABEL FOR ERROR TEXT AND BIND IT
                await OnDataExtract(sender, e, start, end);

            }
        }
        private async Task OnDataExtract(object sender, EventArgs e, DateTime start, DateTime end)
        {
            if (BindingContext is DetailViewModel viewModel)
            {

                //Chart.Series = null; // Clear existing series
                Chart.IsVisible = false;
                await viewModel.ShowData(start, end, "humidity", node_ids);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;

            }
        }
    }
}
