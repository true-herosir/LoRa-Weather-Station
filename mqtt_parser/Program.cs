
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using mkr = mqtt_JSON.mkr_JSON;
using lht = mqtt_JSON.lht_JSON;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using static mqtt_parser.interfaces;
using mqtt_parser;

class Program
{
    
    


    static async Task Main(string[] args)
    {

        string broker = "eu1.cloud.thethings.network";
        int port = 1883;
        string clientId = Guid.NewGuid().ToString();
        string topic = "v3/project-software-engineering@ttn/devices/#";
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

        ////////////////our sensor broker/////////////////////
        string broker_g4 = "eu1.cloud.thethings.network";
        int port_g4 = 1883;
        string clientId_g4 = Guid.NewGuid().ToString();
        string topic_g4 = "v3/weather-thingy-group4-2024@ttn/devices/weather-thingy-g4-2024/up";
        string username_g4 = "weather-thingy-group4-2024@ttn";
        string password_g4 = "NNSXS.76MNJK7GRMLRDFE3YYB6RA7PBHOOOM4UGOWBTXQ.A37U73JN7H34DVJ6VE2HQWKQDAMTNFL4235UU5PJOSPRZC6NDY4Q";
        // Create a MQTT client factory
        var factory_g4 = new MqttFactory();

        // Create a MQTT client instance
        var mqttClient_g4 = factory_g4.CreateMqttClient();

        // Create MQTT client options
        var options_g4 = new MqttClientOptionsBuilder()
            .WithTcpServer(broker_g4, port_g4) // MQTT broker address and port
            .WithCredentials(username_g4, password_g4) // Set username and password
            .WithClientId(clientId_g4)
            .WithCleanSession()
            .Build();


        // Connect to MQTT broker
        var connectResult = await mqttClient.ConnectAsync(options);
        var connectResult_g4 = await mqttClient_g4.ConnectAsync(options_g4);


        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to MQTT broker successfully.");

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(topic);
        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
        }

        if (connectResult_g4.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to G4 MQTT broker successfully.");

            // Subscribe to a topic
            await mqttClient_g4.SubscribeAsync(topic_g4);
        }
        else
        {
            Console.WriteLine($"Failed to connect to G4 MQTT broker: {connectResult_g4.ResultCode}");
        }

        Console.WriteLine("Listening for messages. Press Ctrl+C to exit.");
        // Callback for when a message is received
        string received = " ";
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            received = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            //Console.WriteLine($"Received message:\n{received}");
            try
            {   
                if (received.Contains("mkr-"))
                {
                    Console.WriteLine($"Received message from MKR sensor:");
                    IMKR_parse MKR_Parsed = new MKR_parse();
                    Dictionary<string, object> MKR = MKR_Parsed.data(received);

                    foreach (var item in MKR)
                    {
                        Console.WriteLine($"{item.Key} is: {item.Value}");
                    }


                }

                if (received.Contains("lht-"))
                {
                    Console.WriteLine($"Received message from LHT sensor:");
                    //Console.WriteLine(received);
                    ILHT_parse LHT_Parsed = new LHT_parse();
                    Dictionary<string, object> LHT = LHT_Parsed.data(received);
                    foreach (var item in LHT)
                    {
                        Console.WriteLine($"{item.Key} is: {item.Value}");
                    }

                }

                Console.WriteLine($"success.\n");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Type: " + ex.GetType().Name);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine(received);
            }
            return Task.CompletedTask;
        };


        string us_received = " ";
        mqttClient_g4.ApplicationMessageReceivedAsync += e =>
        {
            us_received = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            //Console.WriteLine($"Received message:\n{received}");
            try
            {
                Console.WriteLine($"Received message from G4 MKR sensor:");
                
                IMKR_parse MKR_Parsed_G4 = new MKR_parse();
                Dictionary<string, object> MKR_g4 = MKR_Parsed_G4.data(us_received);
                //Console.WriteLine(us_received);

                foreach (var item in MKR_g4)
                {
                    Console.WriteLine($"{item.Key} is: {item.Value}");
                }

                Console.WriteLine($"success.\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Type: " + ex.GetType().Name);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine(us_received);
            }
            return Task.CompletedTask;
        };

        await Task.Delay(-1); // Infinite delay to keep the application running



        
    }
}