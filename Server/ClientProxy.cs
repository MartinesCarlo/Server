using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientProxy
    {
        private Socket socket = null;

        public ClientProxy(Socket s)
        {
            this.socket = s;
        }
    }
}
