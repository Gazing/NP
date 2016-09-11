using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using NillyProxy.Proxy;

namespace NillyProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Serializer.SerializePacketTypes();
                Serializer.SerializePacketIds();   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }

            ProxyServer ps = new ProxyServer(IPAddress.Parse("127.0.0.1"), 2052);
            ps.Start();
            Console.ReadLine();
        }
    }
}
