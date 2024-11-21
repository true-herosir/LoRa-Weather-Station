using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_parser
{
    namespace mkr_Parser
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class ApplicationIds
        {
            public string application_id { get; set; }
        }

        public class DataRate
        {
            public Lora lora { get; set; }
        }

        public class DecodedPayload
        {
            public int humidity { get; set; }
            public int light { get; set; }
            public double pressure { get; set; }
            public int temperature { get; set; }
        }

        public class EndDeviceIds
        {
            public string device_id { get; set; }
            public ApplicationIds application_ids { get; set; }
            public string dev_eui { get; set; }
            public string join_eui { get; set; }
            public string dev_addr { get; set; }
        }

        public class GatewayIds
        {
            public string gateway_id { get; set; }
            public string eui { get; set; }
        }

        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int altitude { get; set; }
            public string source { get; set; }
        }

        public class Lora
        {
            public int bandwidth { get; set; }
            public int spreading_factor { get; set; }
            public string coding_rate { get; set; }
        }

        public class NetworkIds
        {
            public string net_id { get; set; }
            public string ns_id { get; set; }
            public string tenant_id { get; set; }
            public string cluster_id { get; set; }
            public string cluster_address { get; set; }
        }

        public class PacketBroker
        {
            public string message_id { get; set; }
            public string forwarder_net_id { get; set; }
            public string forwarder_tenant_id { get; set; }
            public string forwarder_cluster_id { get; set; }
            public string forwarder_gateway_eui { get; set; }
            public string forwarder_gateway_id { get; set; }
            public string home_network_net_id { get; set; }
            public string home_network_tenant_id { get; set; }
            public string home_network_cluster_id { get; set; }
        }

        public class Root
        {
            public EndDeviceIds end_device_ids { get; set; }
            public List<string> correlation_ids { get; set; }
            public string received_at { get; set; }
            public UplinkMessage uplink_message { get; set; }
        }

        public class RxMetadatum
        {
            public GatewayIds gateway_ids { get; set; }
            public PacketBroker packet_broker { get; set; }
            public object time { get; set; }
            public int rssi { get; set; }
            public int channel_rssi { get; set; }
            public double snr { get; set; }
            public Location location { get; set; }
            public string uplink_token { get; set; }
            public string received_at { get; set; }
            public long? timestamp { get; set; }
        }

        public class Settings
        {
            public DataRate data_rate { get; set; }
            public string frequency { get; set; }
        }

        public class UplinkMessage
        {
            public string session_key_id { get; set; }
            public int f_port { get; set; }
            public int f_cnt { get; set; }
            public string frm_payload { get; set; }
            public DecodedPayload decoded_payload { get; set; }
            public List<RxMetadatum> rx_metadata { get; set; }
            public Settings settings { get; set; }
            public string received_at { get; set; }
            public bool confirmed { get; set; }
            public string consumed_airtime { get; set; }
            public VersionIds version_ids { get; set; }
            public NetworkIds network_ids { get; set; }
        }

        public class VersionIds
        {
            public string brand_id { get; set; }
            public string model_id { get; set; }
            public string hardware_version { get; set; }
            public string firmware_version { get; set; }
            public string band_id { get; set; }
        }
    }
    namespace lht_Parser
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class ApplicationIds
        {
            public string application_id { get; set; }
        }

        public class DataRate
        {
            public Lora lora { get; set; }
        }

        public class DecodedPayload
        {
            public double BatV { get; set; }
            public int Bat_status { get; set; }
            public double Hum_SHT { get; set; }
            public int ILL_lx { get; set; }
            public double TempC_SHT { get; set; }
            public string Work_mode { get; set; }
        }

        public class EndDeviceIds
        {
            public string device_id { get; set; }
            public ApplicationIds application_ids { get; set; }
            public string dev_eui { get; set; }
            public string join_eui { get; set; }
            public string dev_addr { get; set; }
        }

        public class GatewayIds
        {
            public string gateway_id { get; set; }
            public string eui { get; set; }
        }

        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int altitude { get; set; }
            public string source { get; set; }
        }

        public class Locations
        {
            public User user { get; set; }
        }

        public class Lora
        {
            public int bandwidth { get; set; }
            public int spreading_factor { get; set; }
            public string coding_rate { get; set; }
        }

        public class NetworkIds
        {
            public string net_id { get; set; }
            public string ns_id { get; set; }
            public string tenant_id { get; set; }
            public string cluster_id { get; set; }
            public string cluster_address { get; set; }
        }

        public class Root
        {
            public EndDeviceIds end_device_ids { get; set; }
            public List<string> correlation_ids { get; set; }
            public string received_at { get; set; }
            public UplinkMessage uplink_message { get; set; }
        }

        public class RxMetadatum
        {
            public GatewayIds gateway_ids { get; set; }
            public string time { get; set; }
            public int timestamp { get; set; }
            public int rssi { get; set; }
            public int channel_rssi { get; set; }
            public int snr { get; set; }
            public Location location { get; set; }
            public string uplink_token { get; set; }
            public string received_at { get; set; }
        }

        public class Settings
        {
            public DataRate data_rate { get; set; }
            public string frequency { get; set; }
            public int timestamp { get; set; }
            public string time { get; set; }
        }

        public class UplinkMessage
        {
            public string session_key_id { get; set; }
            public int f_port { get; set; }
            public int f_cnt { get; set; }
            public string frm_payload { get; set; }
            public DecodedPayload decoded_payload { get; set; }
            public List<RxMetadatum> rx_metadata { get; set; }
            public Settings settings { get; set; }
            public string received_at { get; set; }
            public string consumed_airtime { get; set; }
            public Locations locations { get; set; }
            public VersionIds version_ids { get; set; }
            public NetworkIds network_ids { get; set; }
        }

        public class User
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string source { get; set; }
        }

        public class VersionIds
        {
            public string brand_id { get; set; }
            public string model_id { get; set; }
            public string hardware_version { get; set; }
            public string firmware_version { get; set; }
            public string band_id { get; set; }
        }


    }
}
