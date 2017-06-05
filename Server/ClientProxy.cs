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

                NetworkStream stream = new NetworkStream(socket);
                byte[] buffer = new byte[socket.ReceiveBufferSize];
                int data = stream.Read(buffer, 0, socket.ReceiveBufferSize);
                string massege = Encoding.Unicode.GetString(buffer, 0, data);
                //sendeNachricht("JA");
                pruefeNachricht(massege);
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
                        // Erfolgreich
                        sendeNachricht("ANM#true");
                    }
                    else
                    {
                        // Fehlgeschlagen
                        sendeNachricht("ANM#false");
                    }
                    break;
                case "REG":         // Registrierung
                    if(server.registrierung(n[1], n[2]))
                    {
                        // Erfolgreich
                        sendeNachricht("REG#true");
                    }
                    else
                    {
                        // Fehlgeschlagen
                        sendeNachricht("REG#false");
                    }
                    break;                                         
            }
            
        }
        private void sendeNachricht(string n)
        {
            if(socket.Connected)
            {
                NetworkStream stream = new NetworkStream(socket);
                Byte[] message = Encoding.Unicode.GetBytes(n);
                stream.Write(message, 0, message.Length);
            }
        }
    }
}
