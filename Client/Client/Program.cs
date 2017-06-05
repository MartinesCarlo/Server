using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;

        static void Main(string[] args)
        {
            new Client(); 
            
        }

        public Client()
        {
            connect();
        }

        private void connect()
        {
            Console.WriteLine("[Try to connect to server...]");
            client = new TcpClient();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"),10000);           
            try
            {
                client.Connect(ipEnd);
                if (client.Connected)
                {
                    Console.WriteLine("[Connected]");
                    stream = client.GetStream();
                    sendMessage();
                }
                else
                {
                    Console.WriteLine("[Failed connection]");
                    Console.ReadLine();
                }
            } catch(Exception ex)
            {
                Console.WriteLine("[connectioin error: "+ex.Message.ToString()+ "]");
                Console.ReadLine();
            }
            
        }
        private void sendMessage()
        {

            string m = Console.ReadLine();
            byte[] message = Encoding.Unicode.GetBytes(m);
            stream.Write(message, 0, message.Length);
            receivedMessage();
        }

        private void receivedMessage()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string message = Encoding.Unicode.GetString(buffer, 0, data);
            Console.WriteLine(message);
            sendMessage();
        }
    }
}
