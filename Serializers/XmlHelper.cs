/* 迹I柳燕
 *
 * FileName:   XmlHelper.cs
 * Version:    1.0
 * Date:       2018/8/12 12:07:41
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Serializers
 * @class      XmlHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Ji.CommonHelper.Serializers
{
    /// <summary>  </summary>
    public class XmlHelper
    {
        public static T ToData<T>(string body)
              where T : class
        {
            return ToData<T>(body, Encoding.UTF8);
        }

        public static T ToData<T>(string body, Encoding encoding)
            where T : class
        {
            try
            {
                string toDes = body;
                if (!string.IsNullOrWhiteSpace(toDes))
                {
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(toDes)))
                    {
                        var deseralizer = new XmlSerializer(typeof(T));
                        var data = deseralizer.Deserialize(ms) as T;
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }

            return default(T);
        }

        public static string ToXml(object data)
        {
            using (var stream = new MemoryStream())
            {
                var serialize = new XmlSerializer(data.GetType());
                serialize.Serialize(stream, data);
                var str = Encoding.UTF8.GetString(stream.ToArray());
                return str;
            }
        }
    }
}