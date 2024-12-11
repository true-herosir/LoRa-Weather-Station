    using static WeatherThingy.Sources.Services.WeatherThingyService;
    using LiveChartsCore;
    using LiveChartsCore.Defaults;
    using LiveChartsCore.SkiaSharpView;
    using LiveChartsCore.SkiaSharpView.SKCharts;
using WeatherThingy.Sources.Services;
using System.Collections.ObjectModel;
//using AVFoundation;

namespace WeatherThingy.Pages
{
    public partial class EnschedePage : ContentPage
    {
        public ObservableCollection<string> WeatherData { get; set; } = new ObservableCollection<string>();
        public EnschedePage()
        {
            InitializeComponent();
            BindingContext = this;
            //InitialiseChartAsync();
        }

        //private async void InitialiseChartAsync()
        //{
        //    var temperatureSeriesData = new ObservableCollection<ObservablePoint>();
        //    var data = await new WeatherThingyService().GetNodeData();
        //    foreach (var item in from item in data.data
        //                         where item.humidity != null
        //                         select item)
        //    {
        //        temperatureSeriesData.Add(new ObservablePoint { X = item.time.ToOADate(), Y = item.humidity.Value });
        //    }

        //    Chart.Series = new ISeries[]
        //    {
        //            new LineSeries<ObservablePoint>
        //            {
        //                Values = temperatureSeriesData
        //            }
        //    };
        //}
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

        private void OnPointerEntered(object sender, EventArgs e)
        {
            if (sender is HorizontalStackLayout h_layout) h_layout.BackgroundColor = Color.FromArgb("#415679");
        }

        private void OnPointerExited(object sender, EventArgs e)
        {
            if (sender is HorizontalStackLayout h_layout) h_layout.BackgroundColor = Color.FromArgb("#4A628A");
        }

        private async void ShowData(object sender, EventArgs e)
        {
            var start = DateTime.Now.AddDays(-1);
            var end = DateTime.Now;
            var data = await new WeatherThingyService().GetNodeData("Enschede", start, end, 1);

            WeatherData.Clear();
            foreach (var item in data.data)
            {
                WeatherData.Add($"Humidity in {item.node_id} at {item.time.Value.TimeOfDay.ToString()}: " + item.humidity.Value.ToString()); //showing just the values of humidity
            }
        }

    }

    }
