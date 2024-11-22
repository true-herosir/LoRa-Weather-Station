using lht = mqtt_JSON;
using static mqtt_parser.interfaces;
using Newtonsoft.Json;

namespace mqtt_parser
{
    internal class LHT_parse : INode_parse
    {
        public Dictionary<string, object> data(string JSON)
        {
            Dictionary<string, object> parsed = new Dictionary<string, object>();
            lht.Root root = JsonConvert.DeserializeObject<lht.Root>(JSON);
            lht.DecodedPayload results_lht = root.uplink_message.decoded_payload;
            List<lht.RxMetadatum> location_lht = root.uplink_message.rx_metadata;

            parsed.Add("sensor type", "LHT");
            string city = "unavailable";
            try
            {
                int index = location_lht[0].gateway_ids.gateway_id == "packetbroker" ? 1 : 0;
                city = location_lht[index].gateway_ids.gateway_id;
            }
            catch
            {
                lht.EndDeviceIds loc = root.end_device_ids;
                city = loc.device_id;
            }

            string[] gibberish = { "mkr-", "lht-", "centrum", "slot", "lora", "-" };

            // Remove substrings
            foreach (var item in gibberish)
            {
                city = city.Replace(item, "", StringComparison.OrdinalIgnoreCase);
            }
            city = char.ToUpper(city[0]) + city.Substring(1);

            parsed.Add("City", city);
            //parsed.Add("time_stamp", DateTime.Parse(location_lht[0].time));
            parsed.Add("Gateway_lon", location_lht[0].location.longitude);
            parsed.Add("Gateway_lat", location_lht[0].location.latitude);
            parsed.Add("Time_stamp", DateTime.Now);


            parsed.Add("Temp_in", results_lht.TempC_SHT);
            if (results_lht.TempC_DS != null) parsed.Add("Temp_out", results_lht.TempC_DS);
            else parsed.Add("Temp_out", null);
            parsed.Add("Humidity", results_lht.Hum_SHT);
            if (results_lht.ILL_lx != null) parsed.Add("Light_intensity_%", double.Round((double)results_lht.ILL_lx / 18.5, 2));
            parsed.Add("Bat_v", results_lht.BatV);
            parsed.Add("Bat_stat", results_lht.Bat_status);


            return parsed;
        }
    }
}
