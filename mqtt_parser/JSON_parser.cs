
namespace mqtt_JSON
{
    public class DecodedPayload
    {
        public double? BatV { get; set; }
        public double? Bat_status { get; set; }
        public double? Hum_SHT { get; set; }
        public double? ILL_lx { get; set; }
        public double? TempC_SHT { get; set; }
        public double? TempC_DS { get; set; }
        public double? humidity { get; set; }
        public double? light { get; set; }
        public double? pressure { get; set; }
        public double? temperature { get; set; }
    }

    public class EndDeviceIds
    {
        public string device_id { get; set; }
    }

    public class GatewayIds
    {
        public string gateway_id { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }


    public class Root
    {
        public EndDeviceIds end_device_ids { get; set; }
        public UplinkMessage uplink_message { get; set; }
    }

    public class RxMetadatum
    {
        public GatewayIds gateway_ids { get; set; }
        public Location location { get; set; }
    }

    public class UplinkMessage
    {
        public DecodedPayload decoded_payload { get; set; }
        public List<RxMetadatum> rx_metadata { get; set; }
    }

}