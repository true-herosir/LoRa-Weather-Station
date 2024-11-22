
namespace mqtt_parser
{
    internal class interfaces
    {
        public interface INode_parse
        {
            Dictionary<string, object?> data(string JSON);
            
        }

        public interface ISQL_QueryBuilder
        {
            string build(string Node_ID, DateTime Time, double? pressure, double? Illumination, double? Humidity, string Location, double Temperature_indor, double? Temperature_outdor);
            string build(string Node_ID, string Location, double? Battery_status);
        }
    }
}
