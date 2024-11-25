using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
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
        public string? Location { get; set; }
        public double Temperature_indor { get; set; }
        public double? Temperature_outdor { get; set; }
    }
}
