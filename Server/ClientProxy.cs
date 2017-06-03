using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ClientProxy
    {
        private Socket socket;
        private Server server;
        private Thread thread;
        private StreamReader reader;
        

        public ClientProxy(Socket so,Server se)
        {
            this.socket = so;
            this.server = se;
            thread = new Thread(new ThreadStart(this.listener));
            thread.Start();
            

        }
        private void listener()
        {
            while(true)
            {
                
                Byte[] bytesReceived = new Byte[256];
                int bytes = 0;

                string nachricht;
                
                do
                {
                    bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                    nachricht = nachricht + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);

                pruefeNachricht(nachricht);
            }

        }

        private void pruefeNachricht(string nachricht)
        {
            String[] n = nachricht.Split(new Char[] { '#' });
            switch(n[0])
            {
                case "ANM":         // Anmeldung
                    if(server.pruefeAnmeldung(n[1], n[2]))
                    {
                        sendeNachricht("true");
                    }
                    else
                    {
                        sendeNachricht("false");
                    }
                    break;
                        
                    
            }

        }

        private void sendeNachricht(string v)
        {
            
        }
    }
}
