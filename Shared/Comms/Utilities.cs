using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Sean.Shared.Comms
{
    public static class Utilities
    {
        public static string JsonSerialize<T>(T t)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
            {
                serializer.WriteObject(ms, t);
                ms.Position = 0;
                return sr.ReadToEnd();
            }
        }

        public static T JsonDeserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }

    }
}
