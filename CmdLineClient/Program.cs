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

                TcpClient client = new TcpClient();
                Console.WriteLine ("Connecting...");
                client.Connect(remoteEP);

                var connection = ClientConnection.CreateClientConnection(client, ProcessMessage);
                connection.StartClient();

                ClientConnection.BroadcastMessage(new Message()
                {
                    Ping = new PingMessage()
                    {
                        Message = "Hi"
                    }
                });

                ClientConnection.BroadcastMessage(new Message()
                {
                    MapRequest = new MapRequestMessage()
                    {
                        Coords = new Sean.Shared.ChunkCoords(1, 1)
                    }
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in ServerSocketListener - {0}", e.ToString());
            }
        }

        public static void ProcessMessage(Guid clientId, Message msg)
        {
            Console.WriteLine($"Processing response...");
        }

    }

}
