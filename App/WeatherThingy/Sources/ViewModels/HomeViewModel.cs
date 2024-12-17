using WeatherThingy.Sources.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using System.Collections.ObjectModel;

namespace WeatherThingy.Sources.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public ObservableCollection<Datum> MostRecent { get; } = new();
    public ObservableCollection<Datum> FilteredNode { get; } = new();
    public ObservableCollection<string> NodeId { get; } = new();

    private IWeatherThingyService weather_thingy_service;

    public HomeViewModel()
    {
        weather_thingy_service = new WeatherThingyService();
        _ = InitializeData();
    }

    private async Task InitializeData()
    {
        await GetMostRecentData();
        if (NodeId.Any())
            await GetNodeByLocation(NodeId[0]);
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
            }
        }
        catch (Exception ex)
        {
            // Replace with proper logging
            Console.WriteLine($"Error fetching data: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GetNodeByLocation(string location)
    {
        if (string.IsNullOrEmpty(location))
            return;

        try
        {
            // Assuming MostRecent is an ObservableCollection or IQueryable
            var filteredNodes = MostRecent.Where(n => n.location == location);

            // If you need to update the UI or another collection with the results
            FilteredNode.Clear();
            foreach (var node in filteredNodes)
            {
                FilteredNode.Add(node); // Assuming FilteredNodes is a bindable collection
            }
        }
        catch (Exception ex)
        {
            // Handle potential exceptions (e.g., logging)
            Debug.WriteLine($"Error in GetNodeByLocation: {ex.Message}");
        }
    }
}
