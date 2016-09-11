using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NillyProxy.Proxy
{
    public class ProxyServer
    {
        public IPAddress EndPointIP;
        public int EndPointPort;
        private TcpListener listener { get; set; }
        private bool isOn { get; set; }
        private int connectedClients;

        public string key1 = "311f80691451c71d09a13a2a6e";
        public string key2 = "72c5583cafb6818995cdd74b80";

        public static int BUFFER_SIZE = 16384;

        public ProxyServer(IPAddress ip, int port)
        {
            EndPointIP = ip;
            EndPointPort = port;
            listener = new TcpListener(EndPointIP, EndPointPort);
            isOn = false;
            connectedClients = 0;
        }

        public void Start()
        {
            isOn = true;
            listener.Start();
            Console.WriteLine("Proxy server started.");
            listener.BeginAcceptTcpClient(onAcceptConnection, null);
        }

        private void onAcceptConnection(IAsyncResult asyn)
        {
            connectedClients++;
            TcpClient client = listener.EndAcceptTcpClient(asyn);
            Console.WriteLine("Client connected.");
            ClientConnection cc = new ClientConnection(client, this);
            cc.clientId = connectedClients;
            cc.Connect();
            listener.BeginAcceptTcpClient(onAcceptConnection, null);
        }
    }
}
