/* 迹I柳燕
 *
 * FileName:   ConnectHelper.cs
 * Version:    1.0
 * Date:       2018/9/8 15:07:58
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Http
 * @class      ConnectHelper
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace Ji.CommonHelper.Http
{
    /// <summary>  </summary>
    public class ConnectHelper
    {
        private static readonly object ConnectServerLockData = new object();

        private static Dictionary<string, DateTime?> Servers = new Dictionary<string, DateTime?>();

        public static bool CanConnectServer(string su)
        {
            if (string.IsNullOrWhiteSpace(su))
            {
                return false;
            }

            lock (ConnectServerLockData)
            {
                // 包含且在10s成功连接，直接认为正常联通中
                if (Servers.ContainsKey(su))
                {
                    var lt = Servers[su];
                    if (lt != null)
                    {
                        var ts = (DateTime.Now - lt.Value).TotalSeconds;
                        if (ts < 10)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    Servers.Add(su, null);
                }
            }

            bool isconnet = false;
            var serverurl = su;

            if (serverurl.Contains("?"))
            {
                serverurl += "&r=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }
            else
            {
                serverurl += "?r=" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }

            var request = WebRequest.Create(serverurl) as HttpWebRequest;
            request.Accept = ("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Method = "GET";
            request.ServicePoint.Expect100Continue = false;
            request.UserAgent = "ConnectTest";
            request.Timeout = 20000;
            request.AllowAutoRedirect = true;

            try
            {
                var s = request.GetResponse() as HttpWebResponse;
                if (s != null)
                {
                    isconnet = s.StatusCode == HttpStatusCode.OK;
                    s.Close();

                    lock (ConnectServerLockData)
                    {
                        if (isconnet)
                        {
                            Servers[su] = DateTime.Now;
                        }
                        else
                        {
                            Servers[su] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
                Debug.WriteLine(ex.Message +" => "+ex.StackTrace);
            }

            return isconnet;
        }

    }
}
