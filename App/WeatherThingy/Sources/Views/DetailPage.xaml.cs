using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using WeatherThingy.Sources.ViewModels;
using static WeatherThingy.Sources.ViewModels.HomeViewModel;
using Microsoft.Maui.Controls;  // For ContentPage, etc.
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq.Expressions;

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
            currentlyShowing = "humidity";
            ViewModel.SelectedParameter = "Humidity";
            start = DateTime.Now.AddDays(-1);
            end = DateTime.Now;
            prevClickedTimeDuration = OneDayButton;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<DetailViewModel, string>(this, "NoSensorLabel", async (sender, message) =>
            {
                await DisplayAlert("Some sensors are missing!", message, "OK");
            });

            MessagingCenter.Subscribe<DetailViewModel, string>(this, "TimeIssues", async (sender, message) =>
            {
                await DisplayAlert("Some data is missing!", message, "OK");
            });

            MessagingCenter.Subscribe<DetailViewModel, string>(this, "FailedToFetchData", async (sender, message) =>
            {
                await DisplayAlert("No data to display", message , "OK");
                GraphTitle.IsVisible = false;
                NoGraph.IsVisible = true;
            });

            await ViewModel.InitializePlotsAsync(); // Ensure that the nodes are initialized

            if (!ViewModel.AvailableNodes.Any()) // Check if there are any available nodes
            {
                GraphTitle.IsVisible = false;
                NoGraph.IsVisible = true;
                await DisplayAlert("No Data", "No available nodes found. Please make sure you have a stable internet connection.", "OK");

            }
            else
            {
                GraphTitle.IsVisible = true;
                NoGraph.IsVisible = false;
                await UpdateChart();
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<DetailViewModel, string>(this, "NoSensorLabel");
            MessagingCenter.Unsubscribe<DetailViewModel, string>(this, "FailedToFetchData");
            MessagingCenter.Unsubscribe<DetailViewModel, string>(this, "TimeIssues");
        }

        private async Task UpdateChart()
        {
            if (ViewModel != null)
            {
                GraphTitle.IsVisible = true;
                NoGraph.IsVisible = false;

                Chart.IsVisible = false;
                
                await ViewModel.ShowDataAsync(start, end, currentlyShowing, nodeIds);

                RebuildAxes();
                Chart.CoreChart.Update();

                Chart.IsVisible = true;
            }
        }

        
        private void RebuildAxes()
        {
            // Rebuild the X-axis
            Axis[] XAxes = new Axis[]
        {
                new Axis
                {
                    //MaxLimit = null, MinLimit = null,

                    Labeler = value =>
                    {
                        if (value < DateTime.MinValue.Ticks || value > DateTime.MaxValue.Ticks)
                        {
                            return "0";
                        }
                        var dateTime = new DateTime((long)value); // Convert long (ticks) to DateTime
                        return dateTime.ToString("dd-MM HH:mm");    // Display only day, hour, minute
                    },
                    UnitWidth = TimeSpan.FromMinutes(1).Ticks, // Adjust the spacing for your data
                    LabelsRotation = 120
                } };


            string y_value;

            if (currentlyShowing.Contains("ressure")) y_value = currentlyShowing + " hPa";
            else if (currentlyShowing.Contains("door")) y_value = currentlyShowing + " °C";
            else y_value = currentlyShowing + " %";

            // Optionally, rebuild Y-axis if needed
            Axis[] YAxes =
                    {
                new Axis
                {
                    Name = y_value,
                    NamePadding = new LiveChartsCore.Drawing.Padding(0, 15),
                }
            };

            // Bind the rebuilt axes to the chart
            Chart.XAxes = XAxes;
            Chart.YAxes = YAxes;
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

                if (start.Date == end.Date)
                {
                    start = start.Date;
                    end = end.Date.AddHours(23).AddMinutes(59);
                }

                if (start <= end)
                {
                    await UpdateChart();
                }
                else
                {
                    await DisplayAlert("Invalid time period", "The end date has to be bigger than start date", "OK");
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
