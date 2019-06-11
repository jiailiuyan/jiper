// *********************************************************************** 项目名称 :
// Ji.CommonHelper.FSHelper 类名称 : KS3Helper 作者 : Administrator 所在域 ：MXZ-PC 创建日期 : 2018/10/24 9:17:53
//
// 最后编辑人员 : Administrator 最后编辑日期 : 2018/10/24 9:17:53 ***********************************************************************
// <copyright file="KS3Helper.cs" company="Kingsoft">
//     Copyright © Administrator 2018. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using KS3;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ji.CommonHelper.FSHelper
{
    public class KS3Helper : IFSHelper
    {
        private static string accessKey = "";
        private static string secretKey = "";
        private static string bucketName = "";
        private static string endpoint = "";
        private static KS3Client ks3Client = null;

        public static void InitKS3Helper(string aKey, string sKey, string bName, string ePoint)
        {
            accessKey = aKey;
            secretKey = sKey;
            bucketName = bName;
            endpoint = ePoint;
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("accessKey or secretKey");
            }
            try
            {
                ks3Client = new KS3Client(accessKey, secretKey);
                ks3Client.setEndpoint(endpoint);
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emrTmp");
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ServerUrl { get; set; }
        public string Sqbm { get; set; }
        public string ClientModel { get; set; }

        public bool DeleteFile(string serverurl, string skey)
        {
            bool flag = true;
            try
            {
                ks3Client.deleteObject(bucketName, skey);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                throw ex;
            }
            return flag;
        }

        public byte[] DownloadFileBytes(string serverurl, string skey)
        {
            string path = this.DownloadFile(serverurl, skey);
            return File.ReadAllBytes(path);
        }

        public string DownloadFile(string serverurl, string skey)
        {
            string res = string.Empty;
            if (ks3Client != null)
            {
                try
                {
                    string outFilePath = Path.Combine(GetEMRTmpPath(), Sqbm + "." + DateTime.Today.ToString("yyyy.MM.dd.") + DateTime.Now.Ticks + ".ksc");
                    GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, skey, new FileInfo(outFilePath));
                    KS3Object obj = ks3Client.getObject(getObjectRequest);
                    obj.getObjectContent().Close();
                    res = outFilePath;
                }
                catch (Exception ex)
                {
                    res = string.Empty;
                    throw ex;
                }
            }
            return res;
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

        public string UploadFile(string serverurl, byte[] content)
        {
            string filePath = GetEMRTmpPath() + "/" + Guid.NewGuid().ToString("N");
            File.WriteAllBytes(filePath, content);
            return this.UploadFile(serverurl, filePath);
        }

        public string UploadFile(string serverurl, string sfilename)
        {
            string res = string.Empty;
            if (ks3Client != null)
            {
                try
                {
                    FileInfo file = new FileInfo(sfilename);
                    string objKeyName = "emr/" + ClientModel + "/" + Sqbm + "/" + DateTime.Today.ToString("yyyy/MM/dd/") + DateTime.Now.Ticks + ".ksc";
                    PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyName, file);
                    CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ);
                    putObjectRequest.setCannedAcl(cannedAcl);
                    PutObjectResult putObjectResult = ks3Client.putObject(putObjectRequest);
                    res = objKeyName;
                }
                catch (Exception ex)
                {
                    res = string.Empty;
                    throw ex;
                }
            }
            return res;
        }
    }
}