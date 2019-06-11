using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ji.CommonHelper.Data;
using Ji.CommonHelper.Http.Tasker.Params;

namespace Ji.CommonHelper.Http.Tasker.Util
{
    public class HttpTaskUtil
    {
        public static HttpWebResponse ExecuteResp(HttpWebRequest request, HttpTaskParams param)
        {
            SetHttpWebRequestParams(request, param);
            return (HttpWebResponse)request.GetResponse();
        }

        public static string ExecuteStringResp(HttpWebRequest request, HttpTaskParams param)
        {
            HttpWebResponse resp = null;
            StreamReader streamReader = null;
            try
            {
                SetHttpWebRequestParams(request, param);
                resp = (HttpWebResponse)request.GetResponse();
                Stream responseStream = resp.GetResponseStream();
                streamReader = new StreamReader(responseStream, Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                streamReader.Close();
                Close(resp);
                Abort(request);
            }
        }

        public static byte[] ExecuteBytesResp(HttpWebRequest request, HttpTaskParams param)
        {
            HttpWebResponse resp = null;
            MemoryStream ms = null;
            try
            {
                SetHttpWebRequestParams(request, param);
                resp = (HttpWebResponse)request.GetResponse();
                Stream respStream = resp.GetResponseStream();
                ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                int len = -1;
                while ((len = respStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, len);
                }

                return ms.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ms.Close();
                Close(resp);
                Abort(request);
            }
        }

        /// <summary> 创建HttpWebRequest请求对象 </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpWebRequest(HttpTaskParams param)
        {
            switch (param.Method)
            {
                case HttpTaskParams.METHOD_POST:
                    return createHttpWebPostRequest(param);

                default:
                    return createHttpWebGetRequest(param);
            }
        }

        /// <summary> 创建Post请求对象 </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static HttpWebRequest createHttpWebPostRequest(HttpTaskParams param)
        {
            return (HttpWebRequest)HttpWebRequest.Create(param.Url);
        }

        /// <summary> 创建Get请求对象 </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static HttpWebRequest createHttpWebGetRequest(HttpTaskParams param)
        {
            if (param.getStringParams() == null)
                return (HttpWebRequest)HttpWebRequest.Create(param.Url);

            StringBuilder urlSb = new StringBuilder();
            urlSb.Append(param.Url);
            if (param.HasStringParams())
            {
                if (param.Url.Contains("?"))
                    urlSb.Append('?');

                if (param.Url.Contains("&"))
                    urlSb.Append("&");
            }

            urlSb.Append(GetStringByPairs(param.getStringParams(), param.IsAutoEncoding()));
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlSb.ToString());
            return request;
        }

        /// <summary> 设置所有请求参数 </summary>
        /// <param name="request"></param>
        /// <param name="param">  </param>
        public static void SetHttpWebRequestParams(HttpWebRequest request, HttpTaskParams param)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            request.Timeout = 30 * 1000;
            request.Proxy = null;
            //Debug.WriteLine("****** time out = "+request.Timeout+", proxy="+request.Proxy+ ", limit=" + ServicePointManager.DefaultConnectionLimit);
            SetHttpWebRequestHeaders(request, param);
            SetHttpWebRequestBodyContent(request, param);
        }

        //private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    Debug.WriteLine("accept name = "+ certificate.GetName());
        //    return true;
        //}

        /// <summary> 设置请求头相关信息 </summary>
        /// <param name="request"></param>
        /// <param name="param">  </param>
        public static void SetHttpWebRequestHeaders(HttpWebRequest request, HttpTaskParams param)
        {
            if (request == null || param == null)
                return;

            request.Method = ConvertHttpWebRequestMethodString(param.Method);
            request.Headers.Add("Charset", "UTF-8");
            if (string.IsNullOrEmpty(param.ContentType))
            {
                if (!param.HasFileParams() && param.Method == HttpTaskParams.METHOD_POST)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
            }
            else
            {
                request.ContentType = param.ContentType;
            }
        }

        /// <summary> 设置请求体 </summary>
        /// <param name="request"></param>
        /// <param name="param">  </param>
        public static void SetHttpWebRequestBodyContent(HttpWebRequest request, HttpTaskParams param)
        {
            if (request == null || param == null)
                return;

            if (param.Method == HttpTaskParams.METHOD_GET)
            {
                //nothing,get 请求没有请求体，也不允许写请求体?
            }
            else
            {
                //POST 或 其他类型，优先写自定义请求体
                if (string.IsNullOrEmpty(param.CustomStringContentBody))
                {
                    if (param.getFileParams() == null)
                        //写post请求体
                        SetHttpWebRequestStringBodyContent(request, GetStringByPairs(param.getStringParams(), param.IsAutoEncoding()));
                    else
                        //写上传请求体
                        SetHttpWebRequestUploadBodyContent(request, param);
                }
                else
                {
                    //写自定义请求体
                    SetHttpWebRequestStringBodyContent(request, param.CustomStringContentBody);
                }
            }
        }

        /// <summary>
        ///  设置文本请求体 如果当前网络未联通，request.GetRequestStream() 会抛出 ExecuteString exception=如果设置
        ///  ContentLength&gt;0 或 SendChunked==true，则必须提供请求正文。 在 [Begin]GetResponse 之前 通过调用 [Begin]GetRequestStream，可执行此操作。
        /// </summary>
        /// <param name="request">          </param>
        /// <param name="stringBodyContent"></param>
        private static void SetHttpWebRequestStringBodyContent(HttpWebRequest request, string stringBodyContent)
        {
            if (request == null || string.IsNullOrEmpty(stringBodyContent))
                return;

            Stream stream = request.GetRequestStream();
            byte[] bytes = Encoding.UTF8.GetBytes(stringBodyContent);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        /// <summary> 设置上传请求体 </summary>
        /// <param name="request"></param>
        /// <param name="param">  </param>
        private static void SetHttpWebRequestUploadBodyContent(HttpWebRequest request, HttpTaskParams param)
        {
            if (request == null || param == null)
                return;

            // 1.构造分界线标识
            string boundary = string.Format("----{0}", DateTime.Now.Ticks.ToString("x"));       // 分界线可以自定义参数
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(string.Format("\r\n--{0}\r\n", boundary));

            //2.上传header属性设置
            request.Method = "POST";
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

            byte[] bytes = null;
            Stream stream = request.GetRequestStream();

            //4.写 key value 数据
            List<NameValuePair> pairs = param == null ? null : param.getStringParams();
            if (pairs != null && pairs.Count > 0)
            {
                string pairStr;
                NameValuePair pair;
                for (int i = 0; i < pairs.Count; i++)
                {
                    //写分割线
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    //写key value
                    pair = pairs[i];
                    pairStr = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", pair.Name, pair.Value);
                    bytes = Encoding.UTF8.GetBytes(pairStr);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            //5.写上传文件数据
            List<NameFilePair> filePairs = param == null ? null : param.getFileParams();
            if (filePairs != null && filePairs.Count > 0)
            {
                NameFilePair filePair;
                string pairStr;
                string fileName;
                string contentType;

                byte[] buffer = new byte[1024 * 100];
                int bytesRead = 0;

                for (int i = 0; i < filePairs.Count; i++)
                {
                    //写分割线
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    //写文件属性
                    filePair = filePairs[i];
                    fileName = string.IsNullOrEmpty(filePair.FileName) ? new FileInfo(filePair.FilePath).Name : filePair.FileName;
                    contentType = string.IsNullOrEmpty(filePair.ContentType) ? "application/octet-stream" : filePair.ContentType;
                    pairStr = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", filePair.Name, fileName, contentType);
                    bytes = Encoding.UTF8.GetBytes(pairStr);
                    stream.Write(bytes, 0, bytes.Length);

                    //写文件数据
                    using (FileStream fileStream = new FileStream(filePair.FilePath, FileMode.Open, FileAccess.Read))
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }

            // 6.写结束分界线
            bytes = Encoding.UTF8.GetBytes(string.Format("\r\n--{0}--\r\n", boundary));
            stream.Write(bytes, 0, bytes.Length);

            //7.关闭请求流
            stream.Close();
        }

        /// <summary> 构造请求参数字符串 </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        private static string GetStringByPairs(List<NameValuePair> pairs, bool needEncoding)
        {
            if (pairs == null)
                return string.Empty;

            NameValuePair pair = null;
            StringBuilder pairSb = new StringBuilder();
            for (int i = 0; i < pairs.Count; i++)
            {
                pair = pairs[i];
                if (i != 0)
                    pairSb.Append('&');

                pairSb.Append(pair.Name);
                pairSb.Append('=');

                // 迹 替换UrlEncode
                pairSb.Append(needEncoding ? UrlHelper.UrlEncode(pair.Value) : pair.Value);
                //pairSb.Append(needEncoding ? HttpUtility.UrlEncode(pair.Value) : pair.Value);
                //Uri.EscapeUriString(pair.GetValue())
                //pairSb.Append(pair.Value);
            }

            return pairSb.ToString();
        }

        public static string ConvertHttpWebRequestMethodString(int method)
        {
            switch (method)
            {
                case HttpTaskParams.METHOD_POST:
                    return "POST";

                case HttpTaskParams.METHOD_OPTIONS:
                    return "OPTIONS";

                case HttpTaskParams.METHOD_GET:
                default:
                    return "GET";
            }
        }

        public static void Abort(HttpWebRequest request)
        {
            try
            {
                if (request != null)
                    request.Abort();
            }
            catch { }
        }

        public static void Close(HttpWebResponse resp)
        {
            try
            {
                if (resp != null)
                    resp.Close();
            }
            catch { }
        }
    }
}