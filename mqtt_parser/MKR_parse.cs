using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mkr = mqtt_JSON.mkr_JSON;
using static mqtt_parser.interfaces;
using System.Threading.Tasks;

namespace mqtt_parser
{
    internal class MKR_parse : IMKR_parse
    {
        public Dictionary<string, object> data(string JSON)
        {
            Dictionary<string, object> parsed = new Dictionary<string, object>();
            mkr.Root root = JsonConvert.DeserializeObject<mkr.Root>(JSON);
            mkr.DecodedPayload results_mkr = root.uplink_message.decoded_payload;
            List<mkr.RxMetadatum> location_mkr = root.uplink_message.rx_metadata;
            mkr.EndDeviceIds loc = root.end_device_ids;
            parsed.Add("sensor type", "MKR");
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

            parsed.Add("City", city);
            //parsed.Add("time_stamp", DateTime.Parse(location_lht[0].time));
            parsed.Add("Gateway lon", location_mkr[0].location.longitude);
            parsed.Add("Gateway lat", location_mkr[0].location.latitude);
            parsed.Add("Time_stamp", DateTime.Now);

            parsed.Add("Temp_in", results_mkr.temperature);
            parsed.Add("Humidity", results_mkr.humidity);
            parsed.Add("Light_intensity_%", double.Round(results_mkr.light / 2.55, 2));
            parsed.Add("Pressure", results_mkr.pressure);
            
            

            
            return parsed;
        }
    }
}
