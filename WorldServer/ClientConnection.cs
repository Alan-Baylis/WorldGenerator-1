using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sean.Shared.Comms;
using Sean.WorldGenerator;

namespace Sean.WorldServer
{
    // State object for reading client data asynchronously
    public class ClientConnection {
        public ClientConnection(TcpClient inClientSocket)
        {
            this.socket = inClientSocket;
        }

        public static List<ClientConnection> clientsList = new List<ClientConnection> ();

        public TcpClient socket = null;
        private const int MaxMessageLength = 1024;
        private int clientId;

        public void StartClient()
        {
            Thread readerThread = new Thread(DoSocketReader);
            Thread writerThread = new Thread(DoSocketWriter);
            readerThread.Start();
            writerThread.Start ();
        }

        private void DoSocketReader()
        {
            Console.WriteLine ($"[ClientConnection.DoSocketReader] Listening on socket {socket}");
            try {
                NetworkStream networkStream = socket.GetStream ();
                while (true) 
                {
                    if (!networkStream.DataAvailable)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        // Message
                        byte[] lenBuffer = new byte[2];
                        int bytesRead = networkStream.Read(lenBuffer, 0, 2);
                        if (bytesRead != 2) throw new ApplicationException("Could not read length bytes");
                        int messageLength = lenBuffer[0] * 256 + lenBuffer[1];
                        if (messageLength > MaxMessageLength) throw new ApplicationException ($"Message length {messageLength} too large"); 

                        byte[] msgBuffer = new byte[messageLength];
                        int totalBytesRead = 0;                    
                        do
                        {
                            bytesRead = networkStream.Read(msgBuffer, totalBytesRead, messageLength);
                            totalBytesRead += bytesRead;
                        }
                        while(totalBytesRead < messageLength);

                        var jsonMessage = Encoding.ASCII.GetString(msgBuffer);
                        var msg = Utilities.JsonDeserialize<Message>(jsonMessage);

                        // Data
                        bytesRead = networkStream.Read(lenBuffer, 0, 2);
                        if (bytesRead != 2) throw new ApplicationException("Could not read data length bytes");
                        int dataLength = lenBuffer[0] * 256 + lenBuffer[1];
                        if (dataLength > MaxMessageLength) throw new ApplicationException ($"Message data length {dataLength} too large"); 

                        msg.Data = new byte[messageLength];
                        totalBytesRead = 0;                    
                        do
                        {
                            bytesRead = networkStream.Read(msg.Data, totalBytesRead, messageLength);
                            totalBytesRead += bytesRead;
                        }
                        while(totalBytesRead < messageLength);
                        
                        // Process Message
                        ProcessMessage(msg);
                    }
                }
            } 
            catch (Exception ex) {
                Console.WriteLine ($"[ClientConnection.DoSocketReader] Exception - {ex.Message}");
                if (socket != null) 
                    socket.Close ();
            }
        }

        private Queue<Message> writeQueue = new Queue<Message>();
        private void DoSocketWriter()
        {
            Console.WriteLine ($"[ClientConnection.DoSocketWriter] Writing on socket {socket}");
            try {
                NetworkStream networkStream = socket.GetStream ();
                while (true) 
                {
                    Message message = GetQueuedMessage();
                    if (message == null)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        var messageJson = Utilities.JsonSerialize<Message>(message);
                        var msgBuffer = Encoding.ASCII.GetBytes(messageJson);

                        networkStream.Write((byte)(msgBuffer.Length/256));
                        networkStream.Write((byte)(msgBuffer.Length%256));
                        networkStream.Write(msgBuffer,0,msgBuffer.Length);

                        networkStream.Write((byte)(message.Data.Length/256));
                        networkStream.Write((byte)(message.Data.Length%256));
                        networkStream.Write(message.Data,0,message.Data.Length);
                    }
                }
            } 
            catch (Exception ex) {
                Console.WriteLine ($"[ClientConnection.DoSocketWriter] Exception - {ex.Message}");
                if (socket != null) 
                    socket.Close ();
            }
        }

        private void PutQueuedMessage(Message message)
        {
            lock(writeQueue)
            {
                writeQueue.Enqueue(message);
            }
        }
        private Message GetQueuedMessage()
        {
            lock(writeQueue)
            {
                if (writeQueue.Count == 0) return null;
                return writeQueue.Dequeue();
            }
        }

        /*
        private void Send(Message message, byte[] data = null) 
        {
            var respPacket = MessageParser.SerializeMessage(message, data);
            Send (respPacket);
        }
        private void Send(byte[] byteData) {
            Console.WriteLine ("Send");
            var builder = new StringBuilder ();
            for (int i=0; i<byteData.Length; i++) {
                builder.Append (byteData [i].ToString ());
                builder.Append (",");
            }
            Console.WriteLine ("{0}", builder.ToString());

            // Begin sending the data to the remote device.
            NetworkStream socketStream = socket.GetStream();
            socketStream.Write(byteData, 0, byteData.Length);
            socketStream.Flush ();
        }
        */
        /*
        public static void broadcast(byte[] byteData)
        {
            foreach (ClientConnection client in clientsList)
            {
                client.Send (byteData);
            }
        }
        */

        private void ProcessMessage(Message msg)
        {
            if (msg.Ping != null)
                ProcessPing (msg);
            if (msg.Login != null)
                ProcessLogin (msg);
            if (msg.Say != null)
                ProcessSay (msg);
            if (msg.MapRequest != null)
                ProcessMapRequest (msg);
            if (msg.MapIgnore != null)
                ProcessMapIgnore (msg);
            if (msg.MapUpdate != null)
                ProcessMapUpdate (msg);
            if (msg.QueryServer != null)
                ProcessQueryServer (msg);
            Console.WriteLine("[ClientConnection.ProcessMessage] Unexpected message");
            SendError ("Unexpected message");
        }

        private void ProcessPing(Message pingMsg)
        {
            var pongMsg = new Message () {
                DestId = clientId,
                Pong = new PongMessage (){ Message = pingMsg.Ping.Message }
            };
            PutQueuedMessage(pongMsg);
        }
        private void ProcessLogin(Message msg)
        {
            SendOk ();// TODO
        }
        private void  ProcessSay(Message msg)
        {
        }
        private void ProcessMapRequest(Message msg)
        {
            var chunkCoord = World.GetChunkCoords (msg.MapRequest.Position);
            WorldEvents.ChunkRegister (chunkCoord, this);
        }
        private void ProcessMapIgnore(Message msg)
        {
            var chunkCoord = World.GetChunkCoords (msg.MapRequest.Position);
            WorldEvents.ChunkIgnore (chunkCoord, this);
            SendOk ();
        }
        private void ProcessMapUpdate(Message msg)
        {
            SendOk (); // TODO
        }
        private void ProcessQueryServer(Message msg)
        {
            SendOk (); // TODO
        }

        private void SendOk ()
        {
            var msg = new Message ()
            {
                DestId = clientId,
                Response = new ResponseMessage() { Code = 0 }
            };
            PutQueuedMessage (msg);
        }
        public void SendMap(Chunk chunk)
        {
            var msg = new Message () {
                DestId = clientId,
                Map = new MapMessage () {
                    MinPosition = chunk.MinPosition,
                    MaxPosition = chunk.MaxPosition
                },
                Data = chunk.Serialize ()
            };
            PutQueuedMessage (msg);
        }

        public void SendError(string reason)
        {
            var msg = new Message () {
                DestId = clientId,
                Response = new ResponseMessage () {
                    Code = 1,
                    Message = reason
                }
            };
            PutQueuedMessage (msg);
        }
            
    }
}

