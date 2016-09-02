using System;
using System.Text;

namespace Sean.Shared.Comms
{
    public static class MessageParser
    {
        public static int clientId { get; set; }
        private static int destId = 1;

        public static Message ParsePacket (byte[] packet, out byte[] data)
        {
            int messageLength = packet [0] * 256 + packet [1];
            byte[] msgBuffer = new byte[messageLength];
            Array.Copy (packet, 2, msgBuffer, 0, messageLength);
            var jsonMessage = Encoding.ASCII.GetString(msgBuffer);
            var recv = Utilities.JsonDeserialize<Message>(jsonMessage);

            int dataLength = packet [2 + messageLength] * 256 + packet [2 + messageLength + 1];
            data = new byte[dataLength];
            Array.Copy (packet, 2 + messageLength + 2, data, 0, dataLength);

            return recv;
        }

        public static Message CreateLoginMessage(string ipAddress, int port, string username, string password)
        {
            var message = new Message () {
                DestId = destId,
                FromId = clientId,
                Login = new LoginMessage () {
                    IpAddress = ipAddress,
                    Port = port,
                    Username = username,
                    Password = password,
                }
            };
            return message;
        }

        public static byte[] CreatePacket(Message message, byte[] data)
        {
            var messageJson = Utilities.JsonSerialize<Message>(message);
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);
            using (var memoryStream = new System.IO.MemoryStream()) {
                memoryStream.WriteByte (0); // reserve for length
                memoryStream.WriteByte (0); // reserve for length
                memoryStream.Write(messageBytes,0,messageBytes.Length);
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

