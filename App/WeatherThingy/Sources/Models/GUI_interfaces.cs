namespace WeatherThingy.Sources.Models
{
    public class bat_stat
    {
        public string node_id { get; set; }
        public string location { get; set; }
        public int? battery_status { get; set; }
    }
    internal class GUI_interfaces
    {
        public interface IGUI_JSON_parse
        {
            List<string> node_ID(string json);
            List<DateTime> time(string json);
            List<double?> pressure(string json);
            List<double?> illumination(string json);
            List<double?> humidity(string json);
            List<string> gateway_Location(string json);
            List<double?> temperature_indor(string json);
            List<double?> temperature_outdor(string json);
            List<bat_stat> battery(string json);
        }

    }
}
