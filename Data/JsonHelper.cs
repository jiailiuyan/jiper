using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Ji.DataHelper
{
    public class JsonHelper
    {
        public static string ObjectToJson<T>(T obj, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                var dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return encoding.GetString(dataBytes);
            }
        }

        public static T JsonToObject<T>(string jsonString, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var stream = new MemoryStream(encoding.GetBytes(jsonString)))
                {
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch { }
            return default(T);
        }
    }
}