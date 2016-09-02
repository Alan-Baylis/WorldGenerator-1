using System;
using System.Text;

namespace Sean.Shared.Comms
{
    public static class MessageParser
    {
        public static int clientId { get; set; }
        private static int destId = 1;


        public static string SerializeMessage(Message message)
        {
            return Utilities.JsonSerialize<Message>(message);
        }
        public static Message DeserializeMessage(string json)
        {
            return Utilities.JsonDeserialize<Message>(json);
        }

        public static CommsMessages.Message ParsePacket (byte[] packet, out byte[] data)
        {
            int messageLength = packet [0] * 256 + packet [1];
            byte[] msgBuffer = new byte[messageLength];
            Array.Copy (packet, 2, msgBuffer, 0, messageLength);
            var recv = CommsMessages.Message.ParseFrom(msgBuffer);

            var msgType = (CommsMessages.MsgType)recv.Msgtype;
            Console.WriteLine ("Msg Type: {0}", msgType);

            int dataLength = packet [2 + messageLength] * 256 + packet [2 + messageLength + 1];
            data = new byte[dataLength];
            Array.Copy (packet, 2 + messageLength + 2, data, 0, dataLength);

            return recv;
        }


        public static CommsMessages.Message CreateLoginMessage(string ipAddress, int port, string username, string password)
        {
            var commsLoginMessageBuilder = new CommsMessages.Login.Builder ();
            commsLoginMessageBuilder.SetIpaddress (ipAddress);
            commsLoginMessageBuilder.SetPort (port);
            commsLoginMessageBuilder.SetUsername (username);
            commsLoginMessageBuilder.SetPassword (password);
            var commsLoginMessage = commsLoginMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetLogin (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eLogin);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }

        public static CommsMessages.Message CreateResponseMessage(int code, string message)
        {            
            var commsResponseMessageBuilder = new CommsMessages.Response.Builder ();
            commsResponseMessageBuilder.SetCode(code);
            commsResponseMessageBuilder.SetMessage(message);
            var commsLoginMessage = commsResponseMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetResponse (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eResponse);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }

        public static CommsMessages.Message CreatePingMessage()
        {
            var commsPingMessageBuilder = new CommsMessages.Ping.Builder ();
            var commsLoginMessage = commsPingMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetPing (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.ePing);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreatePongMessage()
        {
            var commsPongMessageBuilder = new CommsMessages.Pong.Builder ();
            var commsLoginMessage = commsPongMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetPong (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.ePong);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateSayMessage(string text)
        {
            var commsSayMessageBuilder = new CommsMessages.Say.Builder ();
            commsSayMessageBuilder.SetText(text);
            var commsLoginMessage = commsSayMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetSay (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eSay);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateMapRequestMessage(int x, int y)
        {
            var commsMapRequestMessageBuilder = new CommsMessages.MapRequest.Builder ();
            commsMapRequestMessageBuilder.SetX(x);
            commsMapRequestMessageBuilder.SetY(y);
            var commsLoginMessage = commsMapRequestMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetMapRequest (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eMapRequest);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateMapIgnoreMessage(int x, int y)
        {
            var commsMapIgnoreMessageBuilder = new CommsMessages.MapIgnore.Builder ();
            commsMapIgnoreMessageBuilder.SetX (x);
            commsMapIgnoreMessageBuilder.SetY (y);
            var commsLoginMessage = commsMapIgnoreMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetMapIgnore (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eMapIgnore);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateMapMessage(int minX, int minY, int maxX, int maxY)
        {
            var commsMapMessageBuilder = new CommsMessages.Map.Builder ();
            commsMapMessageBuilder.SetMinX (minX);
            commsMapMessageBuilder.SetMaxX (maxX);
            commsMapMessageBuilder.SetMinY (minY);
            commsMapMessageBuilder.SetMaxY (maxY);
            var commsLoginMessage = commsMapMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetMap (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eMap);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateMapUpdateMessage(int x, int y, int z, int newBlock)
        {
            var commsMapUpdateMessageBuilder = new CommsMessages.MapUpdate.Builder ();
            commsMapUpdateMessageBuilder.SetX (x);
            commsMapUpdateMessageBuilder.SetY (y);
            commsMapUpdateMessageBuilder.SetZ (z);
            commsMapUpdateMessageBuilder.SetNewBlock(newBlock);
            var commsLoginMessage = commsMapUpdateMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetMapUpdate (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eMapUpdate);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateMapCharacterUpdateMessage(int id, int x, int y, int z)
        {
            var commsMapCharacterUpdateMessageBuilder = new CommsMessages.MapCharacterUpdate.Builder ();
            commsMapCharacterUpdateMessageBuilder.SetId(id);
            commsMapCharacterUpdateMessageBuilder.SetX (x);
            commsMapCharacterUpdateMessageBuilder.SetY (y);
            commsMapCharacterUpdateMessageBuilder.SetZ (z);
            var commsLoginMessage = commsMapCharacterUpdateMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetMapCharacterUpdate (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eMapCharacterUpdate);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateQueryServerMessage(string parameter)
        {
            var commsQueryServerMessageBuilder = new CommsMessages.QueryServer.Builder ();
            commsQueryServerMessageBuilder.SetParameter (parameter);
            var commsLoginMessage = commsQueryServerMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetQueryServer (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eQueryServer);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }
        public static CommsMessages.Message CreateQueryServerResponseMessage(string parameter, string value)
        {
            var commsQueryServerResponseMessageBuilder = new CommsMessages.QueryServerResponse.Builder ();
            commsQueryServerResponseMessageBuilder.SetParameter (parameter);
            commsQueryServerResponseMessageBuilder.SetValue (value);
            var commsLoginMessage = commsQueryServerResponseMessageBuilder.Build ();
            var commsMessageBuilder = new CommsMessages.Message.Builder ();
            commsMessageBuilder.SetQueryServerResponse (commsLoginMessage);
            commsMessageBuilder.SetMsgtype ((int)CommsMessages.MsgType.eQueryServerResponse);
            commsMessageBuilder.SetFrom (clientId);
            commsMessageBuilder.SetDest (destId);
            var commsMessage = commsMessageBuilder.Build ();
            return commsMessage;
        }

        public static byte[] CreatePacket(CommsMessages.Message message, byte[] data)
        {
            using (var memoryStream = new System.IO.MemoryStream()) {
                memoryStream.WriteByte (0); // reserve for length
                memoryStream.WriteByte (0); // reserve for length
                message.WriteTo (memoryStream);
                var messageLength = memoryStream.Position - 2;

                memoryStream.WriteByte (0); // reserve for length
                memoryStream.WriteByte (0); // reserve for length
                using (var dataStream = new System.IO.MemoryStream (data)) {
                    dataStream.WriteTo (memoryStream);
                }
                var dataLength = memoryStream.Position - messageLength - 4;

                var packetBytes = memoryStream.ToArray ();
                packetBytes [0] = (byte)((messageLength)/256);
                packetBytes [1] = (byte)((messageLength)%256);
                packetBytes [messageLength+2] = (byte)((dataLength)/256);
                packetBytes [messageLength+3] = (byte)((dataLength)%256);

                return packetBytes;
            }
        }
    }
}

