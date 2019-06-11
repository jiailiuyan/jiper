// ***********************************************************************
// 项目名称       : Ji.CommonHelper.FSHelper
// 类名称         : FSHelper
// 作者           : Administrator
// 所在域		  ：MXZ-PC
// 创建日期       : 2018/10/24 10:27:03
//
// 最后编辑人员   : Administrator
// 最后编辑日期   : 2018/10/24 10:27:03
// ***********************************************************************
// <copyright file="FSHelper.cs" company="Kingsoft">
//     Copyright © Administrator 2018. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Ji.CommonHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.FSHelper
{
    public class FSHelper
    {
        private static readonly object locker = new object();
        private static IFSHelper _instance;
        private const string clinetKey = "CLIENT_MODE";
        private const string ks3Key = "KS3Url";
        private const string DEFAULT_CLIENT_MODE = "22";
        private const string DEFAULT_KS3_URL = @"http://his.ks3-cn-beijing.ksyun.com/";
        private static string _kS3Url = string.Empty;
        private static string _clientMode = string.Empty;
        private static string SQBM = string.Empty;
        public static void Init(string accessKey, string secretKey, string bucketName, string endPoint, string _SQBM)
        {
            try
            {
                KS3Helper.InitKS3Helper(accessKey, secretKey, bucketName, endPoint);
                HDFSHelper.InitHDFSHelper();
                SQBM = _SQBM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private FSHelper() { }
        public static IFSHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            if (KS3_URL.ToLower().Contains("ks3"))
                            {
                                _instance = new KS3Helper { ServerUrl = KS3_URL, ClientModel = CLIENT_MODE, Sqbm = SQBM };
                            }
                            else
                            {
                                _instance = new HDFSHelper { ServerUrl = KS3_URL, ClientModel = CLIENT_MODE, Sqbm = SQBM };
                            }
                        }
                    }
                }
                return _instance;
            }
        }
        private static string KS3_URL
        {
            get
            {
                if (string.IsNullOrEmpty(_kS3Url))
                {
                    ConfigManagerHelper.ChangeConfigPath();
                    _kS3Url = ConfigManagerHelper.GetValue<string>(ks3Key);
                    if (string.IsNullOrEmpty(_kS3Url))
                    {
                        Ji.CommonHelper.Log.Logger.Warn("the ks3url is empty, and use default value:{0}", DEFAULT_KS3_URL);
                        _kS3Url = DEFAULT_KS3_URL;
                    }
                }
                return _kS3Url;
            }
        }

        public static string CLIENT_MODE
        {
            get
            {
                if (string.IsNullOrEmpty(_clientMode))
                {
                    ConfigManagerHelper.ChangeConfigPath();
                    _clientMode = ConfigManagerHelper.GetValue<string>(clinetKey);
                    if (string.IsNullOrEmpty(_clientMode))
                    {
                        Ji.CommonHelper.Log.Logger.Warn("the client mode is empty, and use default value:{0}", DEFAULT_CLIENT_MODE);
                        _clientMode = DEFAULT_CLIENT_MODE;
                    }
                }
                return _clientMode;
            }
        }
    }
}
