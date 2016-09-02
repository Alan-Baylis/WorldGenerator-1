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
        public PingMessage Ping { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PongMessage Pong { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ResponseMessage Response { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public LoginMessage Login { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SayMessage Say { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MapRequestMessage MapRequest { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MapIgnoreMessage MapIgnore { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MapMessage Map { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MapUpdateMessage MapUpdate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MapCharacterUpdateMessage MapCharacterUpdate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public QueryServerMessage QueryServer { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public QueryServerResponseMessage QueryServerResponse { get; set; }
    }

}
