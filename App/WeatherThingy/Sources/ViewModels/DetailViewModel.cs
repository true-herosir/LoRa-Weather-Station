using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System;
using WeatherThingy.Sources.Services;
using LiveChartsCore.Kernel.Sketches;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WeatherThingy.Sources.ViewModels
{
    public class Plot
    {
        public ObservableCollection<DateTimePoint> datapoints { get; set; } = new ObservableCollection<DateTimePoint>();
        public string node_id { get; set; }
    }

    public class DetailViewModel : INotifyPropertyChanged
    {

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateTime _lowdate;
        public DateTime LowDate
        {
            get => _lowdate;
            set
            {
                if (_lowdate != value) // Update only if the value is different
                {
                    _lowdate = value;
                    OnPropertyChanged(); // Notify subscribers
                }
            }
        }

        public DateTime _highdate;
        public DateTime HighDate
        {
            get => _highdate;
            set
            {
                if (_highdate != value) // Update only if the value is different
                {
                    _highdate = value;
                    OnPropertyChanged(); // Notify subscribers
                }
            }
        }
        public DateTime MinDate { get; } = new DateTime(2024, 11, 22);
        public DateTime MaxDate { get; } = DateTime.Now;
        public List<Plot> plots = new List<Plot>();
        public List<string> nodes = new List<string>();

        public event PropertyChangedEventHandler? PropertyChanged;

        // ObservableCollection for chart data
        public ObservableCollection<DateTimePoint> HumiditySeriesData { get; set; } = new ObservableCollection<DateTimePoint>();
        public ObservableCollection<ISeries> Series { get; set; } = new ObservableCollection<ISeries>();



        public async Task<List<Plot>> initialize_plots()
        {
            var data = await new WeatherThingyService().GetNodeData();
            foreach (var item in data.data)
            {
                if (item.time.HasValue) // Ensure time is not null
                {
                    var retrieved = new Plot();
                    var retrieved_min = new Plot();
                    retrieved.node_id = item.node_id;
                    retrieved_min.node_id = item.node_id + "_min";
                    plots.Add(retrieved);
                    plots.Add(retrieved_min);

                }

            }
            return plots;

        }


        public DetailViewModel()
        {
            initialize_plots();

        }

        //X-Axis configuration with DateTime label formatting
        public Axis[] XAxes { get; set; } = new Axis[]
        {
            new Axis
            {
                Labeler = value =>
                {
                    if (value < DateTime.MinValue.Ticks || value > DateTime.MaxValue.Ticks)
                        {
                            return "0";
                        }
                    var dateTime = new DateTime((long)value); // Convert long (ticks) to DateTime
                    return dateTime.ToString("dd HH:mm");    // Display only day, hour, minute
                },
                UnitWidth = TimeSpan.FromMinutes(30).Ticks, // Adjust the spacing for your data
            }
        };


        public async Task ShowData(DateTime start, DateTime end)
        {
            Series.Clear();
            nodes.Clear();


            nodes.Add("lht-gronau"); nodes.Add("lht-wierden"); nodes.Add("mkr-saxion");
            try
            {
                double filtered_value;
                TimeSpan timeDiff = end - start;
                int daysDifference = timeDiff.Days;

                foreach (var node in nodes)
                {

                    var data = await new WeatherThingyService().GetNodeData(node, start, end, 1);
                    int index = 0;
                    foreach (var plot in plots)
                    {
                        if (plot.node_id == node) break;
                        index++;
                    }
                    plots[index].datapoints.Clear();
                    plots[index + 1].datapoints.Clear();
                    foreach (var item in data.data)
                    {
                        if (item.time.HasValue || item.the_day.HasValue) // Ensure time is not null
                        {

                            var time_stamp = item.time.HasValue ? item.time.Value : item.the_day.Value;

                            if (daysDifference > 14)
                            {
                                filtered_value = item.max_humidity.Value;

                                plots[index + 1].datapoints.Add(new DateTimePoint(time_stamp, item.min_humidity.Value));
                            }
                            else filtered_value = item.humidity.Value;
                            // Add data to chart
                            plots[index].datapoints.Add(new DateTimePoint(time_stamp, filtered_value));
                        }

                    }

                    Series.Add(
                        new LineSeries<DateTimePoint>
                        {
                            Values = plots[index].datapoints,

                            Name = daysDifference > 14 ? plots[index].node_id + "_max" : plots[index].node_id
                        });
                    if (daysDifference > 14)
                    {
                        Series.Add(
                        new LineSeries<DateTimePoint>
                        {
                            Values = plots[index + 1].datapoints,

                            Name = plots[index + 1].node_id
                        });

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