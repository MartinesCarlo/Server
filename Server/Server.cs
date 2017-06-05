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
            //connection = new OleDbConnection(Properties.Settings.Default.db1);
            connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\Schule\\CSharp\\Server\\Server\\bin\\Debug\\Datenbank.accdb;Persist Security Info = False; ");
            adapter = new OleDbDataAdapter("select * from spieler", connection);
            ds = new DataSet();
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
            System.Console.WriteLine("[Listening on port " + serverListenPort + "...]");
            while (true)
            {
                try
                {
                listener.Start();
                
                    while (!listener.Pending()) { Thread.Sleep(sleepTime); }
                    socket = listener.AcceptSocket();
                    vieleClients.Add(new ClientProxy(socket,this));
                    
                    System.Console.WriteLine("[New client connected (" +
                                "IP: " + socket.RemoteEndPoint + ", Port: " + ((IPEndPoint)socket.LocalEndPoint).Port.ToString() + ")]");

                

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
        public bool pruefeAnmeldung(string benutzer, string pw)
        {
            Boolean ok = false;
            foreach(Spieler sp in vieleSpieler)
            {
                if(sp.Benutzername.Equals(benutzer.ToLower()) || sp.Kennwort.Equals(pw))
                {
                    ok = true;
                    break;                
                }                  
            }
            return ok;
        }
        public bool registrierung(string benutzer,string pw)
        {
            Boolean ok = false;           
            if(pruefeBenutzername(benutzer))
            {
                vieleSpieler.Add(new Spieler(benutzer.ToLower(), pw));
                ok = true;
            }
            return ok;
        }

        private bool pruefeBenutzername(string benutzer)
        {
            Boolean ok = true;
            foreach (Spieler sp in vieleSpieler)
            {
                if (sp.Benutzername.Equals(benutzer.ToLower()))
                {
                    ok = false;
                    break;
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