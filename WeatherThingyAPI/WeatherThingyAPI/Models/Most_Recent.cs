using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("most_recent", Schema = "lr2")]
    public class Most_Recent
    {
        public string Node_ID { get; set; }
        public DateTime Time { get; set; }
        public double? Pressure { get; set; }
        public double? Illumination { get; set; }
        public double? Humidity { get; set; }
        public string? Location { get; set; }
        public double? Temperature_indoor { get; set; }
        public double? Temperature_outdoor { get; set; }
        public string? gateway_id { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public double? alt { get; set; }
    }
}
