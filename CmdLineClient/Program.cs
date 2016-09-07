using Sean.Shared.Comms;
using System;
using System.Net;
using System.Net.Sockets;

namespace CmdLineClient
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Hello World!");

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8084);

                TcpClient client = new TcpClient(remoteEP);

                var connection = ClientConnection.CreateClientConnection(client, ProcessMessage);
                connection.StartClient();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in ServerSocketListener - {0}", e.ToString());
            }
        }

        public static void ProcessMessage(Guid clientId, Message msg)
        {
            Console.WriteLine($"Processing...");
        }

    }

}
