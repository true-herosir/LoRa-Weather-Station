using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mqtt_parser.interfaces;

namespace mqtt_parser
{
    internal class sql_com : ISQL_communicator
    {
        public sql_com() {
            m_logger = new logger();
        }

        public sql_com(string log_file)
        {
            m_logger = new logger(log_file);
        }

        static async Task communicate (string query, string table_name)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "weather-thingy-db.c5s2qgq8eg3x.us-east-1.rds.amazonaws.com,1433",
                UserID = "admin",
                Password = "",
                InitialCatalog = "LoRa",
                Encrypt = true, // Enable SSL encryption
                TrustServerCertificate = true // Ensure the certificate is verified
            };

            var connectionString = builder.ConnectionString;
            try
            {
                await using var connection = new SqlConnection(connectionString);
                //Console.WriteLine("\nQuery data example:");
                //Console.WriteLine("=========================================\n");

                await connection.OpenAsync();

                
                await using var command = new SqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                   Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    
                }
                await connection.CloseAsync();
            }
            catch (SqlException e)
            {
                //Console.WriteLine($"SQL Error: {e.Message}");
                m_logger.log_time($"SQL Error: {e.Message}");
                m_logger.log($"query:\n{query}");
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                m_logger.log_time(e.ToString());
                m_logger.log($"query:\n{query}");
            }

            //Console.WriteLine($"\n+++Done updating {table_name}+++");
            m_logger.log_time($"Done updating {table_name}");
        }

        public async Task build(string Node_ID, DateTime Time, double? Pressure, double? Illumination, double? Humidity, string Location, double Temperature_indor, double? Temperature_outdor)
        {
            string time_formated = Time.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            string query_text = $"INSERT INTO lr2.Node (Node_ID, Time, Pressure, Illumination, Humidity, Location, Temperature_indor, Temperature_outdor) VALUES ('{Node_ID}','{time_formated}',{(Pressure != null ? Pressure : "NULL")}, {(Illumination != null ? Illumination : "NULL")},{(Humidity != null ? Humidity : "NULL")}, '{Location}',{Temperature_indor} , {(Temperature_outdor != null ? Temperature_outdor : "NULL")});";
            //Console.WriteLine(query_text);
            await communicate(query_text, "Node");
            
        }

        public async Task build(string Node_ID, string Location, double? Battery_status)
        {
            string query_text = $"IF NOT EXISTS (SELECT Node_ID FROM lr2.Gateway_location WHERE Node_ID = '{Node_ID}') BEGIN INSERT INTO lr2.Gateway_location (Node_ID, Location) VALUES ('{Node_ID}','{Location}'); END; UPDATE lr2.Gateway_location SET Battery_status = {(Battery_status != null ? Battery_status : "NULL")} WHERE Node_ID = '{Node_ID}';";
            //Console.WriteLine(query_text);
            await communicate(query_text, "Gateway_location");
        }

        private static ILogger m_logger;
    }
}
