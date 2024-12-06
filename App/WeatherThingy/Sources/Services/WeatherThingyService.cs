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

        public WeatherThingyService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<Root> GetNodeData()
        {
            string urlData = $"http://84.85.32.192:7086/api/Nodes?nodeId=lht-gronau&page=1&pageSize=1000";
            var responseData = await _httpClient.GetStringAsync(urlData);
            var rootData = JsonDocument.Parse(responseData).RootElement;

            var node = new Root
            {
                totalItems = Convert.ToInt16(rootData.GetProperty("totalItems").ToString()),
                totalPages = Convert.ToInt16(rootData.GetProperty("totalPages").ToString()),
                currentPage = Convert.ToInt16(rootData.GetProperty("currentPage").ToString()),
                pageSize = Convert.ToInt16(rootData.GetProperty("pageSize").ToString()),
                data = new List<Datum>()
            };

            if (rootData.TryGetProperty($"data", out var Data))
            {
                foreach (var item in Data.EnumerateArray())
                {
                    var datum = new Datum
                    {
                        time = Convert.ToDateTime(item.GetProperty("time")),
                        node_ID = item.GetProperty("node_ID").ToString(),
                        pressure = Convert.ToDouble(item.GetProperty("pressure")),
                        illumination = Convert.ToDouble(item.GetProperty("illumination")),
                        humidity = Convert.ToDouble(item.GetProperty("humidity")),
                        temperature_indoor = Convert.ToDouble(item.GetProperty("temperature_indoor")),
                        temperature_outdoor = Convert.ToDouble(item.GetProperty("temperature_outdoor")),
                        gateway_Location = item.GetProperty("gateway_Location").ToString(),
                        location = item.GetProperty("location").ToString(),
                        battery_status = Convert.ToInt16(item.GetProperty("battery_status").ToString())
                    };

                    node.data.Add(datum);
                }
            }
            return node;
        }
    }
}
