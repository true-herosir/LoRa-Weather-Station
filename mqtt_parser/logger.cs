using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static mqtt_parser.interfaces;

namespace mqtt_parser
{
    internal class logger : ILogger
    {
        public logger(string file_name)
        {
            m_file_name = file_name;
        }

        public logger()
        {
            string day = DateTime.Now.ToString("yyyy-MM-dd");
            m_file_name = $"log({day}).txt";
        }
        void ILogger.log_time(string messages)
        {
            try
            {
                messages = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)}] -- {messages}\n";
                // Append the content to the file. If the file does not exist, it will be created.
                File.AppendAllText(m_file_name, messages);

                //Console.WriteLine($"Log updated at [{DateTime.Now}]\n");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                Console.WriteLine("An error occurred while writing to the log file:");
                Console.WriteLine(ex.Message);
            }
        }

        void ILogger.log(string messages)
        {
            try
            {
                messages = $"{messages}\n";
                // Append the content to the file. If the file does not exist, it will be created.
                File.AppendAllText(m_file_name, messages);

                //Console.WriteLine($"Log updated at [{DateTime.Now}]\n");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                Console.WriteLine("An error occurred while writing to the log file:");
                Console.WriteLine(ex.Message);
            }
        }

        void ILogger.change_file(string file_name)
        {
            m_file_name = file_name;
        }

        string m_file_name;
    }
}
