using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherThingy.Sources.Model
{
    public class bat_stat
    {
        public string node_id { get; set; }
        public string location { get; set; }
        public int? bat { get; set; }
    }

        public interface IGUI_JSON_parse
        {
            List<string> node_ID(string json);
            List<DateTime> time(string json);
            List<double?> pressure(string json);
            List<double?> illumination(string json);
            List<double?> humidity(string json);
            List<string> gateway_Location(string json);
            List<double?> temperature_indoor(string json);
            List<double?> temperature_outdoor(string json);
            List<bat_stat> battery(string json);
        }

}
