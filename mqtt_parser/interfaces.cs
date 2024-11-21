using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_parser
{
    internal class interfaces
    {
        public interface ILHT_parse
        {
            Dictionary<string, object> data(string JSON);

        }

        public interface IMKR_parse
        {
            Dictionary<string, object> data(string JSON);

        }
            
    }
}
