using System.Windows.Input;
using WeatherThingy.Sources.Services;

namespace WeatherThingy.Sources.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public ObservableCollection<Datum> MostRecent { get; } = new();
        public ObservableCollection<Datum> FilteredNode { get; set; } = new();
        public ObservableCollection<string> NodeId { get; } = new();

        //public ICommand RefreshCommand { get; }
        public ICommand LoadLocationCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private readonly IWeatherThingyService _weatherThingyService;

        // Use a dictionary for better status mapping
        private static readonly Dictionary<string, string> BatteryStatusMap = new()
        {
            { "1", "bat1.png" },
            { "2", "bat2.png" },
            { "3", "bat3.png" },
            { "4", "bat4.png" }
        };

        // Constructor
        public HomeViewModel(IWeatherThingyService weatherThingyService)
        {
            _weatherThingyService = weatherThingyService ?? throw new ArgumentNullException(nameof(weatherThingyService));

            // Initialize commands
            //RefreshCommand = new RelayCommand(OnRefresh);
            LoadLocationCommand = new RelayCommand<string>(OnClickedLocation);

            // Initialize data asynchronously
            _ = InitializeData();
        }

        // Initialize value by fetching the data
        private async Task InitializeData()
        {
            MostRecent.Clear();
            FilteredNode.Clear();
            NodeId.Clear();
            await GetMostRecentDataCommand();
            await GetNodeByLocationCommand(NodeId.FirstOrDefault());
        }


        private async void OnClickedLocation(string location)
        {
            Console.WriteLine("click button " + location);
            // Refresh data for a specific location
            if (!string.IsNullOrEmpty(location))
            {
                await GetNodeByLocationCommand(location);
            }
        }

        // Get the most recent data
        [RelayCommand]
        private async Task GetMostRecentDataCommand()
        {
            try
            {
                var data = await _weatherThingyService.GetNodeData();
                foreach (var datum in data.data)
                {
                    if (datum is not null)
                    {
                        MostRecent.Add(datum);
                        if (!NodeId.Contains(datum.location))
                            NodeId.Add(datum.location);
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
            }
            catch (Exception ex)
            {
                // Replace with proper logging
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        // Get nodes filtered by location
        private Task GetNodeByLocationCommand(string location)
        {
            if (string.IsNullOrEmpty(location))
                return Task.CompletedTask;

            try
            {
                // Filter nodes by location and update the FilteredNode collection
                var filteredNodes = MostRecent.Where(n => n.location.Equals(location, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredNode.Clear();
                foreach (var node in filteredNodes)
                {
                    FilteredNode.Add(node);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                Debug.WriteLine($"Error filtering nodes by location: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
