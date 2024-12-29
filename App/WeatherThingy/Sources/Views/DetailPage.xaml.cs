using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using WeatherThingy.Sources.ViewModels;
using static WeatherThingy.Sources.ViewModels.HomeViewModel;

namespace WeatherThingy.Sources.Views
{
    public partial class DetailPage : ContentPage
    {
        private DetailViewModel ViewModel => BindingContext as DetailViewModel;
        private List<string> nodeIds { get; } = new();
        private string currentlyShowing;
        private Button prevClickedTimeDuration;
        private DateTime start;
        private DateTime end;

        public DetailPage()
        {
            InitializeComponent();
            BindingContext = new DetailViewModel();
            // Initializing the default graph to be shown (humidity for one day only for lht gronau sensor)
            nodeIds.Add("lht-gronau");
            currentlyShowing = "humidity";
            ViewModel.SelectedParameter = "Humidity";
            start = DateTime.Now.AddDays(-1);
            end = DateTime.Now;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Trigger data fetching when the page appears
            prevClickedTimeDuration = OneDayButton;
            await UpdateChart();
        }

        private async Task UpdateChart()
        {
            if (ViewModel != null)
            {
                await ViewModel.ShowDataAsync(start, end, currentlyShowing, nodeIds);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;
            }
        }

        private async void OnTimeDurationClicked(object sender, EventArgs e, int days)
        {
            start = DateTime.Now.AddDays(-days);
            end = DateTime.Now;

            ChangeButtonColors(prevClickedTimeDuration, false);
            ChangeButtonColors((Button)sender, true);
            prevClickedTimeDuration = (Button)sender;

            await UpdateChart();
        }

        private void OnDayDataClicked(object sender, EventArgs e) => OnTimeDurationClicked(sender, e, 1);
        private void OnThreeDayDataClicked(object sender, EventArgs e) => OnTimeDurationClicked(sender, e, 3);
        private void OnWeekDataClicked(object sender, EventArgs e) => OnTimeDurationClicked(sender, e, 7);
        private void OnTwoWeekDataClicked(object sender, EventArgs e) => OnTimeDurationClicked(sender, e, 14);
        private void OnMonthDataClicked(object sender, EventArgs e) => OnTimeDurationClicked(sender, e, 30);

        private async void OnCustomRangeClicked(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                start = ViewModel.LowDate;
                end = ViewModel.HighDate;

                ChangeButtonColors(prevClickedTimeDuration, false);
                ChangeButtonColors((Button)sender, true);
                prevClickedTimeDuration = (Button)sender;

                if (start <= end)
                {
                    await UpdateChart();
                }
                else
                {
                    // TODO: ADD LABEL FOR ERROR TEXT AND BIND IT
                }
            }
        }

        private async void OnParameterDisplayClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (!string.IsNullOrEmpty(currentlyShowing))
                {
                    var prevButton = this.FindByName<Button>(currentlyShowing);
                    if (prevButton != null)
                    {
                        ChangeButtonColors(prevButton, false);
                    }
                }
                currentlyShowing = clickedButton.StyleId;
                ViewModel.SelectedParameter = clickedButton.Text;
                ChangeButtonColors(clickedButton, true);
                await UpdateChart();
            }
        }

        private async void OnNodeButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                if (nodeIds.Contains(clickedButton.Text))
                {
                    ChangeButtonColors(clickedButton, false);
                    nodeIds.Remove(clickedButton.Text);
                }
                else
                {
                    ChangeButtonColors(clickedButton, true);
                    nodeIds.Add(clickedButton.Text);
                }

                await UpdateChart();
            }
        }

        private void ChangeButtonColors(Button button, bool isSelected)
        {
            if (Application.Current?.Resources != null)
            {
                button.BackgroundColor = (Color)Application.Current.Resources[isSelected ? "ButtonPressed" : "Button"];
            }
        }
    }
}
