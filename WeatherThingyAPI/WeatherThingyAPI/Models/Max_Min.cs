using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("max_min", Schema = "lr2")]
    public class Max_Min
    {
        public string Node_ID { get; set; }
        public string? Location { get; set; }
        public DateOnly the_day { get; set; }
        public double? max_Pressure { get; set; }
        public double? min_Pressure { get; set; }
        public double? max_Illumination { get; set; }
        public double? min_Illumination { get; set; }
        public double? max_Humidity { get; set; }
        public double? min_Humidity { get; set; }
        public double? max_Temperature_indoor { get; set; }
        public double? min_Temperature_indoor { get; set; }
        public double? max_Temperature_outdoor { get; set; }
        public double? min_Temperature_outdoor { get; set; }
    }
}
