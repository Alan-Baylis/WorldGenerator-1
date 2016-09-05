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
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();  
        private int clientId;

        public void StartClient()
        {
            Thread ctThread = new Thread(DoConversation);
            ctThread.Start();
        }

        private void DoConversation()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10000]; // TODO - fix this. ugly
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            try {
                while (true) {
                    NetworkStream networkStream = socket.GetStream ();
                    int bytesRead = networkStream.Read (bytesFrom, 0, BufferSize);
                    if (bytesRead > 0)
                    {
                        var msg = MessageParser.ParsePacket (bytesFrom);
                        Send (ProcessMessage(msg));
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine (ex.ToString ());
            }
        }

        private void Send(Message message, byte[] data = null) 
        {
            var respPacket = MessageParser.CreatePacket(message, data);
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

        /*
        public static void broadcast(byte[] byteData)
        {
            foreach (ClientConnection client in clientsList)
            {
                client.Send (byteData);
            }
        }
        */

        public void SendMap(Chunk chunk)
        {
            Send (new Message () {
                DestId = clientId,
                Map = new MapMessage () {
                    MinPosition = chunk.MinPosition,
                    MaxPosition = chunk.MaxPosition
                }
            }, chunk.Serialize());
        }

        private Message ProcessMessage(Message msg)
        {
            if (msg.Ping != null)
                return ProcessPing (msg);
            if (msg.Login != null)
                return ProcessLogin (msg);
            if (msg.Say != null)
                return ProcessSay (msg);
            if (msg.MapRequest != null)
                return ProcessMapRequest (msg);
            if (msg.MapIgnore != null)
                return ProcessMapIgnore (msg);
            if (msg.MapUpdate != null)
                return ProcessMapUpdate (msg);
            if (msg.QueryServer != null)
                return ProcessQueryServer (msg);
            return ProcessError (msg, "Unexpected message");
        }

        private Message ProcessError(Message msg, string reason)
        {
            return new Message () {
                DestId = clientId,
                Response = new ResponseMessage () {
                    Code = 1,
                    Message = reason
                }
            };
        }

        private Message ProcessPing(Message msg)
        {
            return new Message () {
                DestId = clientId,
                Pong = new PongMessage (){ Message = msg.Ping.Message }
            };
        }
        private Message ProcessLogin(Message msg)
        {
            return CreateOk ();// TODO
        }
        private Message  ProcessSay(Message msg)
        {
            return CreateOk (); // TODO
        }
        private Message ProcessMapRequest(Message msg)
        {
            var chunkCoord = World.GetChunkCoords (msg.MapRequest.Position);
            WorldEvents.ChunkRegister (chunkCoord, this);
            return CreateOk ();
        }
        private Message ProcessMapIgnore(Message msg)
        {
            var chunkCoord = World.GetChunkCoords (msg.MapRequest.Position);
            WorldEvents.ChunkIgnore (chunkCoord, this);
            return CreateOk ();
        }
        private Message ProcessMapUpdate(Message msg)
        {
            return CreateOk (); // TODO
        }
        private Message ProcessQueryServer(Message msg)
        {
            return CreateOk (); // TODO
        }

        private Message CreateOk ()
        {
            return new Message ()
            {
                DestId = clientId,
                Response = new ResponseMessage() { Code = 0 }
            };
        }
    }
}

