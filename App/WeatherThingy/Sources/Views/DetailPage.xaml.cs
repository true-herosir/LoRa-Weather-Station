using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using WeatherThingy.Sources.ViewModels;
using static WeatherThingy.Sources.ViewModels.HomeViewModel;

namespace WeatherThingy.Sources.Views
{
    public partial class DetailPage : ContentPage
    {
        private List<string> node_ids { get; set; } = new();
        private string currently_showing { get; set; }

        private Button prev_clicked_time_duration { get; set; } = new();
        private DateTime start { get; set;}
        private DateTime end { get; set;}

        public DetailPage()
        {
            InitializeComponent();
            BindingContext = new DetailViewModel();
            //initialising the default graph to be shown (humidity for one day only for lht gronau sensor)
            node_ids.Add("lht-gronau");
            currently_showing = "humidity";
            start = DateTime.Now.AddDays(-1);
            end = DateTime.Now;
        }

        // Example: Trigger ShowData when the page is loaded or when a button is clicked
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Trigger data fetching when the page appears
            prev_clicked_time_duration = OneDayButton;
            if (BindingContext is DetailViewModel viewModel)
            {
                await viewModel.ShowData(start, end, currently_showing, node_ids);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;
            }
        }

        // Alternatively, you can bind it to a button click
        private async void OnDayDataClicked(object sender, EventArgs e)
        {
            start = DateTime.Now.AddDays(-1);
            end = DateTime.Now;

            ChangeButtonColours(prev_clicked_time_duration, false);
            ChangeButtonColours((Button)sender, true);
            prev_clicked_time_duration = (Button)sender;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnWeekDataClicked(object sender, EventArgs e)
        {
            //Chart.Series = null; // Clear existing series
            start = DateTime.Now.AddDays(-7);
            end = DateTime.Now;

            ChangeButtonColours(prev_clicked_time_duration, false);
            ChangeButtonColours((Button)sender, true);
            prev_clicked_time_duration = (Button)sender;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnThreeDayDataClicked(object sender, EventArgs e)
        {
            //Chart.Series = null; // Clear existing series
            start = DateTime.Now.AddDays(-3);
            end = DateTime.Now;

            ChangeButtonColours(prev_clicked_time_duration, false);
            ChangeButtonColours((Button)sender, true);
            prev_clicked_time_duration = (Button)sender;

            await OnDataExtract(sender, e, start, end);
        }

        private async void OnTwoWeekDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                //Chart.Series = null; // Clear existing series
                start = DateTime.Now.AddDays(-14);
                end = DateTime.Now;

                ChangeButtonColours(prev_clicked_time_duration, false);
                ChangeButtonColours((Button)sender, true);
                prev_clicked_time_duration = (Button)sender;

                await OnDataExtract(sender, e, start, end);

            }
        }
        private async void OnMonthDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                start = DateTime.Now.AddMonths(-1);
                end = DateTime.Now;

                ChangeButtonColours(prev_clicked_time_duration, false);
                ChangeButtonColours((Button)sender, true);
                prev_clicked_time_duration = (Button)sender;

                await OnDataExtract(sender, e, start, end);

            }
        }

        private async void OnCustomRangeClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                start = viewModel.LowDate;
                end = viewModel.HighDate;

                ChangeButtonColours(prev_clicked_time_duration, false);
                ChangeButtonColours((Button)sender, true);
                prev_clicked_time_duration = (Button)sender;

                if (start > end) { return; } //TODO: ADD LABEL FOR ERROR TEXT AND BIND IT
                await OnDataExtract(sender, e, start, end);

            }
        }

        private async void OnParameterDisplayClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (currently_showing != null)
            {
                Button prevButton = this.FindByName<Button>(currently_showing);
                if (prevButton != null)
                {
                    ChangeButtonColours(prevButton, false);
                }
            }
            currently_showing = clickedButton.StyleId;
            ChangeButtonColours(clickedButton, true);
            await OnDataExtract(sender, e, start, end);
        }

        private async void OnNodeButtonClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if(node_ids.Contains(clickedButton.Text))
            {
                //clickedButton.BackgroundColor = (Color)Application.Current.Resources["Button"];
                ChangeButtonColours(clickedButton, false);
                node_ids.Remove(clickedButton.Text);
            }
            else
            {
                ChangeButtonColours(clickedButton, true);
                //clickedButton.BackgroundColor = (Color)Application.Current.Resources["ButtonPressed"];
                node_ids.Add(clickedButton.Text);
            }


            await OnDataExtract(sender, e, start, end);
        }

        private void ChangeButtonColours(Button button, bool is_selected)
        {
            if (!is_selected) //if button is no longer selected go back to default colour
            {
                button.BackgroundColor = (Color)Application.Current.Resources["Button"];
            }
            else
            {
                button.BackgroundColor = (Color)Application.Current.Resources["ButtonPressed"];
            }
        }
        private async Task OnDataExtract(object sender, EventArgs e, DateTime start, DateTime end)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                //Chart.Series = null; // Clear existing series
                Chart.IsVisible = false;
                await viewModel.ShowData(start, end, currently_showing, node_ids);
                Chart.CoreChart.Update();
                Chart.IsVisible = true;

            }
        }
    }
}
