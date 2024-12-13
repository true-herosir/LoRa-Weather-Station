namespace WeatherThingy.Sources.Services;
internal class WeatherThingyService : IWeatherThingyService
{
    HttpClient _httpClient;
    string _api_IP;
    string _api_PORT;
    //string _api_base;
    string _api_param;
    string _api_complete;
    List<string> _api_table = new List<string>();

    public WeatherThingyService()
    {
        _httpClient = new HttpClient();
        //_api_base = $"http://84.85.32.192:7086/api/Nodes/lht-gronau?page=1&pageSize=1000";
        _api_IP = $"http://84.85.32.192";
        _api_PORT = ":7086/api/";
        _api_table.Add("most_recent");
        _api_table.Add("Nodes/node_location");
        _api_table.Add("Hours_AVG");
        _api_table.Add("Max_Min");
        _api_table.Add("Node_location");
    }
    public async Task<Root> callAPI()
    {
        string urlData = _api_complete;
        var responseData = await _httpClient.GetStringAsync(urlData);
        var rootData = JsonDocument.Parse(responseData).RootElement;

        var node = new Root
        {
            total_items = Convert.ToInt16(rootData.GetProperty("total_items").ToString()),
            total_pages = Convert.ToInt16(rootData.GetProperty("total_pages").ToString()),
            current_page = Convert.ToInt16(rootData.GetProperty("current_page").ToString()),
            page_size = Convert.ToInt16(rootData.GetProperty("page_size").ToString()),
            data = new List<Datum>()
        };


        if (rootData.TryGetProperty($"data", out var Data))
        {
            foreach (var item in Data.EnumerateArray())
            {
                var datum = new Datum
                {
                    time = item.TryGetProperty("time", out var timeElement) &&
       DateTime.TryParse(timeElement.ToString(), out var parsedTime)
    ? (DateTime?)parsedTime
    : null,

                    node_id = item.TryGetProperty("node_id", out var nodeElement)
    ? nodeElement.ToString()
    : null,

                    pressure = item.TryGetProperty("pressure", out var pressureElement) &&
           double.TryParse(pressureElement.ToString(), out var parsedPressure)
    ? (double?)parsedPressure
    : null,

                    illumination = item.TryGetProperty("illumination", out var illuminationElement) &&
               double.TryParse(illuminationElement.ToString(), out var parsedIllumination)
    ? (double?)parsedIllumination
    : null,

                    humidity = item.TryGetProperty("humidity", out var humidityElement) &&
           double.TryParse(humidityElement.ToString(), out var parsedHumidity)
    ? (double?)parsedHumidity
    : null,

                    temperature_indoor = item.TryGetProperty("temperature_indoor", out var tempIndoorElement) &&
                     double.TryParse(tempIndoorElement.ToString(), out var parsedTempIndoor)
    ? (double?)parsedTempIndoor
    : null,

                    temperature_outdoor = item.TryGetProperty("temperature_outdoor", out var tempOutdoorElement) &&
                      double.TryParse(tempOutdoorElement.ToString(), out var parsedTempOutdoor)
    ? (double?)parsedTempOutdoor
    : null,

                    gateway_Location = item.TryGetProperty("gateway_Location", out var gatewayLocationElement)
    ? gatewayLocationElement.ToString()
    : null,

                    location = item.TryGetProperty("location", out var locationElement)
    ? locationElement.ToString()
    : null,

                    battery_status = item.TryGetProperty("battery_status", out var batteryStatusElement) &&
                 short.TryParse(batteryStatusElement.ToString(), out var parsedBatteryStatus)
    ? (short?)parsedBatteryStatus
    : null,

                    avg_pressure = item.TryGetProperty("avG_pressure", out var avgPressureElement) &&
               double.TryParse(avgPressureElement.ToString(), out var parsedAvgPressure)
    ? (double?)parsedAvgPressure
    : null,

                    avg_illumination = item.TryGetProperty("avG_illumination", out var avgIlluminationElement) &&
                   double.TryParse(avgIlluminationElement.ToString(), out var parsedAvgIllumination)
    ? (double?)parsedAvgIllumination
    : null,

                    avg_humidity = item.TryGetProperty("avG_humidity", out var avgHumidityElement) &&
               double.TryParse(avgHumidityElement.ToString(), out var parsedAvgHumidity)
    ? (double?)parsedAvgHumidity
    : null,

                    avg_temperature_indoor = item.TryGetProperty("avG_temperature_indoor", out var avgTempIndoorElement) &&
                         double.TryParse(avgTempIndoorElement.ToString(), out var parsedAvgTempIndoor)
    ? (double?)parsedAvgTempIndoor
    : null,

                    avg_temperature_outdoor = item.TryGetProperty("AVG_temperature_outdoor", out var avgTempOutdoorElement) &&
                          double.TryParse(avgTempOutdoorElement.ToString(), out var parsedAvgTempOutdoor)
    ? (double?)parsedAvgTempOutdoor
    : null,

                    min_pressure = item.TryGetProperty("min_pressure", out var minPressureElement) &&
               double.TryParse(minPressureElement.ToString(), out var parsedMinPressure)
    ? (double?)parsedMinPressure
    : null,

                    min_illumination = item.TryGetProperty("min_illumination", out var minIlluminationElement) &&
                   double.TryParse(minIlluminationElement.ToString(), out var parsedMinIllumination)
    ? (double?)parsedMinIllumination
    : null,

                    min_humidity = item.TryGetProperty("min_humidity", out var minHumidityElement) &&
               double.TryParse(minHumidityElement.ToString(), out var parsedMinHumidity)
    ? (double?)parsedMinHumidity
    : null,

                    min_temperature_indoor = item.TryGetProperty("min_temperature_indoor", out var minTempIndoorElement) &&
                         double.TryParse(minTempIndoorElement.ToString(), out var parsedMinTempIndoor)
    ? (double?)parsedMinTempIndoor
    : null,

                    min_temperature_outdoor = item.TryGetProperty("min_temperature_outdoor", out var minTempOutdoorElement) &&
                          double.TryParse(minTempOutdoorElement.ToString(), out var parsedMinTempOutdoor)
    ? (double?)parsedMinTempOutdoor
    : null,

                    max_pressure = item.TryGetProperty("max_pressure", out var maxPressureElement) &&
               double.TryParse(maxPressureElement.ToString(), out var parsedMaxPressure)
    ? (double?)parsedMaxPressure
    : null,

                    max_illumination = item.TryGetProperty("max_illumination", out var maxIlluminationElement) &&
                   double.TryParse(maxIlluminationElement.ToString(), out var parsedMaxIllumination)
    ? (double?)parsedMaxIllumination
    : null,

                    max_humidity = item.TryGetProperty("max_humidity", out var maxHumidityElement) &&
               double.TryParse(maxHumidityElement.ToString(), out var parsedMaxHumidity)
    ? (double?)parsedMaxHumidity
    : null,

                    max_temperature_indoor = item.TryGetProperty("max_temperature_indoor", out var maxTempIndoorElement) &&
                         double.TryParse(maxTempIndoorElement.ToString(), out var parsedMaxTempIndoor)
    ? (double?)parsedMaxTempIndoor
    : null,

                    max_temperature_outdoor = item.TryGetProperty("max_temperature_outdoor", out var maxTempOutdoorElement) &&
                          double.TryParse(maxTempOutdoorElement.ToString(), out var parsedMaxTempOutdoor)
    ? (double?)parsedMaxTempOutdoor
    : null

                };

                node.data.Add(datum);
            }
        }
        return node;
    }


    public Task<Root> GetNodeData()
    {

        _api_param = "?page=1&page_size=10";
        _api_complete = _api_IP + _api_PORT + _api_table[0] + _api_param;
        return callAPI();
    }


    public Task<Root> GetBattData() // Node_location
    {

        _api_param = "?page=1&page_size=10";
        _api_complete = _api_IP + _api_PORT + _api_table[4] + _api_param;
        return callAPI();
    }

    public Task<Root> GetNodeData(string location, DateTime start, DateTime end, int page)
    {

        if (location.Contains("lht") || location.Contains("mkr") || location.Contains("thingy"))
        {
            location = "?id=" + location;
        }
        else location = "?location=" + location;
        TimeSpan timeDiff = end - start;
        int daysDifference = timeDiff.Days;
        int table = 0;
        string start_st;
        string end_st;
        int page_size = location.Contains("lht") ? 250 : 900;

        if (daysDifference < 0) throw new InvalidDataException("End date cannot be before start date");
        else if (daysDifference <= 3)
        {
            table = 1;
            start_st = start.ToString("yyyy-MM-dd'%20'HH'%3A'mm'%3A'00.000");
            end_st = end.ToString("yyyy-MM-dd'%20'HH'%3A'mm'%3A'00.000");

        }
        else if (daysDifference > 3 && daysDifference < 15)
        {
            table = 2;
            start_st = start.ToString("yyyy-MM-dd");
            end_st = end.ToString("yyyy-MM-dd");
        }
        else
        {
            table = 3;
            start_st = start.ToString("yyyy-MM-dd");
            end_st = end.ToString("yyyy-MM-dd");
        }

        _api_param = $"{location}&start_time={start_st}&end_time={end_st}&page={page}&page_size={page_size}";
        _api_complete = _api_IP + _api_PORT + _api_table[table] + _api_param;
        return callAPI();
    }

}
