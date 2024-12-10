using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherThingy.Sources.Model.GUI_interfaces;

namespace WeatherThingy.Sources.Model
{
    internal class GUI_JSON_parse : IGUI_JSON_parse
    {

        internal class Datum
        {
            public string node_ID { get; set; }
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

            public double? avG_pressure { get; set; }
            public double? avG_illumination { get; set; }
            public double? avG_humidity { get; set; }
            public double? avG_temperature_indoor { get; set; }
            public double? avG_temperature_outdoor { get; set; }

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

        internal class Root
        {
            public int total_items { get; set; }
            public int total_pages { get; set; }
            public int current_page { get; set; }
            public int page_size { get; set; }
            public List<Datum> data { get; set; }
        }

        public List<string> gateway_Location(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<string> result = new List<string>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.gateway_Location);
            }
            return result;
        }

        public List<double?> humidity(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<double?> result = new List<double?>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.humidity);
            }
            return result;
        }

        public List<double?> illumination(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<double?> result = new List<double?>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.illumination);
            }
            return result;
        }

        public List<string> node_ID(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<string> result = new List<string>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.node_ID);
            }
            return result;
        }

        public List<double?> pressure(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<double?> result = new List<double?>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.pressure);
            }
            return result;
        }

        public List<double?> temperature_indor(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<double?> result = new List<double?>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.temperature_indoor);
            }
            return result;
        }

        public List<double?> temperature_outdor(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<double?> result = new List<double?>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.temperature_outdoor);
            }
            return result;
        }

        public List<DateTime> time(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<DateTime> result = new List<DateTime>();
            foreach (var item in Deserialized.data)
            {
                result.Add(item.time.Value);
            }
            return result;
        }

        public List<bat_stat> battery(string json)
        {
            Root? Deserialized = JsonConvert.DeserializeObject<Root>(json);
            List<bat_stat> result = new List<bat_stat>();
            foreach (var item in Deserialized.data)
            {
                bat_stat temp = new bat_stat();
                temp.node_id = item.node_ID;
                temp.location = item.location;
                temp.bat = item.battery_status;
                result.Add(temp);
            }
            return result;
        }
    }
}
