using System;
using System.Runtime.Serialization;
using System.Text;

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
        public Guid FromId { get; set; }
        [DataMember]
        public Guid DestId { get; set; }

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

        // Note: Serialize separately as binary not json
        public byte[] Data { get; set; }

        public override string ToString()
        {
            var text = new StringBuilder();
            text.Append($"From:{FromId} ");
            text.Append($"To:{DestId} ");
            if (Ping != null) text.Append($"Ping({Ping.Message}) ");
            if (Pong != null) text.Append($"Pong({Pong.Message}) ");
            if (Response != null) text.Append($"Response({Response.Code},{Response.Message}) ");
            if (Login != null) text.Append($"Login({Login.Username}) ");
            if (Say != null) text.Append($"Say({Say.Text}) ");
            if (MapRequest != null) text.Append($"MapRequest({MapRequest.Position}) ");
            if (MapIgnore != null) text.Append($"MapIgnore({MapIgnore.Position}) ");
            if (Map != null) text.Append($"Map({Map.MinPosition}) ");
            if (MapUpdate != null) text.Append($"MapUpdate({MapUpdate.Position}={MapUpdate.NewBlock}) ");
            if (MapCharacterUpdate != null) text.Append($"MapCharacterUpdate({MapCharacterUpdate.CharacterId}->{MapCharacterUpdate.Position}) ");
            if (QueryServer != null) text.Append($"QueryServer({QueryServer.Parameter}) ");
            if (QueryServerResponse != null) text.Append($"QueryServerResponse({QueryServerResponse.Parameter}={QueryServerResponse.Value}) ");
            return text.ToString();
        }
    }

}
