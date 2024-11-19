
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using mqtt_parser;
using mkr = mqtt_parser.mkr_Parser;
using lht = mqtt_parser.lht_Parser;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;

class Program
{
    
    


    static async Task Main(string[] args)
    {
        
        string test_mkr = "{\"end_device_ids\":{\"device_id\":\"mkr-saxion\",\"application_ids\":{\"application_id\":\"project-software-engineering\"},\"dev_eui\":\"A8610A353929630F\",\"join_eui\":\"A000000000000100\",\"dev_addr\":\"260B3587\"},\"correlation_ids\":[\"pba:uplink:01JD1T13ZK5PCTVZ3QPJXN4ZMQ\"],\"received_at\":\"2024-11-19T08:53:36.582021368Z\",\"uplink_message\":{\"session_key_id\":\"AZMGUWskX5R2Z2asRsEJ3A==\",\"f_port\":2,\"f_cnt\":3428,\"frm_payload\":\"S0gVACY=\",\"decoded_payload\":{\"humidity\":38, \"light\":72, \"pressure\":987.5, \"temperature\":21},\"rx_metadata\":[{\"gateway_ids\":{\"gateway_id\":\"packetbroker\"},\"packet_broker\":{\"message_id\":\"01JD1T13ZK5PCTVZ3QPJXN4ZMQ\",\"forwarder_net_id\":\"000013\",\"forwarder_tenant_id\":\"ttnv2\",\"forwarder_cluster_id\":\"ttn-v2-legacy-eu\",\"forwarder_gateway_eui\":\"AA555A000806053F\",\"forwarder_gateway_id\":\"eui-aa555a000806053f\",\"home_network_net_id\":\"000013\",\"home_network_tenant_id\":\"ttn\",\"home_network_cluster_id\":\"eu1.cloud.thethings.network\"},\"time\":\"2024-11-19T08:53:36.350200Z\",\"rssi\":-103,\"channel_rssi\":-103,\"snr\":9,\"location\":{\"latitude\":52.22121,\"longitude\":6.8857374,\"altitude\":66},\"uplink_token\":\"CucCZXlKaGJHY2lPaUpCTVRJNFIwTk5TMWNpTENKbGJtTWlPaUpCTVRJNFIwTk5JaXdpYVhZaU9pSnJXV1ZKZFdkemJFZHpXR2hSTkdob0lpd2lkR0ZuSWpvaWVIbDNXVUZzYldWcU5UUTVWbkpUUXpGYWExZE5RU0o5LkR4MHRhd2lLWGdpWGU4d0hTd29samcuaEVYcldPWHNrNTNxbjZldS54UWs1b01DeEdrb1JhUWNYQnhDX24xci02RzVhcm42QUR2Z2lkU1Q1enFUSkl6ZjUwQ0RmUjgxeGxGeUpCLWdNb2FYQkduYlVfNW5uTnVvQzNkMk0xSTZzbjdnMkQ1Z3FLZGFjQWhldTdieEVyNERCNndNZHpieUNSNUMyOEloVkRUd1Jsa3ZCQ3B1OGRDU2c3S1l2VDUzbkZLc09HaDd1R2RWUVlsNERuZ2pxSlFZLjVFbVFVUE03NkVreUNwMVF6NVpZU2caHgoDAAATEgV0dG52MhoQdHRuLXYyLWxlZ2FjeS1ldQ==\",\"received_at\":\"2024-11-19T08:53:36.367918426Z\"},{\"gateway_ids\":{\"gateway_id\":\"centrum-enschede\",\"eui\":\"AC1F09FFFE057EE0\"},\"time\":\"2024-11-19T08:53:36.351258993Z\",\"timestamp\":3349041279,\"rssi\":-109,\"channel_rssi\":-109,\"snr\":3.5,\"location\":{\"latitude\":52.2212025684184,\"longitude\":6.88635438680649,\"altitude\":70,\"source\":\"SOURCE_REGISTRY\"},\"uplink_token\":\"Ch4KHAoQY2VudHJ1bS1lbnNjaGVkZRIIrB8J//4FfuAQ/6D5vAwaDAiQpPG5BhCi6LyzASCY4KmTvNsC\",\"received_at\":\"2024-11-19T08:53:36.364014491Z\"}],\"settings\":{\"data_rate\":{\"lora\":{\"bandwidth\":125000, \"spreading_factor\":7, \"coding_rate\":\"4/5\"}}, \"frequency\":\"867700000\"},\"received_at\":\"2024-11-19T08:53:36.374268674Z\",\"confirmed\":true,\"consumed_airtime\":\"0.051456s\",\"version_ids\":{\"brand_id\":\"arduino\", \"model_id\":\"mkr-wan-1310\", \"hardware_version\":\"1.0\", \"firmware_version\":\"1.2.0\", \"band_id\":\"EU_863_870\"},\"network_ids\":{\"net_id\":\"000013\",\"ns_id\":\"EC656E0000000181\",\"tenant_id\":\"ttn\",\"cluster_id\":\"eu1\",\"cluster_address\":\"eu1.cloud.thethings.network\"}}}";

        // Deserialize the Root object
        mkr.Root root = JsonConvert.DeserializeObject<mkr.Root>(test_mkr);

        // Access the decoded_payload
        mkr.DecodedPayload results_mkr = root.uplink_message.decoded_payload;
        List<mkr.RxMetadatum> location_mkr = root.uplink_message.rx_metadata;

        Console.WriteLine("MKR now \n");
        Console.WriteLine($"humedity is:{results_mkr.humidity}");
        Console.WriteLine($"ill lux:{results_mkr.light}");
        Console.WriteLine($"pressure is:{results_mkr.pressure}");
        Console.WriteLine($"temp is:{results_mkr.temperature}");
        Console.WriteLine($"location is:{location_mkr[1].gateway_ids.gateway_id}");
        Console.WriteLine($"time is:{DateTime.Parse(location_mkr[0].time.ToString())}");

        string test_lht = "{\"end_device_ids\":{\"device_id\":\"lht-wierden\",\"application_ids\":{\"application_id\":\"project-software-engineering\"},\"dev_eui\":\"A840414E618350B1\",\"join_eui\":\"A000000000000100\",\"dev_addr\":\"260BCC18\"},\"correlation_ids\":[\"gs:uplink:01JD1TFZGTW6G8JY53Z06N8KTW\"],\"received_at\":\"2024-11-19T09:01:43.528371599Z\",\"uplink_message\":{\"session_key_id\":\"AXxMYtR3CkBWclq0v9WHwQ==\",\"f_port\":2,\"f_cnt\":82199,\"frm_payload\":\"y9MBMAO5BQEVf/8=\",\"decoded_payload\":{\"BatV\":3.027, \"Bat_status\":3, \"Hum_SHT\":95.3, \"ILL_lx\":277, \"TempC_SHT\":3.04, \"Work_mode\":\"Illumination Sensor\"},\"rx_metadata\":[{\"gateway_ids\":{\"gateway_id\":\"slot-wierden\",\"eui\":\"58A0CBFFFE8005DA\"},\"time\":\"2024-11-19T09:01:43.259736061Z\",\"timestamp\":1487573204,\"rssi\":-72,\"channel_rssi\":-72,\"snr\":8,\"location\":{\"latitude\":52.36891310514845,\"longitude\":6.602898538112641,\"altitude\":10,\"source\":\"SOURCE_REGISTRY\"},\"uplink_token\":\"ChoKGAoMc2xvdC13aWVyZGVuEghYoMv//oAF2hDUoarFBRoMCPen8bkGEP7Rv5kBIKD4ltKl9RE=\",\"received_at\":\"2024-11-19T09:01:43.253524557Z\"}],\"settings\":{\"data_rate\":{\"lora\":{\"bandwidth\":125000, \"spreading_factor\":7, \"coding_rate\":\"4/5\"}}, \"frequency\":\"867900000\", \"timestamp\":1487573204, \"time\":\"2024-11-19T09:01:43.259736061Z\"},\"received_at\":\"2024-11-19T09:01:43.324109106Z\",\"consumed_airtime\":\"0.061696s\",\"locations\":{\"user\":{\"latitude\":52.368744969360165,\"longitude\":6.602897674665767,\"source\":\"SOURCE_REGISTRY\"}},\"version_ids\":{\"brand_id\":\"dragino\", \"model_id\":\"lht65\", \"hardware_version\":\"_unknown_hw_version_\", \"firmware_version\":\"1.8\", \"band_id\":\"EU_863_870\"},\"network_ids\":{\"net_id\":\"000013\",\"ns_id\":\"EC656E0000000181\",\"tenant_id\":\"ttn\",\"cluster_id\":\"eu1\",\"cluster_address\":\"eu1.cloud.thethings.network\"}}}";
        // Deserialize the Root object
        lht.Root root2 = JsonConvert.DeserializeObject<lht.Root>(test_lht);
      


        // Access the decoded_payload
        lht.DecodedPayload results_lht = root2.uplink_message.decoded_payload;
        List<lht.RxMetadatum> location_lht = root2.uplink_message.rx_metadata;

        Console.WriteLine("LHT now \n");
        Console.WriteLine($"humedity is:{results_lht.Hum_SHT}");
        Console.WriteLine($"ill lux:{results_lht.ILL_lx}");
        Console.WriteLine($"bat v is:{results_lht.BatV}");
        Console.WriteLine($"bat stat is:{results_lht.Bat_status}");
        Console.WriteLine($"temp is:{results_lht.TempC_SHT}");
        Console.WriteLine($"mode is:{results_lht.Work_mode}");
        Console.WriteLine($"location is:{location_lht[0].gateway_ids.gateway_id}");
        Console.WriteLine($"time is:{DateTime.Parse(location_lht[0].time)}");


        string broker = "eu1.cloud.thethings.network";
        int port = 1883;
        string clientId = Guid.NewGuid().ToString();
        string topic = "v3/project-software-engineering@ttn/devices/mkr-saxion/#";
        string username = "project-software-engineering@ttn";
        string password = "NNSXS.DTT4HTNBXEQDZ4QYU6SG73Q2OXCERCZ6574RVXI.CQE6IG6FYNJOO2MOFMXZVWZE4GXTCC2YXNQNFDLQL4APZMWU6ZGA";

        // Create a MQTT client factory
        var factory = new MqttFactory();

        // Create a MQTT client instance
        var mqttClient = factory.CreateMqttClient();

        // Create MQTT client options
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port) // MQTT broker address and port
            .WithCredentials(username, password) // Set username and password
            .WithClientId(clientId)
            .WithCleanSession()
            .Build();

        // Connect to MQTT broker
        // Callback for when a message is received
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
            return Task.CompletedTask;
        };

        // Connect to MQTT broker
        var connectResult = await mqttClient.ConnectAsync(options);

        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to MQTT broker successfully.");

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(topic);

            // Callback function when a message is received
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                return Task.CompletedTask;
            };

        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
        }

        Console.WriteLine("Listening for messages. Press Ctrl+C to exit.");
        await Task.Delay(-1); // Infinite delay to keep the application running
        /*
        Console.WriteLine("Press Ctrl+C to exit.");
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true; // Prevent immediate exit
            cts.Cancel();
        };

        try
        {
            await Task.Delay(Timeout.Infinite, cts.Token); // Wait indefinitely until cancellation
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Application is shutting down...");
        }

        // Disconnect MQTT client
        await mqttClient.DisconnectAsync();
        */
        Console.WriteLine("Listening for messages. Press Ctrl+C to exit.");
        await Task.Delay(-1); // Infinite delay to keep the application running

    }
}