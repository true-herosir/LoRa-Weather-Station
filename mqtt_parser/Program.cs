
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
class Program
{
    static async Task Main(string[] args)
    {
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