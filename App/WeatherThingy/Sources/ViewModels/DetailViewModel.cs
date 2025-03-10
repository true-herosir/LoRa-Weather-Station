﻿using LiveChartsCore;
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
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using CommunityToolkit.Mvvm.Messaging;

//using Org.W3c.Dom;


namespace WeatherThingy.Sources.ViewModels
{
    public class Plot
    {
        public ObservableCollection<DateTimePoint> Datapoints { get; } = new ObservableCollection<DateTimePoint>();
        public string NodeId { get; set; }
    }

    public partial class DetailViewModel : ObservableObject
    {

        private string _selectedParameter;
        public string SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                if (SetProperty(ref _selectedParameter, value))
                {
                    UpdateAxes();
                }
            }
        }

        private TimeSpan _selectedTimeDuration = TimeSpan.FromMinutes(1);
        public TimeSpan SelectedTimeDuration
        {
            get => _selectedTimeDuration;
            set
            {
                if (SetProperty(ref _selectedTimeDuration, value))
                {
                    UpdateAxes();
                }
            }
        }

        private void UpdateAxes()
        {
            if (XAxes != null && XAxes.Length > 0)
            {
                XAxes[0].UnitWidth = SelectedTimeDuration.Ticks;
            }

            if (YAxes != null && YAxes.Length > 0)
            {
                YAxes[0].Name = _selectedParameter;

                switch (_selectedParameter.ToLower())
                {
                    case "humidity":
                        YAxes[0].Name = "Humidity (%)";
                        YAxes[0].Labeler = value => value.ToString("N2"); // Percentage format
                        break;
                    case "illumination":
                        YAxes[0].Name = "Illumination (%)";
                        YAxes[0].Labeler = value => value.ToString("N2"); // Default formatting
                        break;
                    case "pressure":
                        YAxes[0].Name = "Pressure (hPa)";
                        YAxes[0].Labeler = value => value.ToString("N2"); // Default formatting
                        break;
                    case "temperature indoor":
                        YAxes[0].Name = "Temp. Indoor (°C)";
                        YAxes[0].Labeler = value => value.ToString("N2"); // Default formatting
                        break;
                    case "temperature outdoor":
                        YAxes[0].Name = "Temp. Outdoor (°C)";
                        YAxes[0].Labeler = value => value.ToString("N2"); // Default formatting
                        break;
                    default:
                        YAxes[0].Labeler = value => value.ToString("N2"); // Default formatting
                        break;
                }
            }
        }

        private DateTime _lowDate;
        public DateTime LowDate
        {
            get => _lowDate;
            set => SetProperty(ref _lowDate, value);
        }

        private DateTime _highDate;
        public DateTime HighDate
        {
            get => _highDate;
            set => SetProperty(ref _highDate, value);
        }

        public DateTime MinDate { get; } = new DateTime(2024, 11, 22);
        public DateTime MaxDate { get; } = DateTime.Now;
        public List<Plot> Plots { get; } = new List<Plot>();

        // ObservableCollection for chart data
        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        public ObservableCollection<String> AvailableNodes { get; } = new ObservableCollection<string>();

        public async Task InitializePlotsAsync()
        {
            try
            {
                var data = await new WeatherThingyService().GetNodeData();
                if (data == null || data.data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }
                foreach (var item in data.data)
                {
                    if (item.time.HasValue) // Ensure time is not null
                    {
                        var retrieved = new Plot { NodeId = item.node_id };
                        var retrievedMin = new Plot { NodeId = item.node_id + "_min" };
                        Plots.Add(retrieved);
                        Plots.Add(retrievedMin);
                        if (!AvailableNodes.Contains(item.node_id)) AvailableNodes.Add(item.node_id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        // X-Axis configuration with DateTime label formatting
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; }
        public async Task ShowDataAsync(DateTime start, DateTime end, string value, List<string> nodes)
        {
            Series.Clear();
            List<string> null_nodes = new List<string>(); //for nodes missing sensors
            List<string> no_data_nodes = new List<string>(); // for nodes that return any data, like a late installed devices

            Dictionary<string,string> time_issue_nodes = new Dictionary<string, string>(); // for nodes with no data before  more than 2 hrs from the start time, and their start times


            try
            {
                double? filteredValue;
                TimeSpan timeDiff = end - start;
                int daysDifference = timeDiff.Days;
                bool DataFetchFailed = false;
                bool FoundNullNode = false;
                bool no_data = false;
                bool time_issues = false;
                char interval = daysDifference > 14 ? 'm' : daysDifference > 3 ? 'w' : 'd';
                foreach (var node in nodes)
                {
                    var data = await new WeatherThingyService().GetNodeData(node, start, end, 1);
                    if ((data == null || data.data == null) && !DataFetchFailed)
                    {
                        MessagingCenter.Send(this, "FailedToFetchData", "Failed to fetch the data! Please make sure you have a stable internet connection.");
                        DataFetchFailed = true;
                        throw new ArgumentNullException(nameof(data));
                    }
                    if (data.data.Count == 0)
                    {
                        no_data = true;
                        no_data_nodes.Add($"({node})");
                        continue;
                    }

                    var plot = Plots.FirstOrDefault(p => p.NodeId == node);
                    var plotMin = Plots.FirstOrDefault(p => p.NodeId == node + "_min");

                    plot?.Datapoints.Clear();
                    plotMin?.Datapoints.Clear();


                    //handling late installed devices
                    var first_time = data.data[0].time ?? data.data[0].the_day.Value;
                    TimeSpan first_timeDiff = first_time - start;

                    if (first_timeDiff.TotalHours >= 1.5)
                    {
                        time_issues = true;
                        time_issue_nodes.Add($"({node})", first_time.ToString("yyyy-MM-dd HH:mm"));
                        
                    }


                    foreach (var item in data.data)
                        {
                            if (item.time.HasValue || item.the_day.HasValue) // Ensuring time is not null
                            {
                                var timeStamp = item.time ?? item.the_day.Value;


;

                                if (daysDifference > 14)
                                {

                                    filteredValue = GetPropertyValue(item, "max_" + value);
                                    plotMin?.Datapoints.Add(new DateTimePoint(timeStamp, GetPropertyValue(item, "min_" + value)));
                                }
                                else
                                {
                                    filteredValue = GetPropertyValue(item, value);
                                }

                                // Add data to chart if the value is not null
                                if (filteredValue != null)
                                {
                                    plot?.Datapoints.Add(new DateTimePoint(timeStamp, filteredValue));
                                }
                                else
                                {
                                    //For some reason could not make the newer version work
                                    //WeakReferenceMessenger.Default.Send("One or more of the chosen nodes do not support the chosen sensor", "NoSensorLabel");

                                    null_nodes.Add($"({node})");
                                    FoundNullNode = true;
                                    break;
                                }
                            }
                        }

                    if (plot != null)
                    {
                        Series.Add(new StepLineSeries<DateTimePoint>
                        {
                            Values = plot.Datapoints,
                            Name = daysDifference > 14 ? plot.NodeId + "_max" : plot.NodeId,
                            GeometrySize = 0.5
                        });
                    }

                    if (daysDifference > 14 && plotMin != null)
                    {
                        Series.Add(new StepLineSeries<DateTimePoint>
                        {
                            Values = plotMin.Datapoints,
                            Name = plotMin.NodeId,
                            GeometrySize = 0.5,
                        });
                    }
                }
                if (FoundNullNode)
                {
                    string null_nodes_text = "";
                    null_nodes_text = String.Join(", ", null_nodes.ToArray());

                    string p_s = null_nodes.Count() > 1 ? "s" : "";
                    string p_es = null_nodes.Count() > 1 ? "" : "es";
                    string p_th = null_nodes.Count() > 1 ? "those" : "that";

                    MessagingCenter.Send(this, "NoSensorLabel", $"The node{p_s} {null_nodes_text} do{p_es} not support a sensor for ({value}).\nPlease unselect {p_th} node{p_s}.");
                }

                if (no_data)
                {
                    string no_data_nodes_text = "";
                    no_data_nodes_text = String.Join(", ", no_data_nodes.ToArray());

                    string p_s = no_data_nodes.Count() > 1 ? "s" : "";
                    string p_es = no_data_nodes.Count() > 1 ? "" : "es";
                    string p_th = no_data_nodes.Count() > 1 ? "those" : "that";
                    string date_format = interval == 'd' ? "yyyy-MM-dd HH:mm" : interval == 'w' ? "yyyy-MM-dd HH:00" : "yyyy-MM-dd";
                    MessagingCenter.Send(this, "TimeIssues", $"The node{p_s} {no_data_nodes_text} do{p_es} not have data to show before {end.ToString(date_format)}.\nPlease unselect {p_th} node{p_s}.");
                }

                if (time_issues)
                {
                    string addition = daysDifference > 14 ? "accumulated daily " : daysDifference > 3 ? "accumulated hourly " : "";

                    string error_msg = "";
                    foreach (var node in time_issue_nodes)
                    {
                        error_msg += $"The node {node.Key} does not have {addition}data to show before {node.Value}.\n";
                    }
                    error_msg += "Please keep that in mind while checking data.";
                    MessagingCenter.Send(this, "TimeIssues", error_msg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        private double? GetPropertyValue(object obj, string propertyName)
        {
            var prop = obj.GetType().GetProperty(propertyName);
            return prop?.GetValue(obj) as double?;
        }
    }
}
