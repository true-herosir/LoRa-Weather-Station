namespace WeatherThingy.Sources.Models;

public partial class Datum
{
    public string node_id { get; set; }
    public DateTime? time { get; set; }
    public string? gateway_Location { get; set; }
    public string? location { get; set; }

    public double? pressure { get; set; }
    public double? illumination { get; set; }
    public double? humidity { get; set; }
    public double? temperature_indoor { get; set; }
    public double? temperature_outdoor { get; set; }

    public DateTime? the_day { get; set; }
    public DateTime? the_hour { get; set; }

    public double? avg_pressure { get; set; }
    public double? avg_illumination { get; set; }
    public double? avg_humidity { get; set; }
    public double? avg_temperature_indoor { get; set; }
    public double? avg_temperature_outdoor { get; set; }

    public double? min_pressure { get; set; }
    public double? min_illumination { get; set; }
    public double? min_humidity { get; set; }
    public double? min_temperature_indoor { get; set; }
    public double? min_temperature_outdoor { get; set; }

    public double? max_pressure { get; set; }
    public double? max_illumination { get; set; }
    public double? max_humidity { get; set; }
    public double? max_temperature_indoor { get; set; }
    public double? max_temperature_outdoor { get; set; }
    public int? battery_status { get; set; }
}

public partial class Root
{
    public int total_items { get; set; }
    public int total_pages { get; set; }
    public int current_page { get; set; }
    public int page_size { get; set; }
    public List<Datum> data { get; set; }
}
