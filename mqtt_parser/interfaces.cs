﻿
namespace mqtt_parser
{
    internal class interfaces
    {
        public interface INode_parse
        {
            Dictionary<string, object?> data(string JSON);

        }


        public interface ISQL_communicator
        {
            Task build(string Node_ID, DateTime Time, double? pressure, double? Illumination, double? Humidity, string Location, double? Temperature_indor, double? Temperature_outdor);
            Task build(string Node_ID, string Location, double? Battery_status);
            Task coord_update(string Node_ID, double? lat, double? lng, double? alt);
        }

        public interface ILogger
        {
            void log(string messages);
            void log_time(string messages);
            void change_file(string file_name);
        }

    }
}
