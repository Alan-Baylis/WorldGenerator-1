using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Sean.Shared.Comms;
using System.Net;

namespace Sean.WorldServer
{
	public class ServerSocketListener {
		public static int ServerListenPort = 8084;
        private const int MaxMessageBufferSize = 1024;

	    // Thread signal.
	    public static ManualResetEvent allDone = new ManualResetEvent(false);

	    public ServerSocketListener() {
	    }

	    public static void Run() {
            Thread thread = new Thread (new ThreadStart (StartListening));
            thread.Start ();
        }
        private static void StartListening() {
            try {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEP = new IPEndPoint(ipAddress, 8084);

                TcpListener serverSocket = new TcpListener(localEP);
                serverSocket.Start();
                Console.WriteLine($"Waiting for binary connection on port {ServerListenPort}...");
                while (true) {
                    var socket = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Client joined");
                    var client = ClientConnection.CreateClientConnection(socket, MessageProcessor.ServerProcessMessage);
                    client.StartClient();
                }
                serverSocket.Stop();
                Console.WriteLine("Ending Listening Server");
            }
            catch (Exception e) {
                Console.WriteLine("Exception caught in ServerSocketListener - {0}", e.ToString());
            }
	    }
	}
}