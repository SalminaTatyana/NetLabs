using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    public class Connection
    {
        public int port { get; set; }
        public string ip { get; set; }
        public Connection(string ip,int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}
