using System.Text;
using MQTTnet;
using MQTTnet.Client;
using static mqtt_parser.interfaces;
using mqtt_parser;
using System.Globalization;

class Program
{
    private static ILogger logger = new logger();
    static string day = DateTime.Now.ToString("yyyy-MM-dd");
    static async Task Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture; // setting the language settings to more generic / universal


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
            logger.log_time("Connected to MQTT broker successfully.");
            // Subscribe to a topic
            await mqttClient.SubscribeAsync(topic);
        }
        else
        {
            Console.WriteLine($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
            logger.log_time($"Failed to connect to MQTT broker: {connectResult.ResultCode}");
        }

        if (connectResult_g4.ResultCode == MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Connected to G4 MQTT broker successfully.");
            logger.log_time("Connected to G4 MQTT broker successfully.");
            // Subscribe to a topic
            await mqttClient_g4.SubscribeAsync(topic_g4);
        }
        else
        {
            Console.WriteLine($"Failed to connect to G4 MQTT broker: {connectResult_g4.ResultCode}");
            logger.log_time($"Failed to connect to G4 MQTT broker: {connectResult.ResultCode}");
        }


        Console.WriteLine("Listening for messages. Press Ctrl+C to exit.");
        // Callback for when a message is received
        string received = " ";
        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            if (day != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
                logger.change_file($"log({day}).txt");
            }

            received = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);           

            try
            {   
                if (received.Contains("mkr-"))
                {
                    //Console.WriteLine($"Received message from MKR sensor:");
                    logger.log_time("Received message from MKR sensor:");
                    INode_parse MKR_Parsed = new MKR_parse();
                    Dictionary<string, object?> MKR = MKR_Parsed.data(received);

                    foreach (var item in MKR)
                    {
                        //Console.WriteLine($"{item.Key}: {item.Value}");
                        logger.log($"{item.Key}: {item.Value}");
                    }

                    //ISQL_QueryBuilder database = new Inserter();
                    ISQL_communicator database = new sql_com($"log({day}).txt");
                    await database.build((string)MKR["Node_ID"], (string)MKR["Location"], (double?)MKR["Battery_status"]);
                    

                    await database.build((string)MKR["Node_ID"], (DateTime)MKR["Time"], (double?)MKR["Pressure"], (double?)MKR["Illumination"], (double?)MKR["Humidity"],
                        (string)MKR["Location"], (double?)MKR["Temperature_indoor"], (double?)MKR["Temperature_outdoor"]);
                    

                }

                if (received.Contains("lht-"))
                {
                    //Console.WriteLine($"Received message from LHT sensor:");
                    logger.log_time("Received message from LHT sensor:");

                    INode_parse LHT_Parsed = new LHT_parse();
                    Dictionary<string, object?> LHT = LHT_Parsed.data(received);
                    foreach (var item in LHT)
                    {
                        //Console.WriteLine($"{item.Key}: {item.Value}");
                        logger.log($"{item.Key}: {item.Value}");
                    }

                    //ISQL_QueryBuilder database = new Inserter();
                    ISQL_communicator database = new sql_com($"log({day}).txt");
                    await database.build((string)LHT["Node_ID"], (string)LHT["Location"], (double?)LHT["Battery_status"]);


                    await database.build( (string)LHT["Node_ID"], (DateTime)LHT["Time"], (double?)LHT["Pressure"], (double?)LHT["Illumination"], (double?)LHT["Humidity"],
                        (string)LHT["Location"], (double?)LHT["Temperature_indoor"], (double?)LHT["Temperature_outdoor"]);
                    

                    
                }

                //Console.WriteLine("\n+++success.+++\n");
                logger.log_time("+++success.+++\n");


            }
            catch (Exception ex)
            {
                /*
                Console.WriteLine("Error Type: " + ex.GetType().Name);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine(received);
                */
                logger.log_time("Error Type: " + ex.GetType().Name);
                logger.log("Message: " + ex.Message);
                logger.log("Stack Trace: " + ex.StackTrace);
                logger.log(received);
            }
            //return Task.CompletedTask;
        };

        // if disconnect For mqttClient
        bool disconnected = false;
        mqttClient.DisconnectedAsync += async e =>
        {
            if (day != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
                logger.change_file($"log({day}).txt");
            }

            if (disconnected == false)
            {
                logger.log_time("Disconnected from MQTT broker. Entering reconnection loop.");
                Console.WriteLine("Disconnected from MQTT broker. Entering reconnection loop.");
                disconnected = true;
            }
            DateTime lastLogTime = DateTime.Now; // Track the last log time for error logging

            while (true) // Infinite loop to retry connection
            {
                try
                {
                    // Attempt to reconnect
                    connectResult = await mqttClient.ConnectAsync(options);
                    if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
                    {
                        logger.log_time("Reconnected to MQTT broker successfully.");
                        Console.WriteLine("Reconnected to MQTT broker successfully.");
                        disconnected = false;
                        await mqttClient.SubscribeAsync(topic);
                        logger.log_time($"Re-subscribed to topic.");
                        break; // Exit loop upon successful reconnection
                    }
                    else
                    {
                        // Log only if 10 minutes have passed since the last log
                        if ((DateTime.UtcNow - lastLogTime).TotalMinutes >= 10)
                        {
                            logger.log_time($"Reconnection attempt to failed: {connectResult.ResultCode}");
                            lastLogTime = DateTime.UtcNow; // Update the last log time
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log only if 10 minutes have passed since the last log
                    if ((DateTime.Now - lastLogTime).TotalMinutes >= 10)
                    {
                        logger.log_time($"Reconnection error to: {ex.Message}");
                        lastLogTime = DateTime.Now; // Update the last log time
                    }
                }

                // Wait for 10 seconds before retrying
                await Task.Delay(10000); // Non-blocking delay
            }
        };

        /// our sensor
        string us_received = " ";
        mqttClient_g4.ApplicationMessageReceivedAsync += async e =>
        {
            if (day != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
                logger.change_file($"log({day}).txt");
            }

            us_received = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            
            try
            {
                //Console.WriteLine($"Received message from G4 MKR sensor:");
                logger.log_time($"Received message from G4 MKR sensor:");

                INode_parse MKR_Parsed_G4 = new MKR_parse();
                Dictionary<string, object?> MKR_g4 = MKR_Parsed_G4.data(us_received);

                MKR_g4["Illumination"] = double.Round(((double)MKR_g4["Illumination"] * 2.55) / 6.6, 2); /// handle our sensor with max lux measured of 700
                MKR_g4["Illumination"] = (double)MKR_g4["Illumination"] > 100 ? 100 : (double)MKR_g4["Illumination"];

                foreach (var item in MKR_g4)
                {
                    //Console.WriteLine($"{item.Key}: {item.Value}");
                    logger.log($"{item.Key}: {item.Value}");
                }

                //ISQL_QueryBuilder database = new Inserter();
                ISQL_communicator database = new sql_com($"log({day}).txt");
                await database.build((string)MKR_g4["Node_ID"], (string)MKR_g4["Location"], (double?)MKR_g4["Battery_status"]);
                

                await database.build((string)MKR_g4["Node_ID"], (DateTime)MKR_g4["Time"], (double?)MKR_g4["Pressure"], (double?)MKR_g4["Illumination"], (double?)MKR_g4["Humidity"],
                    (string)MKR_g4["Location"], (double?)MKR_g4["Temperature_indoor"], (double?)MKR_g4["Temperature_outdoor"]);

                //Console.WriteLine("\n+++success.+++\n");
                logger.log_time("+++success.+++\n");

            }
            catch (Exception ex)
            {
                /*
                Console.WriteLine("Error Type: " + ex.GetType().Name);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine(received);
                */
                logger.log_time("Error Type: " + ex.GetType().Name);
                logger.log("Message: " + ex.Message);
                logger.log("Stack Trace: " + ex.StackTrace);
                logger.log(us_received);
            }
            //return Task.CompletedTask;
        };

        // if disconnected For mqttClient_g4
        bool g4_disconnect = false;
        mqttClient_g4.DisconnectedAsync += async e =>
        {
            if (day != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
                logger.change_file($"log({day}).txt");
            }

            if (g4_disconnect == false)
            {
                logger.log_time("Disconnected from G4 MQTT broker. Entering reconnection loop.");
                Console.WriteLine("Disconnected from G4 MQTT broker. Entering reconnection loop.");
                g4_disconnect = true;
            }

            DateTime lastLogTime = DateTime.Now; // Track the last log time for error logging

            while (true) // Infinite loop to retry connection
            {
                try
                {
                    // Attempt to reconnect
                    connectResult_g4 = await mqttClient_g4.ConnectAsync(options_g4);
                    if (connectResult_g4.ResultCode == MqttClientConnectResultCode.Success)
                    {
                        logger.log_time("Reconnected to G4 MQTT broker successfully.");
                        Console.WriteLine("Reconnected to G4 MQTT broker successfully.");
                        g4_disconnect = false;
                        await mqttClient_g4.SubscribeAsync(topic_g4);
                        logger.log_time($"Re-subscribed to G4 topic.");
                        break; // Exit loop upon successful reconnection
                    }
                    else
                    {
                        // Log only if 10 minutes have passed since the last log
                        if ((DateTime.UtcNow - lastLogTime).TotalMinutes >= 10)
                        {
                            logger.log_time($"Reconnection attempt to G4 failed: {connectResult_g4.ResultCode}");
                            lastLogTime = DateTime.UtcNow; // Update the last log time
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log only if 10 minutes have passed since the last log
                    if ((DateTime.Now - lastLogTime).TotalMinutes >= 10)
                    {
                        logger.log_time($"Reconnection error to G4: {ex.Message}");
                        lastLogTime = DateTime.Now; // Update the last log time
                    }
                }

                // Wait for 10 seconds before retrying
                await Task.Delay(10000); // Non-blocking delay
            }
        };

        await Task.Delay(-1); // Infinite delay to keep the application running

    }
}