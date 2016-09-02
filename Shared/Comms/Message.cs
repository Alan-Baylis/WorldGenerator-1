using System.Runtime.Serialization;

namespace Sean.Shared.Comms
{
    [DataContract]
    public class PingMessage
    {
        [DataMember]
        public string Message { get; set; }
    }
    [DataContract]
    public class PongMessage
    {
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class ResponseMessage
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class LoginMessage
    {
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

    [DataContract]
    public class SayMessage
    {
        [DataMember]
        public string Text { get; set; }
    }

    [DataContract]
    public class MapRequestMessage
    {
        [DataMember]
        public Position Position { get; set; }
    }

    [DataContract]
    public class MapIgnoreMessage
    {
        [DataMember]
        public Position Position { get; set; }
    }

    [DataContract]
    public class MapMessage
    {
        [DataMember]
        public Position MinPosition { get; set; }
        [DataMember]
        public Position MaxPosition { get; set; }
    }

    [DataContract]
    public class MapUpdateMessage
    {
        [DataMember]
        public Position Position { get; set; }
        [DataMember]
        public Block NewBlock { get; set; }
    }

    [DataContract]
    public class MapCharacterUpdateMessage
    {
        [DataMember]
        public Position Position { get; set; }
        [DataMember]
        public int CharacterId { get; set; }
    }

    [DataContract]
    public class QueryServerMessage
    {
        [DataMember]
        public string Parameter { get; set; }
    }

    [DataContract]
    public class QueryServerResponseMessage
    {
        [DataMember]
        public string Parameter { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
    
    [DataContract]
    public class Message
    {
        [DataMember]
        public int FromId { get; set; }
        [DataMember]
        public int DestId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        PingMessage Ping { get; set; }

        [DataMember(EmitDefaultValue = false)]
        PongMessage Pong { get; set; }

        [DataMember(EmitDefaultValue = false)]
        ResponseMessage Response { get; set; }

        [DataMember(EmitDefaultValue = false)]
        LoginMessage Login { get; set; }

        [DataMember(EmitDefaultValue = false)]
        SayMessage Say { get; set; }

        [DataMember(EmitDefaultValue = false)]
        MapRequestMessage MapRequest { get; set; }

        [DataMember(EmitDefaultValue = false)]
        MapIgnoreMessage MapIgnore { get; set; }

        [DataMember(EmitDefaultValue = false)]
        MapMessage Map { get; set; }

        [DataMember(EmitDefaultValue = false)]
        MapUpdateMessage MapUpdate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        MapCharacterUpdateMessage MapCharacterUpdate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        QueryServerMessage QueryServer { get; set; }

        [DataMember(EmitDefaultValue = false)]
        QueryServerResponseMessage QueryServerResponse { get; set; }
    }

}
