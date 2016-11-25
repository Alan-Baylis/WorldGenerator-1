using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Sean.Shared.Comms;
using System.Net;
using Sean.Shared;

namespace Sean.WorldServer
{
	public class ServerSocketListener {
		public static int ServerListenPort = 8084;
        private const int MaxMessageBufferSize = 1024;
        private static Thread thread;

        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

	    public ServerSocketListener()
        {
	    }

	    public static void Run()
        {
            thread = new Thread (new ThreadStart (StartListening));
            thread.Start ();
        }
        public static void Stop()
        {
            thread.Abort();
        }
        private static void StartListening() {
            try {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEP = new IPEndPoint(ipAddress, 8084);

                TcpListener serverSocket = new TcpListener(localEP);
                serverSocket.Start();
                Log.WriteInfo($"TcpSocket waiting for a connection on port {ServerListenPort}...");
                while (true) {
                    var socket = serverSocket.AcceptTcpClient();
                    Log.WriteInfo("Client joined");
                    var client = ClientConnection.CreateClientConnection(socket, MessageProcessor.ServerProcessMessage);
                    client.StartClient();
                }
                serverSocket.Stop();
                Log.WriteInfo("Ending Listening Server");
            }
            catch (Exception e) {
                Log.WriteError($"Exception caught in ServerSocketListener - {e.ToString()}");
            }
	    }
	}
}