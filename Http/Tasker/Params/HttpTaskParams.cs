using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Params
{
    /// <summary> HttpTask 请求参数集合对象 </summary>
    public class HttpTaskParams
    {
        public const int METHOD_GET = 0;
        public const int METHOD_POST = 1;
        public const int METHOD_OPTIONS = 2;

        public string Url { get; set; }
        public int Method { get; set; }

        public string ContentType { set; get; }
        public string CustomStringContentBody { set; get; }

        private bool AutoEncoding = true;
        private List<NameValuePair> StringParams;
        private List<NameFilePair> FileParams;

        private HttpTaskParams(String url, int method)
        {
            Url = url;
            Method = method;
        }

        public static HttpTaskParams NewGet(string url)
        {
            return new HttpTaskParams(url, METHOD_GET);
        }

        public static HttpTaskParams NewPost(string url)
        {
            return new HttpTaskParams(url, METHOD_POST);
        }

        public static HttpTaskParams NewOptions(string url)
        {
            return new HttpTaskParams(url, METHOD_OPTIONS);
        }

        public void SetAutoEncoding(bool auto)
        {
            AutoEncoding = auto;
        }

        public bool IsAutoEncoding()
        {
            return AutoEncoding;
        }

        public void AddStringParam(string key, string value)
        {
            if (StringParams == null)
                StringParams = new List<NameValuePair>();

            if (!string.IsNullOrEmpty(key))
                StringParams.Add(new NameValuePair(key, value));
        }

        public void AddFileParam(string key, string filePath)
        {
            if (FileParams == null)
                FileParams = new List<NameFilePair>();

            FileParams.Add(new NameFilePair(key, filePath));
        }

        public void AddFileParam(string key, string filePath, string contentType)
        {
            if (FileParams == null)
                FileParams = new List<NameFilePair>();

            FileParams.Add(new NameFilePair(key, filePath, contentType));
        }

        public List<NameValuePair> getStringParams()
        {
            return StringParams;
        }

        public List<NameFilePair> getFileParams()
        {
            return FileParams;
        }

        public bool HasStringParams()
        {
            return StringParams != null;
        }

        public bool HasFileParams()
        {
            return StringParams != null;
        }
    }
}