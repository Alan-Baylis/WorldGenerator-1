using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Sean.Shared.Comms;

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
                TcpListener serverSocket = new TcpListener(ServerListenPort);
                serverSocket.Start();
                Console.WriteLine($"Waiting for a connection on port {ServerListenPort}...");
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