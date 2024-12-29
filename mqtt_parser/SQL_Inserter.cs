using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static mqtt_parser.interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SQL_Interface
{
    public class Inserter: ISQL_QueryBuilder
    {
        public Inserter() {
            _queries = new Queue<string> ();
            _connection = new SqlConnection(
                "Server=weather-thingy-db.c5s2qgq8eg3x.us-east-1.rds.amazonaws.com,1433;" +
                "Database=LoRa;" +
                "User=admin;" +
                "Password=N5F3EVU9^s#fc;" +
                "Encrypt=true;" +
                "TrustServerCertificate=True;" +
                "Connection Timeout=30;"
                );
            insert_data_running = false;
        }

        public void connect()
        {
            _connection.Open();
        }

        public void disconnect()
        {
            _connection.Close();
        }

        public Task execute()
        {
            var command = new SqlCommand();

            command.Connection = _connection;
            command.CommandType = CommandType.Text;
            command.CommandText = _queries.Dequeue();

            return command.ExecuteReaderAsync();
        }

        public async Task insert_data()
        {
            insert_data_running = true;
            while(_queries.Count != 0)
            {
                await execute();
            }
            insert_data_running= false;

        }

        bool insert_data_running;
        Queue<string> _queries;
        SqlConnection _connection;
        public string build(string Node_ID, DateTime Time, double? Pressure, double? Illumination, double? Humidity, string Location, double Temperature_indor, double? Temperature_outdor)
        {
            string query_text = $"INSERT INTO lr2.Node (Node_ID, Time, Pressure, Illumination, Humidity, Location, Temperature_indor, Temperature_outdor) VALUES ('{Node_ID}','{Time}',{(Pressure != null ? Pressure : "NULL")}, {(Illumination != null ? Illumination : "NULL")},{(Humidity != null ? Humidity : "NULL")}, '{Location}',{Temperature_indor} , {(Temperature_outdor != null ? Temperature_outdor : "NULL")});";
            _queries.Enqueue(query_text);
            if (!insert_data_running) Task.Run(insert_data);
            return query_text;
        }
        public string build(string Node_ID, string Location, double? Battery_status)
        {

            //string query_text = $"INSERT INTO lr2.Gateway_location (Node_ID, Location, Battery_status) VALUES ('{Node_ID}','{Location}',{(Battery_status != null? Battery_status:"NULL")})";
            string query_text = $"IF NOT EXISTS (SELECT 1 FROM lr2.Gateway_location WHERE Node_ID = '{Location}') BEGIN INSERT INTO lr2.Gateway_location (Node_ID, Location) VALUES ('{Node_ID}','{Location}'); END; UPDATE lr2.Gateway_location SET Battery_status = {(Battery_status != null ? Battery_status : "NULL")};";
            _queries.Enqueue(query_text);
            if (!insert_data_running) Task.Run(insert_data);
            return query_text;
        }
    }
}

