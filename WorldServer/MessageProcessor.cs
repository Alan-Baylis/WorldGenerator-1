using System;
using Sean.Shared.Comms;
using Sean.WorldGenerator;
using System.Collections.Generic;

namespace Sean.WorldServer
{
    public static class MessageProcessor
    {
        public static void ServerProcessMessage(Guid clientId, Message msg)
        {
            if (msg.Ping != null)
                ProcessPing(clientId, msg);
            if (msg.Login != null)
                ProcessLogin(clientId, msg);
            if (msg.Say != null)
                ProcessSay(clientId, msg);
            if (msg.MapRequest != null)
                ProcessMapRequest(clientId, msg);
            if (msg.MapIgnore != null)
                ProcessMapIgnore(clientId, msg);
            if (msg.MapUpdate != null)
                ProcessMapUpdate(clientId, msg);
            if (msg.QueryServer != null)
                ProcessQueryServer(clientId, msg);
            Console.WriteLine("[ClientConnection.ProcessMessage] Unexpected message");
            SendError(clientId, "Unexpected message");
        }

        private static void ProcessPing(Guid clientId, Message pingMsg)
        {
            var pongMsg = new Message()
            {
                DestId = clientId,
                Pong = new PongMessage() { Message = pingMsg.Ping.Message }
            };
            ClientConnection.EnqueueMessage(clientId, pongMsg);
        }
        private static void ProcessLogin(Guid clientId, Message msg)
        {
            SendOk(clientId);// TODO
        }
        private static void ProcessSay(Guid clientId, Message msg)
        {
        }
        private static void ProcessMapRequest(Guid clientId, Message msg)
        {
            var chunkCoord = World.GetChunkCoords(msg.MapRequest.Position);
            WorldEvents.ChunkRegister(chunkCoord, clientId);
        }
        private static void ProcessMapIgnore(Guid clientId, Message msg)
        {
            var chunkCoord = World.GetChunkCoords(msg.MapRequest.Position);
            WorldEvents.ChunkIgnore(chunkCoord, clientId);
            SendOk(clientId);
        }
        private static void ProcessMapUpdate(Guid clientId, Message msg)
        {
            SendOk(clientId); // TODO
        }
        private static void ProcessQueryServer(Guid clientId, Message msg)
        {
            SendOk(clientId); // TODO
        }

        private static void SendOk(Guid clientId)
        {
            var msg = new Message()
            {
                DestId = clientId,
                Response = new ResponseMessage() { Code = 0 }
            };
            ClientConnection.EnqueueMessage(clientId, msg);
        }
        public static void SendMap(Guid clientId, Chunk chunk)
        {
            var msg = new Message()
            {
                DestId = clientId,
                Map = new MapMessage()
                {
                    MinPosition = chunk.MinPosition,
                    MaxPosition = chunk.MaxPosition
                },
                Data = chunk.Serialize()
            };
            ClientConnection.EnqueueMessage(clientId, msg);
        }

        public static void SendError(Guid clientId, string reason)
        {
            var msg = new Message()
            {
                DestId = clientId,
                Response = new ResponseMessage()
                {
                    Code = 1,
                    Message = reason
                }
            };
            ClientConnection.EnqueueMessage(clientId, msg);
        }
    }
}

