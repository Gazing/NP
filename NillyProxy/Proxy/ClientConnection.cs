using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using NillyProxy.Packets;
using System.Net;
using System.IO;
using NillyProxy.Crypto;
using NillyProxy.Packets.ClientPackets;

namespace NillyProxy.Proxy
{
    public class ClientConnection
    {
        private TcpClient localConnection;
        private TcpClient remoteConnection;
        public int clientId;

        public string USNE = "104.236.220.138";

        private PacketBuffer localBuffer = new PacketBuffer();
        private PacketBuffer remoteBuffer = new PacketBuffer();
        private ProxyServer proxy;

        private RC4 ClientReceiveKey;

        private RC4 ServerReceiveKey;

        private RC4 ClientSendKey;

        private RC4 ServerSendKey;

        public ClientConnection(TcpClient client, ProxyServer proxy)
        {
            this.localConnection = client;
            this.remoteConnection = new TcpClient();
            this.proxy = proxy;
            this.ClientReceiveKey = new RC4(this.proxy.key1);
            this.ServerReceiveKey = new RC4(this.proxy.key2);
            this.ClientSendKey = new RC4(this.proxy.key2);
            this.ServerSendKey = new RC4(this.proxy.key1);
        }

        public void Connect()
        {
            remoteConnection.BeginConnect(IPAddress.Parse(USNE), proxy.EndPointPort, remoteConnected, null);
            //beginLocalRead(0, ProxyServer.BUFFER_SIZE);
        }

        private void remoteConnected(IAsyncResult asyn)
        {
            remoteConnection.EndConnect(asyn);

            Console.WriteLine("Client connected to remote server");
            beginRemoteRead(0, 4);
            beginLocalRead(0, 4);
        }

        private void beginLocalRead(int offset, int length)
        {
            try
            {
                this.localConnection.GetStream().BeginRead(localBuffer.Buffer(), offset, length, localRead, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot read from client - " + ex.ToString());
            }
        }

        private void beginRemoteRead(int offset, int length)
        {
            try
            {
                this.remoteConnection.GetStream().BeginRead(remoteBuffer.Buffer(), offset, length, remoteRead, null);
            }
            catch
            {
                Console.WriteLine("Client disconnected from server");
            }
        }

        private void remoteRead(IAsyncResult asyn)
        {
            try
            {
                NetworkStream stream = remoteConnection.GetStream();
                //Console.WriteLine("Packet received from server");
                remoteBuffer.Advance(stream.EndRead(asyn));
                if (remoteBuffer.Length() == 0)
                {
                    Close(new EndOfStreamException());
                    return;
                }
                bool flag = this.remoteBuffer.Index() == 4;
                if (flag)
                {
                    this.remoteBuffer.Resize(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.remoteBuffer.Buffer(), 0)));
                    this.beginRemoteRead(this.remoteBuffer.Index(), this.remoteBuffer.BytesRemaining());
                }
                else
                {
                    bool flag2 = this.remoteBuffer.BytesRemaining() > 0;
                    if (flag2)
                    {
                        this.beginRemoteRead(this.remoteBuffer.Index(), this.remoteBuffer.BytesRemaining());
                    }
                    else
                    {
                        ServerReceiveKey.Cipher(remoteBuffer.Buffer());
                        //Console.WriteLine(remoteBuffer.ToString());
                        sendToClient(this.remoteBuffer.Buffer());
                        remoteBuffer.Flush();
                        this.beginRemoteRead(0, 4);
                    }
                }
            }
            catch (Exception ex)
            {
                Close(ex);
            }
        }

        private void sendPolicyString()
        {
            NetworkStream stream = localConnection.GetStream();
            Console.WriteLine("Sending policy string...");
            string policy = "<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0";
            byte[] array = Encoding.UTF8.GetBytes(policy);
            stream.Write(array, 0, array.Length);
        }

        private void localRead(IAsyncResult asyn)
        {
            try
            {


                NetworkStream stream = localConnection.GetStream();
                //Console.WriteLine("Packet received from client");
                localBuffer.Advance(stream.EndRead(asyn));
                if (localBuffer.Length() == 0)
                {
                    Close(new EndOfStreamException());
                    return;
                }
                
                if (localBuffer.Buffer()[0] == 0x03C)
                {
                    sendPolicyString();
                    localBuffer.Flush();
                    Close(new EndOfStreamException());
                }
                else
                {                    
                    bool flag = this.localBuffer.Length() == 4;
                    if (flag)
                    {
                        this.localBuffer.Resize(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.localBuffer.Buffer(), 0)));
                        this.beginLocalRead(this.localBuffer.Index(), this.localBuffer.BytesRemaining());
                    }
                    else
                    {
                        bool flag2 = this.localBuffer.BytesRemaining() > 0;
                        if (flag2)
                        {
                            this.beginLocalRead(this.localBuffer.Index(), this.localBuffer.BytesRemaining());
                        }
                        else
                        {
                            //Console.WriteLine("Before: {0}", localBuffer.ToString());
                            ClientReceiveKey.Cipher(localBuffer.Buffer());

                            Packet packet = Packet.Create(localBuffer.Buffer());
                            if (packet.Type != PacketType.UNKNOWN)
                            {
                                Console.WriteLine("Received from client: {0}, id={1}", packet.Type, packet.id);
                            }
                            if (packet.send)
                            {
                                //Console.WriteLine("Debug, send packetid={0}", packet.id);
                                sendToServer(packet);
                            }
                            
                            localBuffer.Flush();
                            this.beginLocalRead(0, 4);
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Close(ex);
            }
        }

        public void Close(Exception reason)
        {
            bool flag = this.remoteConnection.Connected || this.localConnection.Connected;
            if (flag)
            {
                Console.WriteLine("[Client Handler] {1} disconnected. {0}", reason.ToString(), "Client " + clientId);
            }
            bool connected = this.remoteConnection.Connected;
            if (connected)
            {
                this.remoteConnection.Close();
            }
            bool connected2 = this.localConnection.Connected;
            if (connected2)
            {
                this.localConnection.Close();
            }
        }

        private void sendToClient(byte[] packet)
        {
            try
            {
                ClientSendKey.Cipher(packet);
                localConnection.GetStream().Write(packet, 0, packet.Length);
            }
            catch (Exception ex)
            {
                Close(ex);
            }
        }

        private void sendToServer(Packet packet)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                using (PacketWriter packetWriter = new PacketWriter(memoryStream))
                {
                    packetWriter.Write(0);
                    packetWriter.Write(packet.id);
                    packet.Write(packetWriter);
                }
                byte[] array = memoryStream.ToArray();
                PacketWriter.BlockCopyInt32(array, array.Length);
                ServerSendKey.Cipher(array);
                NetworkStream stream = this.remoteConnection.GetStream();
                //Console.WriteLine("After: {0}, length={1}", byteArrayToString(array), array.Length);
                stream.Write(array, 0, array.Length);
            }
            catch (Exception ex)
            {
                Close(ex);
            }
        }

        private string byteArrayToString(byte[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += String.Format("{0:X2} ", array[i]);
            }
            return result;
        }

        private void clearStream(NetworkStream ns) 
        {
            byte[] l = new byte[1];
            while (ns.DataAvailable)
            {
                int read = ns.Read(l, 0, 1);
                if (read == 0)
                {
                    Close(new EndOfStreamException());
                }
            }
        }
    }
}
