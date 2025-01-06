using Newtonsoft.Json;
using mkr = mqtt_JSON;
using static mqtt_parser.interfaces;
using System;

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

            string gateway_id ;
            double? lat = null;
            double? lng = null;
            double? alt = null;
            lat = location_mkr[0].location.latitude;
            lng = location_mkr[0].location.longitude;
            alt = location_mkr[0].location.altitude;
            try
            {
                gateway_id = location_mkr[0].gateway_ids.gateway_id;
            }
            catch { gateway_id = "unavailable"; }

            parsed.Add("Location", gateway_id);



            parsed.Add("Temperature_indoor", results_mkr.temperature);
            parsed.Add("Temperature_outdoor", null);


            parsed.Add("Bat_v", null);
            parsed.Add("Battery_status", null);

            parsed.Add("lat", lat);
            parsed.Add("lng", lng);
            parsed.Add("alt", alt);


            return parsed;
        }
    }
}
