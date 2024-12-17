using WeatherThingy.Sources.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using System.Collections.ObjectModel;

namespace WeatherThingy.Sources.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ObservableCollection<Datum> MostRecent { get; } = new();
    public ObservableCollection<string> NodeId { get; } = new();

    private IWeatherThingyService weather_thingy_service;

    Image image = new Image();

    public HomeViewModel()
    {
        weather_thingy_service = new WeatherThingyService();
        Task.Run(async () => await GetMostRecentData());
    }


    [RelayCommand]
    private async Task GetMostRecentData()
    {
        try
        {
            var data = await weather_thingy_service.GetNodeData();
            foreach (var datum in data.data)
            {
                if (datum is not null)
                {
                    MostRecent.Add(datum);
                    if (!NodeId.Contains(datum.location))
                        NodeId.Add(datum.location);
                }
                switch (datum.battery_status)
                {
                    case "1":
                        datum.battery_status = "bat1.png";
                        break;
                    case "2":   
                        datum.battery_status = "bat2.png";
                        break;
                    case "3":
                        datum.battery_status = "bat3.png";
                        break;
                    case "4":
                        datum.battery_status = "bat4.png";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            // Replace with proper logging
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }
}
