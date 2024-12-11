using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("Node_location", Schema = "lr2")]
    public class Node_location
    {
        public string Node_ID { get; set; }
        public string? Location { get; set; }
        public double? Battery_status { get; set; }
    }
}
