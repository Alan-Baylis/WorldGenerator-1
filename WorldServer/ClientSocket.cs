using System;
using System.Net;
using System.Net.Sockets;
using Sean.Shared.Comms;
using Sean.Shared;

namespace Sean.WorldServer
{
    public static class ClientSocket
    {
        private static SynchronousSocketClient socket;
        private const string server = "elixirserver";
        private const int port = 8083;

        private static SynchronousSocketClient GetSocket()
        {
            if (socket == null) {
                socket = new SynchronousSocketClient (server, port);
            }
            return socket;
        }

        public static void SendMessage(Message message, byte[] data)
        {
            // TODO - threads
            GetSocket ().SendMessage (message, data);
        }

        public static void RecvMessage()
        {
            // TODO - threads
            GetSocket ().RecvMessage ();
        }
    }

    public class SynchronousSocketClient : IDisposable
    {
        public SynchronousSocketClient(string server, int serverPort)
        {
            remoteHost = Dns.GetHostEntry(server);
            ipAddress = remoteHost.AddressList[0];
            port = serverPort;
        }

        // Dispose pattern
        bool disposed = false;
        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 
            if (disposing) {
                // Free any other managed objects here.
                if (socket != null) {
                    socket.Shutdown (SocketShutdown.Both);
                    socket.Close ();
                    socket = null;
                }
            }
            // Free any unmanaged objects here.
            disposed = true;
        }

        private Socket socket;
        private int port;
        private static IPHostEntry remoteHost;
        private IPAddress ipAddress;

        private bool IsConnected
        {
            get
            {
                return socket != null && socket.IsBound;
            }
        }

        private void Connect() 
        {
            Log.WriteInfo("SynchronousSocketClient.StartClient");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            socket = new Socket(
                AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp );
            socket.Connect(remoteEP);
            Log.WriteInfo($"Socket connected to {socket.RemoteEndPoint.ToString()}");
        }

        public void SendMessage(Message message, byte[] data)
        {
            try {
                if (!IsConnected) Connect();
                // Encode the data string into a byte array.
                /*
                                var messageBytes = MessageParser.WriteMessage (message);

                                //byte[] msg = Encoding.ASCII.GetBytes ("This is a test<EOF>");
                                var msg = new byte[messageBytes.Length + data.Length];
                                //msg[0] = (byte)((messageBytes.Length + data.Length)/256);
                                //msg[1] = (byte)((messageBytes.Length + data.Length)%256);
                                messageBytes.CopyTo(msg, 0);
                                data.CopyTo(msg, messageBytes.Length);

                                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                                for (int z = 0; z < 20; z++)
                                {
                                    builder.Append(msg[z]);
                                    builder.Append(",");
                                }
                                Log.WriteInfo(builder.ToString());

                                int bytesSent = socket.Send (msg);
                                Log.WriteInfo("SynchronousSocketClient.SendMessage sent {0} bytes", bytesSent);
                */
            }
            catch (ArgumentNullException ane) {
                Log.WriteError($"ArgumentNullException : {ane.Message}");
            } catch (SocketException se) {
                Log.WriteError($"SocketException : {se.Message}");
            } catch (Exception ex) {
                Log.WriteError($"Unexpected exception : {ex.Message}");
            }
        }

        public void RecvMessage()
        {
            try {
                if (!IsConnected) Connect();
                // Receive the response from the remote device.
                byte[] bytes = new byte[1024];
                int bytesRec = socket.Receive (bytes);
                Log.WriteError($"SynchronousSocketClient.RecvMessage received {bytesRec} bytes");
             } catch (SocketException se) {
                Log.WriteError($"SocketException : {se.Message}");
            } catch (Exception ex) {
                Log.WriteError($"Unexpected exception : {ex.Message}");
            }
        }
    }
}
