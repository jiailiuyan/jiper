// *********************************************************************** 项目名称 :
// Ji.CommonHelper.FSHelper 类名称 : HDFSHelper 作者 : Administrator 所在域 ：MXZ-PC 创建日期 : 2018/10/24 9:28:04
//
// 最后编辑人员 : Administrator 最后编辑日期 : 2018/10/24 9:28:04 ***********************************************************************
// <copyright file="HDFSHelper.cs" company="Kingsoft">
//     Copyright © Administrator 2018. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ji.CommonHelper.FSHelper
{
    public class HDFSHelper : IFSHelper
    {
        public string ServerUrl { get; set; }
        public string Sqbm { get; set; }
        public string ClientModel { get; set; }

        public bool DeleteFile(string serverurl, string skey)
        {
            bool flag = true;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = WebRequest.Create(serverurl + "/" + skey) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "DELETE";
                response = request.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return flag;
        }

        internal static void InitHDFSHelper()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emrTmp");
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public byte[] DownloadFileBytes(string serverurl, string skey)
        {
            string path = this.DownloadFile(serverurl, skey);
            return File.ReadAllBytes(path);
        }

        public string DownloadFile(string serverurl, string skey)
        {
            string fileName = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(serverurl + "/" + skey) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    fileName = GetEMRTmpPath() + "/" + Sqbm + "." + DateTime.Today.ToString("yyyy/MM/dd/") + DateTime.Now.Ticks + ".ksc";
                    using (FileStream fs = File.Create(fileName))
                    {
                        byte[] bytes = new byte[1024];
                        int n = 1;
                        while (n > 0)
                        {
                            n = stream.Read(bytes, 0, 1024);
                            fs.Write(bytes, 0, n);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fileName = string.Empty;
                throw ex;
            }
            return fileName;
        }

        public string UploadFile(string serverurl, byte[] content)
        {
            string filePath = GetEMRTmpPath() + "/" + Guid.NewGuid().ToString("N");
            File.WriteAllBytes(filePath, content);
            return this.UploadFile(serverurl, filePath);
        }

        public string UploadFile(string serverurl, string sfilename)
        {
            string skey = string.Empty;
            FileStream fs = null;
            Stream putStream = null;
            try
            {
                string objKeyName = "emr/" + ClientModel + "/" + Sqbm + "/" + DateTime.Today.ToString("yyyy/MM/dd/") + DateTime.Now.Ticks + ".ksc";
                HttpWebRequest request = WebRequest.Create(serverurl + "/" + objKeyName) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "PUT";
                fs = new FileStream(sfilename, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                putStream = request.GetRequestStream();
                putStream.Write(bArr, 0, bArr.Length);
                putStream.Close();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                content = content.Substring("his".Length + 1);
                skey = content;
            }
            catch (Exception ex)
            {
                skey = string.Empty;
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (putStream != null)
                {
                    putStream.Close();
                }
            }
            return skey;
        }

        private string GetEMRTmpPath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emrTmp");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}