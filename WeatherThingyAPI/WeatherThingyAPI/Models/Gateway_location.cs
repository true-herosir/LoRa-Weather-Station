using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("Gateway_location", Schema = "lr2")]
    public class Gateway_location
    {
        public string Node_ID { get; set; }
        public string? Location { get; set; }
        public double? Battery_status { get; set; }
    }
}
