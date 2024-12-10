using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherThingy.Sources.Model;
using static WeatherThingy.Sources.Model.GUI_JSON_parse;

namespace WeatherThingy.Sources.Services
{
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
            _api_table.Add("Most_Recent");
            _api_table.Add("Nodes/sensor_location");
            _api_table.Add("Hours_AVG");
            _api_table.Add("Max_Min");
        }
        public async Task<Root> callAPI()
        {
            string urlData = _api_complete;
            var responseData = await _httpClient.GetStringAsync(urlData);
            var rootData = JsonDocument.Parse(responseData).RootElement;

            var node = new Root
            {
                total_items = Convert.ToInt16(rootData.GetProperty("totalItems").ToString()),
                total_pages = Convert.ToInt16(rootData.GetProperty("totalPages").ToString()),
                current_page = Convert.ToInt16(rootData.GetProperty("currentPage").ToString()),
                page_size = Convert.ToInt16(rootData.GetProperty("pageSize").ToString()),
                data = new List<Datum>()
            };
            

            if (rootData.TryGetProperty($"data", out var Data))
            {
                foreach (var item in Data.EnumerateObject())
                {
                    var datum = new Datum
                    {
                        time = Convert.ToDateTime(item.Value.GetProperty("time")),
                        node_ID = item.Value.GetProperty("node_ID").ToString(),
                        pressure = Convert.ToDouble(item.Value.GetProperty("pressure")),
                        illumination = Convert.ToDouble(item.Value.GetProperty("illumination")),
                        humidity = Convert.ToDouble(item.Value.GetProperty("humidity")),
                        temperature_indoor = Convert.ToDouble(item.Value.GetProperty("temperature_indoor")),
                        temperature_outdoor = Convert.ToDouble(item.Value.GetProperty("temperature_outdoor")),
                        gateway_Location = item.Value.GetProperty("gateway_Location").ToString(),
                        location = item.Value.GetProperty("location").ToString(),
                        battery_status = Convert.ToInt16(item.Value.GetProperty("battery_status").ToString())
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

        public Task<Root> GetNodeData(string location, DateTime start, DateTime end, int page)
        {
            if (location.Contains("lht") || location.Contains("mkr") || location.Contains("thingy"))
            {
                location = "?id=" + location;
            }
            else location = "?sensor_location=" + location;
            TimeSpan timeDiff = end - start;
            int daysDifference = timeDiff.Days;
            int table = 0;
            string start_st;
            string end_st;

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
                      
            _api_param = $"{location}&start_time={start_st}&end_time={end_st}&page={page}&page_size=200";
            _api_complete = _api_IP + _api_PORT + _api_table[table] + _api_param;
            return callAPI();
        }

    }
}
