using System;
using System.Text;

namespace Sean.Shared
{
    public static class MessageParser
    {
        public static int clientId { get; set; }
        private static int destId = 1;


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

