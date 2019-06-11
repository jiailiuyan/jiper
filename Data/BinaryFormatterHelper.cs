using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ji.DataHelper
{
    public static class BinaryFormatterHelper
    {
        public static bool Save(this object data, string filepath)
        {
            if (data != null)
            {
                try
                {
                    var ms = new System.IO.MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, data);
                    ms.Position = 0;
                    byte[] datas = new byte[ms.Length];
                    ms.Read(datas, 0, datas.Length);
                    ms.Close();

                    File.WriteAllBytes(filepath, datas);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static T Read<T>(string filepath)
        {
            try
            {
                using (var fs = File.OpenRead(filepath))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(fs);
                }
            }
            catch { }
            return default(T);
        }
    }
}