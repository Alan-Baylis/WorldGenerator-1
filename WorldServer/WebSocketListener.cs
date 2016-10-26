using System;
using System.Threading;
using System.Text;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Net;
using Sean.Shared.Comms;
using System.Collections.Generic;

namespace Sean.WorldServer
{
    // See http://sta.github.io/websocket-sharp/

    //public class Echo : WebSocketBehavior
    //{
    //    protected override void OnMessage(MessageEventArgs e)
    //    {
    //        Send(e.Data);
    //    }
    //}

    public class WebSocketListener
    {
        public static Dictionary<Guid, WorldWebSocketClientConnection> clientsList = new Dictionary<Guid, WorldWebSocketClientConnection>();
        private static Thread thread;

        public WebSocketListener()
        {
        }

        public static void Run()
        {
            thread = new Thread(new ThreadStart(StartListening));
            thread.Start();
        }
        public static void Stop()
        {
            thread.Abort();
        }
        public static void SendMessage(Guid clientId, Message message)
        {
            if (!clientsList.ContainsKey(clientId))
                return;
            clientsList[clientId].SendMessage(message);
        }
        public static void BroadcastMessage(Message message)
        {
            foreach (var clientId in clientsList.Keys)
            {
                clientsList[clientId].SendMessage(message);
            }
        }

        private static void StartListening()
        {
            try
            {
                var ServerListeningPort = 8083;
                var wssv = new WebSocketServer($"ws://localhost:{ServerListeningPort}");
                Console.WriteLine($"Websocket waiting for a connection on port {ServerListeningPort}...");
                //wssv.AddWebSocketService<Echo>("/Echo");
                wssv.AddWebSocketService<WorldWebSocketClientConnection>("/WebSocket");
                wssv.Start();

                while (true)
                { }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught in WebSocketListener - {0}", e.ToString());
            }
        }
    }


    public class WorldWebSocketClientConnection : WebSocketBehavior
    {
        public WorldWebSocketClientConnection()
        {
            IgnoreExtensions = true;
            clientId = Guid.NewGuid ();
            WebSocketListener.clientsList[clientId] = this;
            Console.WriteLine ($"Connected {ID}={clientId}");
        }

        private Sean.Shared.Comms.ClientConnection.ProcessMessage processMessageFn = MessageProcessor.ServerProcessMessage;
        private const int MaxMessageLength = 1024;
        private const int MaxDataMessageLength = 1048576;
        private Guid clientId;
        private Guid serverId = new Guid();

        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                Console.WriteLine ($"received from {clientId}");
                var data = e.RawData;

                // Message
                byte[] lenBuffer = new byte[2];
                Array.Copy (data, 0, lenBuffer, 0, 2);
                int messageLength = data[0] * 256 + data[1];
                if (messageLength > MaxMessageLength) throw new ApplicationException ($"Message length {messageLength} too large"); 
                if (messageLength == 0) throw new ApplicationException ($"Message length 0");

                byte[] msgBuffer = new byte[messageLength];
                Array.Copy (data, 2, msgBuffer, 0, messageLength);
                var jsonMessage = Encoding.ASCII.GetString(msgBuffer);
                var msg = Utilities.JsonDeserialize<Message>(jsonMessage);

                // Data
                byte[] dataLenBuffer = new byte[4];
                Array.Copy (data, 2 + messageLength, dataLenBuffer, 0, 4);
                int dataLength = BitConverter.ToInt32(dataLenBuffer, 0);
                if (dataLength > MaxDataMessageLength) throw new ApplicationException ($"Message data length {dataLength} too large");
                Console.WriteLine($"[ClientConnection.DoSocketReader] DataLength:{dataLength}");

                if (dataLength > 0)
                {
                    msg.Data = new byte[dataLength];
                    Array.Copy (data, 2 + messageLength + 4, msg.Data, 0, dataLength);
                }

                // Process Message
                Console.WriteLine($"Received: {msg.ToString()}");                    
                processMessageFn(clientId, msg);

                //Sessions.Broadcast(e.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OnMessage] Exception: {ex.Message}");
            }
        }

        public void SendMessage(Message message)
        {
            message.DestId = clientId;
            message.FromId = serverId;
            Console.WriteLine($"Sending: {message.ToString()}");

            using (var memoryStream = new System.IO.MemoryStream())
            {
                var messageJson = Utilities.JsonSerialize<Message>(message);
                var msgBuffer = Encoding.ASCII.GetBytes(messageJson);

                memoryStream.WriteByte((byte)(msgBuffer.Length / 256));
                memoryStream.WriteByte((byte)(msgBuffer.Length % 256));
                Console.WriteLine($"[SendMessage] Writing message length:{msgBuffer.Length}");
                memoryStream.Write(msgBuffer, 0, msgBuffer.Length);

                var dataLength = message.Data == null ? 0 : message.Data.Length;
                byte[] intBytes = BitConverter.GetBytes(dataLength);
                memoryStream.WriteByte(intBytes[3]);
                memoryStream.WriteByte(intBytes[2]);
                memoryStream.WriteByte(intBytes[1]);
                memoryStream.WriteByte(intBytes[0]);

                Console.WriteLine($"[SendMessage] Writing data length:{dataLength}");
                if (message.Data != null)
                    memoryStream.Write(message.Data, 0, message.Data.Length);

                var temp = memoryStream.ToArray();
                Send(temp);
            }
        }

        protected override void OnError (ErrorEventArgs e)
        {
            Console.WriteLine ($"OnError {ID}: {e.Message}");
        }
    }

}

