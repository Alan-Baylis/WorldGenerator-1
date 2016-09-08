using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sean.Shared.Comms
{
    // State object for reading client data asynchronously
    public class ClientConnection {

        public delegate void ProcessMessage(Guid clientId, Message msg);
        private ProcessMessage processMessageFn;
        private static Guid serverId = Guid.NewGuid();

        public static ClientConnection CreateClientConnection(TcpClient inClientSocket, ProcessMessage processMessageFn)
        {
            var clientId = Guid.NewGuid();
            var client = new ClientConnection(clientId, inClientSocket, processMessageFn);
            clientsList[clientId] = client;
            return client;
        }

        public static void EnqueueMessage(Guid clientId, Message message)
        {
            if (!clientsList.ContainsKey(clientId))
                return;
            clientsList[clientId].PutQueuedMessage(message);
        }

        public static void BroadcastMessage(Message message)
        {
            foreach (var clientId in clientsList.Keys)
            {
                clientsList[clientId].PutQueuedMessage(message);
            }
        }

        private ClientConnection(Guid clientId, TcpClient inClientSocket, ProcessMessage processMessageFn)
        {
            this.clientId = clientId;
            this.socket = inClientSocket;
            this.processMessageFn = processMessageFn;
        }

        public static Dictionary<Guid, ClientConnection> clientsList = new Dictionary<Guid, ClientConnection> ();

        private Guid clientId;
        public TcpClient socket = null;
        private const int MaxMessageLength = 1024;

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
                if (!networkStream.CanRead)
                    throw new ApplicationException("Cannot read from socket");
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

                        if (dataLength > 0)
                        {
                            msg.Data = new byte[dataLength];
                            totalBytesRead = 0;
                            do
                            {
                                bytesRead = networkStream.Read(msg.Data, totalBytesRead, dataLength);
                                totalBytesRead += bytesRead;
                            }
                            while (totalBytesRead < messageLength);
                        }

                        // Process Message
                        Console.WriteLine($"Received: {msg.ToString()}");                    
                        processMessageFn(clientId, msg);
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
                if (!networkStream.CanWrite)
                    throw new ApplicationException("Cannot write to socket");
                while (true) 
                {
                    Message message = GetQueuedMessage();
                    message.DestId = clientId;
                    message.FromId = serverId;
                    if (message == null)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.WriteLine($"Sending: {message.ToString()}");                    
                        var messageJson = Utilities.JsonSerialize<Message>(message);
                        var msgBuffer = Encoding.ASCII.GetBytes(messageJson);

                        networkStream.WriteByte((byte)(msgBuffer.Length/256));
                        networkStream.WriteByte((byte)(msgBuffer.Length%256));
                        networkStream.Write(msgBuffer,0,msgBuffer.Length);

                        var dataLength = message.Data == null ? 0 : message.Data.Length;
                        networkStream.WriteByte((byte)(dataLength / 256));
                        networkStream.WriteByte((byte)(dataLength % 256));
                        if (message.Data != null)
                            networkStream.Write(message.Data,0,message.Data.Length);

                        networkStream.Flush();
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
    }
}

