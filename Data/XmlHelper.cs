/* 迹I柳燕
 *
 * FileName:   XmlHelper.cs
 * Version:    2.0
 * Date:       2015.07.13
 * Author:     Ji
 *
 *========================================
 *
 * @namespace  Helper.JilyData
 * @class      XmlData
 *             XmlAction
 * @extends
 *
 *             对于Xml的保存和读取
 *
 *========================================

 * (http://www.jiailiuyan.com)
 *
 *
 *
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

/*
[Serializable]
public class StringProperty
{
    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }
}

[Serializable]
[XmlType(Namespace = "http://schemas.citrix.com/2014/xd/machinecreation")]
public class CustomProperties : List<StringProperty>
{
}

    CustomProperties c = new APP.App.CustomProperties();
            c.Add(new StringProperty() { Name = "TestName", Value = "TestValue" });

            try
            {
                var obj = c;
                var type = typeof(CustomProperties);
                var filePath = "C:/1.xml";

                var settings = new XmlWriterSettings();
                settings.CloseOutput = true;
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;

                settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

                using (var writer = XmlWriter.Create(filePath, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("namespace", "http://schemas.citrix.com/2014/xd/machinecreation");
                    var formatter = new XmlSerializer(type);
                    formatter.Serialize(writer, obj, ns);
                }
            }
            catch { }

            var d = XmlData.ReadFromXml<CustomProperties>("C:/1.xml");

 * */

namespace Ji.DataHelper
{
    /// <summary> 保存和读取 xml 数据 </summary>
    public static class XmlData
    {
        /// <summary> 从XML读取数据 </summary>
        /// <typeparam name="T"> 读取的数据类型 </typeparam>
        /// <param name="fileInfo"> 包含数据的文件 FileInfo 信息 </param>
        /// <returns> 返回为 null 的时候读取失败 </returns>
        public static T ReadFromXml<T>(this FileInfo fileInfo)
        {
            return ReadFromXml<T>(fileInfo.FullName);
        }

        /// <summary> 从XML读取数据 </summary>
        /// <typeparam name="T"> 读取的数据类型 </typeparam>
        /// /// 
        /// <param name="FullPath"> 包含数据的文件路径 </param>
        /// <returns> Tpye = C ， 返回为null的时候读取失败 </returns>
        public static T ReadFromXml<T>(this string FullPath)
        {
            return XmlAction.Read<T>(FullPath);
        }

        public static object ReadFromXml(this Stream stream, Type type)
        {
            return XmlAction.Read(stream, type);
        }

        /// <summary> 写入数据到 XML </summary>
        /// <typeparam name="T"> 读取的数据类型 </typeparam>
        /// <param name="obj">      将要写入的数据 </param>
        /// <param name="fullpath"> 写 =入的文件路径 </param>
        /// <returns> 返回为 null 的时候写入成功 </returns>
        public static string WriteToXml<T>(this T obj, string fullpath)
        {
            try
            {
                FileHelper.CreatFile(fullpath);

                if (XmlAction.Save(obj, fullpath))
                {
                    return null;
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static T ReadFromStream<T>(string path)
        {
            FileStream fs = null;
            MemoryStream ms = null;
            try
            {
                fs = new FileStream(path, FileMode.OpenOrCreate);
                var bytes = new byte[1024];
                var len = -1;
                ms = new MemoryStream();
                while ((len = fs.Read(bytes, 0, bytes.Length)) > 0)
                {
                    ms.Write(bytes, 0, len);
                }

                return ReadFromStream<T>(ms);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (fs != null)
                        fs.Dispose();
                }
                catch { }

                try
                {
                    if (ms != null)
                        ms.Dispose();
                }
                catch { }
            }

            return default(T);
        }

        /// <summary> 从XML读取数据 </summary>
        /// <typeparam name="T"> 读取的数据类型 </typeparam>
        /// <param name="stream"> 包含数据的数据流 </param>
        /// <returns> Tpye = C ， 返回为null的时候读取失败 </returns>
        public static T ReadFromStream<T>(this Stream stream)
        {
            return XmlAction.Read<T>(stream);
        }

        public static bool WriteToStream<T>(this T obj, string path)
        {
            Stream stream = null;
            FileStream fileStream = null;
            try
            {
                stream = WriteDataToStream<T>(obj);
                if (stream == null)
                    return false;

                fileStream = new FileStream(path, FileMode.OpenOrCreate);
                var bytes = new byte[1024];
                var len = -1;
                while ((len = stream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    fileStream.Write(bytes, 0, len);
                }

                return true;
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (stream != null)
                        stream.Dispose();
                }
                catch { }

                try
                {
                    if (fileStream != null)
                        fileStream.Dispose();
                }
                catch { }
            }

            return false;
        }

        public static Stream WriteDataToStream<T>(this T obj)
        {
            try
            {
                return XmlAction.SaveStream(obj);
            }
            catch { }
            return null;
        }
    }

    /// <summary> 封装对与Xml读写操作的类 <see cref="string" /> </summary>
    internal static class XmlAction
    {
        internal static bool Save(object obj, string filePath)
        {
            return Save(obj, filePath, obj.GetType());
        }

        internal static bool Save(object obj, string filePath, System.Type type)
        {
            try
            {
                var memorystream = new MemoryStream();
                var writer = new StreamWriter(memorystream);
                var xs = new System.Xml.Serialization.XmlSerializer(type);
                xs.Serialize(writer, obj);
                var datas = new byte[memorystream.Length];
                memorystream.Position = 0;
                memorystream.Read(datas, 0, datas.Length);
                writer.Close();
                memorystream.Close();

                try
                {
                    File.WriteAllBytes(filePath, datas);
                }
                catch
                {
                    File.WriteAllBytes(filePath, datas);
                }

                return true;
            }
            catch (Exception ex)
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"\Ji\Logs\Errors", "Locals");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var path = Path.Combine(dir, "E_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                var w = ex.Message + "\r\n" + ex.StackTrace;
                File.AppendAllText(path, w);
            }
            return false;
        }

        internal static Stream SaveStream(object obj)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var xs = new XmlSerializer(obj.GetType());
            xs.Serialize(writer, obj);
            stream.Position = 0;
            return stream as Stream;
        }

        internal static T Read<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    var datas = File.ReadAllBytes(filePath);
                    if (datas?.Count() > 0)
                    {
                        using (var ms = new MemoryStream(datas))
                        {
                            using (var reader = XmlReader.Create(ms))
                            {
                                var type = typeof(T);
                                var xs = new XmlSerializer(type);
                                if (xs.CanDeserialize(reader))
                                {
                                    var obj = xs.Deserialize(reader);
                                    reader.Close();
                                    return (T)obj;
                                }
                                else
                                {
                                    Debug.WriteLine("无法序列化 " + filePath);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }
            return default(T);
        }

        internal static object Read(Stream stream, Type type)
        {
            if (stream != null && stream.Length != 0)
            {
                try
                {
                    return (new XmlSerializer(type)).Deserialize(stream);
                }
                catch { }
            }
            return null;
        }

        internal static T Read<T>(Stream stream)
        {
            if (stream != null && stream.Length != 0)
            {
                try
                {
                    var type = typeof(T);
                    return (T)(new XmlSerializer(type)).Deserialize(stream);
                }
                catch { }
            }
            return default(T);
        }
    }
}