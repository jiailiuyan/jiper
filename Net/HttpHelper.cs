/* 迹I柳燕
 *
 * FileName:   JilyHelperHttp.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.NetHelper
 * @class      JilyHelperHttp
 * @extends
 *
 *             对于 Http 的数据包装
 *
 *========================================

 * 
 *
 * 
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Ji.NetHelper
{
    /// <summary> 基于Http的数据请求 </summary>
    public static class HttpHelper
    {
        ///// <summary> 默认设置当前的标识为IE7 </summary>
        //public const string IE7 = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";

        /// <summary> 默认设置当前的标识为I </summary>
        public const string SetterSign = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.3) Gecko/20070309 Firefox/2.0.0.3";

        /// <summary> 设置Cookie容器 此项能在需要的时候保存Cookies </summary>
        public static CookieContainer CookieContainers = new CookieContainer();

        public static Dictionary<string, string> Cookies = new Dictionary<string, string>();

        /// <summary> 设置需要证书请求的时候默认为true </summary>
        static HttpHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
        }

        /// <summary> 向HTTP流中添加数据头 </summary>
        /// <param name="url"> 请求的URL </param>
        /// <param name="method"> 请求使用的方法 GET、POST </param>
        /// <returns> 返回创建的 HttpWebRequest </returns>
        private static HttpWebRequest CreatRequest(this string url, string method, List<string> headers = null)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            //优化多线程超时响应
            req.KeepAlive = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;

            req.Method = method.ToUpper();
            req.AllowAutoRedirect = true;
            req.CookieContainer = CookieContainers;
            req.ContentType = "application/x-www-form-urlencoded";

            req.UserAgent = SetterSign;
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add("Accept-Language:zh-CN");
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    req.Headers.Add(item);
                }
            }

            req.Timeout = 5000;

            return req;
        }

        /// <summary> 根据URL获取回传的 Stream 无编码格式的确认 </summary>
        /// <param name="url"> 请求的URL </param>
        /// <returns> 返回的数据流 </returns>
        public static Stream GetStreamResponse(this string url, string method = "get", string data = "", List<string> headers = null)
        {
            try
            {
                var req = CreatRequest(url, method, headers);

                if (method.ToUpper() == "POST" && data != null)
                {
                    var postBytes = new ASCIIEncoding().GetBytes(data);
                    req.ContentLength = postBytes.Length;
                    Stream st = req.GetRequestStream();
                    st.Write(postBytes, 0, postBytes.Length);
                    st.Close();
                }
                //System.Debug.WriteLine();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                //return res.GetResponseStream();
                using (var stream = res.GetResponseStream())
                {
                    //优化多线程内存流的释放
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);

                    //接收到的数据流需要重新设置读取起始位
                    ms.Position = 0;

                    //优化多线程多个实例时的端口占用
                    res.Close();
                    req.Abort();

                    stream.Flush();
                    stream.Close();

                    return ms;
                }
            }
            catch { }

            return null;
        }

        /// <summary> 以字符串形式获取返回值 </summary>
        /// <param name="url"> 请求的URl </param>
        /// <param name="method"> 传递方法 </param>
        /// <param name="data"> 传递数据 </param>
        /// <returns> 返回的字符串 UTF-8 编码 </returns>
        public static string GetStringResponse(this string url, string method = "get", string data = "", List<string> headers = null)
        {
            return GetStringResponse(url, Encoding.UTF8, method, data, headers);
        }

        /// <summary> 以字符串形式获取返回值 </summary>
        /// <param name="url"> 请求的URl </param>
        /// <param name="encoding"> 编码格式 </param>
        /// <param name="method"> 传递方法 </param>
        /// <param name="data"> 传递数据 </param>
        /// <returns> 返回指定编码的字符串 </returns>
        public static string GetStringResponse(this string url, Encoding encoding, string method = "get", string data = "", List<string> headers = null)
        {
            try
            {
                var req = CreatRequest(url, method, headers);

                if (method.ToUpper() == "POST" && data != null)
                {
                    var postBytes = new ASCIIEncoding().GetBytes(data);
                    req.ContentLength = postBytes.Length;
                    Stream st = req.GetRequestStream();
                    st.Write(postBytes, 0, postBytes.Length);
                    st.Close();
                }

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                foreach (Cookie cookie in res.Cookies)
                {
                    Cookies[cookie.Name] = cookie.Value;
                    CookieContainers.Add(cookie);
                }

                //优化多线程内存流的释放
                using (var stream = res.GetResponseStream())
                {
                    var sr = new StreamReader(stream, encoding).ReadToEnd();

                    //优化多线程多个实例时的端口占用
                    res.Close();
                    req.Abort();
                    return sr;
                }
            }
            catch { }

            return string.Empty;
        }

        public static List<byte> GetListByteResponse(this string url, byte[] datas)
        {
            try
            {
                var req = CreatRequest(url, "POST");
                req.ContentLength = datas.Length;

                using (var st = req.GetRequestStream())
                {
                    st.Write(datas, 0, datas.Length);
                    st.Close();
                }
                var res = (HttpWebResponse)req.GetResponse();
                var rs = res.GetResponseStream();
                var rd = new List<byte>();
                while (true)
                {
                    var b = rs.ReadByte();
                    if (b == -1)
                    {
                        break;
                    }
                    rd.Add((byte)b);
                }

                //优化多线程多个实例时的端口占用
                res.Close();
                req.Abort();
                return rd;
            }
            catch { }

            return null;
        }

        /// <summary> 以字符串形式获取返回值 </summary>
        /// <param name="url"> 请求的URl </param>
        /// <param name="encoding"> 编码格式 </param>
        /// <param name="method"> 传递方法 </param>
        /// <param name="data"> 传递数据 </param>
        /// <returns> 返回指定编码的字符串 </returns>
        public static string PostStringResponse(this string url, Encoding encoding, string data)
        {
            try
            {
                var req = CreatRequest(url, "post", null);

                var postBytes = encoding.GetBytes(data);
                req.ContentLength = postBytes.Length;
                Stream st = req.GetRequestStream();
                st.Write(postBytes, 0, postBytes.Length);
                st.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                //优化多线程内存流的释放
                using (var stream = res.GetResponseStream())
                {
                    var sr = new StreamReader(stream, encoding).ReadToEnd();

                    //优化多线程多个实例时的端口占用
                    res.Close();
                    req.Abort();
                    return sr;
                }
            }
            catch { }

            return string.Empty;
        }

        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);
            //请求头部信息
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();
            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }
    }
}