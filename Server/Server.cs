using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Server
{
    public class Server
    {
        private const int serverListenPort = 10000;
        private const int sleepTime = 200;
        private IPAddress ipAddress = IPAddress.Any;
        private Thread mainThread;
        private List<ClientProxy> vieleClients;
        private Socket socket = null;
        private OleDbConnection connection;
        private OleDbDataAdapter adapter;
        private DataSet ds;
        private List<Spieler> vieleSpieler;


        public Server()
        {
            vieleClients = new List<ClientProxy>();
            mainThread = new Thread(new ThreadStart(this.mainListener));
            mainThread.Start();
            ladeDatenbank();
            Console.ReadLine();


        }

        private void ladeDatenbank()
        {
            connection = new OleDbConnection(Properties.Settings.Default.db1);
            adapter = new OleDbDataAdapter("select * from spieler", connection);
            adapter.Fill(ds, "Spieler");
            DataTableReader reader = ds.Tables["Spieler"].CreateDataReader();
            vieleSpieler = new List<Spieler>();
            while(reader.Read())
            {
                vieleSpieler.Add(new Spieler(Convert.ToInt32(reader[0]), reader[1].ToString(), reader[2].ToString()));
            }

        }

        private void mainListener()
        {
            TcpListener listener = new TcpListener(ipAddress, serverListenPort);
            System.Console.WriteLine("Listening on port " + serverListenPort + "...");
            while (true)
            {
                try
                {
                listener.Start();
                
                    while (!listener.Pending()) { Thread.Sleep(sleepTime); }
                    socket = listener.AcceptSocket();
                    vieleClients.Add(new ClientProxy(socket,this));
                    
                    System.Console.WriteLine("Neue Client-Verbindung (" +
                                "IP: " + socket.RemoteEndPoint + ", Port: " + ((IPEndPoint)socket.LocalEndPoint).Port.ToString() + ")");

                

                }
                catch(ThreadInterruptedException)
                {
                    Console.WriteLine("listener wird beendet");
                    return;

                }
                catch (Exception)
                {
                    mainThread.Interrupt();                    
                }
            }
        }
        public bool pruefeAnmeldung(String benutzer, String pw)
        {
            Boolean ok = false;

            foreach(Spieler sp in vieleSpieler)
            {
                if(sp.Benutzername.Equals(benutzer))
                {
                    if(sp.Kennwort.Equals(pw))
                    {
                        ok = true;
                        break;
                    }
                }
                    
            }

            return ok;
        }
        static void Main(string[] args)
        {
            new Server();
        }
    }
}