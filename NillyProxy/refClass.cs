using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace NillyProxy
{
    class refClass
    {
        public void test(string[] args)
        {
            TcpListener serverSocket = new TcpListener(2050);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine(" >> Server Started");

            while (true)
            {
                Console.WriteLine("Listening...");
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> Accept connection from client");
                requestCount = 0;

                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];
                    int readSize = networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = "";
                    byte[] actual = new byte[readSize];
                    for (int i = 0; i < readSize; i++)
                    {
                        actual[i] = bytesFrom[i];
                        dataFromClient += String.Format("{0:X2} ", bytesFrom[i]);
                    }
                    String request = Encoding.ASCII.GetString(actual);
                    Console.WriteLine("Received Request: " + request);
                    if (request == "")
                    {
                        Console.WriteLine("Empty Request Closing connection");
                        continue;
                    }
                    char[] separator = { ' ', '\n' };

                    String[] reqData = request.Split(separator);
                    char[] separator2 = { ':' };

                    //Console.WriteLine(reqData[1]);
                    HttpWebRequest newReq = (HttpWebRequest)WebRequest.Create(reqData[1]);

                    newReq.Credentials = CredentialCache.DefaultCredentials;
                    //newReq.Method = reqData[0];
                    newReq.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.1500.95 Safari/537.36";
                    //Console.WriteLine(reqData[2]);
                    newReq.ContentType = reqData[2];
                    WebResponse response = newReq.GetResponse();
                    Console.WriteLine("Reponse received from host");
                    Stream dataStream = response.GetResponseStream();
                    //StreamReader reader = new StreamReader(dataStream);
                    //string responseFromServer = reader.ReadToEnd();
                    //Byte[] sendBytes = Encoding.UTF8.GetBytes(responseFromServer);
                    dataStream.CopyTo(networkStream);
                    //networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine("Response sent back to client");
                    Console.WriteLine("Closing Connection...");
                    //Console.ReadLine();
                    //Console.WriteLine(" >> " + responseFromServer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }


                clientSocket.Close();
                //serverSocket.Stop();
                Console.WriteLine(" >> connection closed");
                //Console.ReadLine();
            }
        }

    }
}

/*
bool flag = this.localBuffer.Length() == 4;
			if (flag)
            {
                for (int i = 0; i < localBuffer.Length(); i++)
                {
                    Console.Write(String.Format("{0:X2} ", localBuffer.Buffer()[i]));
                }
                if (localBuffer.Buffer()[0] == 0x03C)
                {
                    sendPolicyString();
                    localBuffer.Flush();
                    
                    this.beginLocalRead(0, 8192);
                }
                else
                {
                    Console.WriteLine("Resizing: " + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.localBuffer.Buffer(), 0)));
                    this.localBuffer.Resize(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(this.localBuffer.Buffer(), 0)));
                    this.beginLocalRead(this.localBuffer.Index(), this.localBuffer.BytesRemaining());
                }
                
            }
            else
            {
                bool flag2 = this.localBuffer.BytesRemaining() > 0;
                if (!flag2)
                {
                    this.beginLocalRead(this.localBuffer.Index(), this.localBuffer.BytesRemaining());
                }
                else
                {
                    for (int i = 0; i < localBuffer.Length(); i++)
                    {
                        Console.Write(String.Format("{0:X2} ", localBuffer.Buffer()[i]));
                    }
                    localBuffer.Flush();
                    this.beginLocalRead(0, 8192);
                }
            }
            
            //localBuffer.Flush();
*/