using WeatherThingy.Sources.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using System.Collections.ObjectModel;

namespace WeatherThingy.Sources.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ObservableCollection<Datum> MostRecent { get; } = new();

    private IWeatherThingyService weather_thingy_service;

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
                MostRecent.Add(datum);
        }
        catch (Exception ex)
        {
            // Replace with proper logging
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }
}
