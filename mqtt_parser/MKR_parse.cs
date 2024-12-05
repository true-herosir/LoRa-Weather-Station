using Newtonsoft.Json;
using mkr = mqtt_JSON;
using static mqtt_parser.interfaces;

namespace mqtt_parser
{
    internal class MKR_parse : INode_parse
    {
        public Dictionary<string, object?> data(string JSON)
        {
            Dictionary<string, object?> parsed = new Dictionary<string, object?>();
            mkr.Root root = JsonConvert.DeserializeObject<mkr.Root>(JSON);
            mkr.DecodedPayload results_mkr = root.uplink_message.decoded_payload;
            List<mkr.RxMetadatum> location_mkr = root.uplink_message.rx_metadata;
            mkr.EndDeviceIds loc = root.end_device_ids;
            

            parsed.Add("Node_ID", loc.device_id);
            parsed.Add("Time", DateTime.Now);
            parsed.Add("Pressure", results_mkr.pressure);

            if (results_mkr.light != null)
            {
                double lighty = double.Round((double)(results_mkr.light / 2.55), 2);
                lighty = lighty>100? 100 : lighty;
                parsed.Add("Illumination", lighty);
            }
            else parsed.Add("Illumination", null);

            parsed.Add("Humidity", results_mkr.humidity);

            string city = "unavailable";
            try
            {
                int index = location_mkr[0].gateway_ids.gateway_id == "packetbroker" ? 1 : 0;
                city = location_mkr[index].gateway_ids.gateway_id;

            }
            catch
            {
                city = loc.device_id;
            }
            if (loc.device_id == "weather-thingy-g4-2024") city = "Enschede";

            string[] gibberish = { "mkr-", "lht-", "centrum", "slot", "lora", "-" };

            // Remove substrings
            foreach (var item in gibberish)
            {
                city = city.Replace(item, "", StringComparison.OrdinalIgnoreCase);
            }
            city = char.ToUpper(city[0]) + city.Substring(1);

            parsed.Add("Location", city);
            //parsed.Add("time_stamp", DateTime.Parse(location_lht[0].time));
            //parsed.Add("Gateway lon", location_mkr[0].location.longitude);
            //parsed.Add("Gateway lat", location_mkr[0].location.latitude);


            parsed.Add("Temperature_indoor", results_mkr.temperature);
            parsed.Add("Temperature_outdoor", null);


            parsed.Add("Bat_v", null);
            parsed.Add("Battery_status", null);




            return parsed;
        }
    }
}
