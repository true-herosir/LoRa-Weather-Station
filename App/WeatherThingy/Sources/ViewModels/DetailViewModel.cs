using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System;
using WeatherThingy.Sources.Services;

namespace WeatherThingy.Sources.ViewModels
{
    public class DetailViewModel
    {
        // ObservableCollection for chart data
        public ObservableCollection<DateTimePoint> HumiditySeriesData { get; set; } = new ObservableCollection<DateTimePoint>();
        public ObservableCollection<DateTimePoint> HumiditySeriesData2 { get; set; } = new ObservableCollection<DateTimePoint>();

        // Property to bind to the chart
        public ISeries[] Series => new ISeries[]
        {
            new LineSeries<DateTimePoint>
            {
                Values = HumiditySeriesData,
                Name = "ser"
            },
             new LineSeries<DateTimePoint>
            {
                Values = HumiditySeriesData2,
                Name = "ser2"
            }
        };

        public DetailViewModel()
        {
        }

        // X-Axis configuration with DateTime label formatting
        public Axis[] XAxes { get; set; } = new Axis[]
        {
            new Axis
            {
                Labeler = value =>
                {
                    var dateTime = new DateTime((long)value); // Convert long (ticks) to DateTime
                    return dateTime.ToString("dd HH:mm");    // Display only day, hour, minute
                },
                UnitWidth = TimeSpan.FromMinutes(30).Ticks, // Adjust the spacing for your data
            }
        };

        // Async method to fetch and display data
        public async Task ShowData()
        {
            try
            {

                var start = DateTime.Now.AddDays(-1);
                var end = DateTime.Now;
                var data = await new WeatherThingyService().GetNodeData("lht-gronau", start, end, 1);
                var data2 = await new WeatherThingyService().GetNodeData("lht-wierden", start, end, 1);

                HumiditySeriesData.Clear();

                // Process data and update the collections
                foreach (var item in data.data)
                {
                    if (item.time.HasValue) // Ensure time is not null
                    {
                        // Add data to chart
                        HumiditySeriesData.Add(new DateTimePoint(item.time.Value, item.humidity.Value));
                    }
                }
                foreach (var item in data2.data)
                {
                    if (item.time.HasValue) // Ensure time is not null
                    {
                        // Add data to chart
                        HumiditySeriesData2.Add(new DateTimePoint(item.time.Value, item.humidity.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as network errors or invalid data
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }
    }
}
