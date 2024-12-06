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
            var data = await new WeatherThingyService().GetNodeData();

            WeatherData.Clear();
            foreach (var item in data.data)
            {
                WeatherData.Add($"{item.temperature_outdoor.ToString}");
            }

        }

    }

    //namespace ViewModelsSamples.Lines.XY
    //{
    //    public class ViewModel
    //    {
    //        public ISeries[] Series { get; set; }

    //        public ViewModel() {
    //            var data = new WeatherThingyService().GetNodeData();
    //            //var data = thingy.GetNodeData();
    //            var series = new LineSeries<ObservablePoint>();
    //            foreach(var item in data.Result.data) 
    //            {
    //                Series.Append(new ObservablePoint(item.temperature_outdoor, Convert.ToDouble(item.time)));
    //                //series += new ObservablePoint(item.temperature_outdoor, Convert.ToDouble(item.time));
    //            }

    //            Series = series;
    //        }

    //    }

    }
