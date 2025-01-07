using System.Windows.Input;
using WeatherThingy.Sources.Services;

namespace WeatherThingy.Sources.ViewModels
{

    public partial class HomeViewModel : ObservableObject
    {
        private string lastlocation = new string("Gronau");
        
        float timer_minutes = 5;
        public ObservableCollection<Datum> MostRecent { get; } = new();
        public ObservableCollection<Datum> FilteredNode { get; set; } = new();
        public ObservableCollection<string> NodeId { get; } = new();

        public ICommand LoadLocationCommand { get; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private bool _isButtonPressed;
        public bool IsButtonPressed
        {
            get => _isButtonPressed;
            set => SetProperty(ref _isButtonPressed, value);
        }

        private readonly IWeatherThingyService _weatherThingyService;

        // Constructor
        public HomeViewModel(IWeatherThingyService weatherThingyService)
        {
            _weatherThingyService = weatherThingyService ?? throw new ArgumentNullException(nameof(weatherThingyService));

            // Initialize commands
            LoadLocationCommand = new RelayCommand<string>(OnClickedLocation);

            // Initialize data asynchronously
            _ = InitializeDataAsync();
        }

        // Initialize value by fetching the data
        private async Task InitializeDataAsync()
        {
            while (true)
            {
                await GetMostRecentDataAsync();
                await GetNodeByLocationAsync(NodeId.FirstOrDefault());
                OnClickedLocation(lastlocation);
                await Task.Delay(TimeSpan.FromMinutes(timer_minutes));
            }
        }

        private async void OnClickedLocation(string location)
        {
            Console.WriteLine("click button " + location);
            lastlocation = location;
            // Refresh data for a specific location
            if (!string.IsNullOrEmpty(location))
            {
                await GetNodeByLocationAsync(location);
            }
        }

        // Get the most recent data
        [RelayCommand]
        private async Task GetMostRecentDataAsync()
        {
            try
            {
                var data = await _weatherThingyService.GetNodeData();
                if (data == null || data.data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                MostRecent.Clear();
                NodeId.Clear();

                foreach (var datum in data.data)
                {
                    if (datum is not null)
                    {
                        MostRecent.Add(datum);
                        if (!NodeId.Contains(datum.location))
                            NodeId.Add(datum.location);

                        datum.battery_status = datum.battery_status switch
                        {
                            "0" => "bat01.png",
                            "1" => "bat1.png",
                            "2" => "bat2.png",
                            "3" => "bat3.png",
                            _ => "batt_na.png",
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }

        }

        // Get nodes filtered by location
        private Task GetNodeByLocationAsync(string location)
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
