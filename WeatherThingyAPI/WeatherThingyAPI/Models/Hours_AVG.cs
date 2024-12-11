using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherThingyAPI.Models
{
    [Table("hours_avg", Schema = "lr2")]
    public class Hours_AVG
    {
        public string Node_ID { get; set; }
        public string? Location { get; set; }
        public DateOnly the_day { get; set; }
        public Byte the_hour { get; set; }
        public double? AVG_Pressure { get; set; }
        public double? AVG_Illumination { get; set; }
        public double? AVG_Humidity { get; set; }
        public double? AVG_Temperature_indoor { get; set; }
        public double? AVG_Temperature_outdoor { get; set; }
    }
}
