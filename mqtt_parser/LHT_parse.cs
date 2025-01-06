using lht = mqtt_JSON;
using static mqtt_parser.interfaces;
using Newtonsoft.Json;
using System;

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

            string gateway_id;
            double? lat = null;
            double? lng = null;
            double? alt = null;
            lat = location_lht[0].location.latitude;
            lng = location_lht[0].location.longitude;
            alt = location_lht[0].location.altitude;
            try
            {
                gateway_id = location_lht[0].gateway_ids.gateway_id;
            }
            catch { gateway_id = "unavailable"; }

            parsed.Add("Location", gateway_id);

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
