using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("Node", Schema = "lr2")]
    public class Node
    {
        public string Node_ID { get; set; }
        public DateTime Time { get; set; }
        public double? Pressure { get; set; }
        public double? Illumination { get; set; }
        public double? Humidity { get; set; }
        public string? Gateway_Location { get; set; }
        public double? Temperature_indoor { get; set; }
        public double? Temperature_outdoor { get; set; }
    }
}
