using lht = mqtt_JSON;
using static mqtt_parser.interfaces;
using Newtonsoft.Json;

namespace mqtt_parser
{
    internal class LHT_parse : INode_parse
    {
        double maximum_lux = 65535;
        public Dictionary<string, object?> data(string JSON)
        {
            Dictionary<string, object?> parsed = new Dictionary<string, object?>();

            lht.Root root = JsonConvert.DeserializeObject<lht.Root>(JSON);
            lht.DecodedPayload results_lht = root.uplink_message.decoded_payload;
            List<lht.RxMetadatum> location_lht = root.uplink_message.rx_metadata;
            lht.EndDeviceIds loc = root.end_device_ids;

            parsed.Add("Node_ID", loc.device_id);
            parsed.Add("Time", DateTime.Now);
            parsed.Add("Pressure", null);

            if (results_lht.ILL_lx != null)
            {

                double light_percent = ((double)results_lht.ILL_lx) <= 0 ? 0: (double.Log10((double)results_lht.ILL_lx) / double.Log10(maximum_lux)) * 100;
                light_percent = double.Round(light_percent, 2);
                light_percent = light_percent > 100 ? 100 : light_percent;
                parsed.Add("Illumination", light_percent);
            }
            else parsed.Add("Illumination", null);

            parsed.Add("Humidity", results_lht.Hum_SHT);

            string city = "unavailable";
            double? lat = null;
            double? lng = null;
            double? alt = null;
            try
            {
                int index = location_lht[0].gateway_ids.gateway_id == "packetbroker" ? 1 : 0;
                city = location_lht[index].gateway_ids.gateway_id;
                lat = location_lht[index].location.latitude;
                lng = location_lht[index].location.longitude;
                alt = location_lht[index].location.altitude;

            }
            catch
            {                
                city = loc.device_id;
            }

            string[] gibberish = { "mkr-", "lht-" };

            // Remove substrings
            foreach (var item in gibberish)
            {
                city = city.Replace(item, "", StringComparison.OrdinalIgnoreCase);
            }
            
            parsed.Add("Location", city);
            //parsed.Add("time_stamp", DateTime.Parse(location_lht[0].time));
            //parsed.Add("Gateway_lon", location_lht[0].location.longitude);
            //parsed.Add("Gateway_lat", location_lht[0].location.latitude);
            
            parsed.Add("Temperature_indoor", results_lht.TempC_SHT);
            parsed.Add("Temperature_outdoor", results_lht.TempC_DS);
                    

            parsed.Add("Bat_v", results_lht.BatV);
            parsed.Add("Battery_status", results_lht.Bat_status);

            parsed.Add("lat", lat);
            parsed.Add("lng", lng);
            parsed.Add("alt", alt);

            return parsed;
        }
    }
}
