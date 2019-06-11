using JZYD.TY.Common.Http.Tasker;
using JZYD.TY.Common.Http.Tasker.Params;
using JZYD.TY.Common.Util;
using JZYD.TY.WeChat.Http;
using JZYD.TY.WeChat.Http.Model;
using JZYD.TY.WeChat.Http.Model.Result;
using JZYD.TY.WeChat.Model;
using JZYD.TY.WeChat.Util;
using System;
using System.Net;
using System.Threading;
using static JZYD.TY.WeChat.Mgr.WxDelegate;

namespace JZYD.TY.WeChat.Task
{
    /// <summary> 微信线程任务 </summary>
    public class WxTask
    {
        public event OnWxLoginQrCodeResult OnWxTaskLoginQrCodeHandler;

        public event OnWxLoginPre OnWxTaskLoginPreHandler;

        public event OnWxLoginSuccess OnWxTaskLoginSuccessHandler;

        public event OnWxLoginFailed OnWxTaskLoginFailedHandler;

        public event OnWxLoginInitContact OnWxTaskLoginInitHandler;

        public event OnWxLoginAllContact OnWxTaskLoginGetContactAllHandler;

        public event OnWxReceiveMsg OnWxTaskReceiveMsgHandler;

        public event OnWxLogout OnWxTaskLogoutHandler;

        private const int STATE_LOGINING = 1;//正在登录
        private const int STATE_LOGINED = 2;//已登录
        private const int STATE_LOGIN_CANCEL = 3;//取消登录
        private const int STATE_LOGOUT_ACTIVE = 4;//主动登出
        private const int STATE_LOGOUT_FORCED = 5;//被迫登出
        private const int STATE_LOGOUT_FORCED_MSG_CLOSURE = 6;//上传文件受限被迫登出
        private const int STATE_LOGOUT_FORCED_ACCOUNT_CLOSURE = 7;//账号被限制

        private object mLockObj = new object();
        private Thread mTaskThread;
        private HttpTask mCurrentHttpTask;
        private int mWxState;
        private string mHost;
        private CookieContainer mCookieContainer;

        public WxTask()
        {
            mHost = WxHttpApi.URL_WX_HOST;
        }

        /// <summary> 开始工作线程 </summary>
        public void Start()
        {
            lock (mLockObj)
            {
                if (mTaskThread == null)
                {
                    mTaskThread = new Thread(new ThreadStart(run));
                    mTaskThread.IsBackground = true;
                    mTaskThread.Start();
                    setLoginState(STATE_LOGINING);//标记正在登录
                }
            }
        }

        /// <summary> 取消登录 </summary>
        public void CancelLogin()
        {
            lock (mLockObj)
            {
                ThreadUtil.Interrupt(mTaskThread);

                if (mCurrentHttpTask != null)
                    mCurrentHttpTask.Cancel();

                setLoginState(STATE_LOGIN_CANCEL);
                mTaskThread = null;
            }
        }

        /// <summary> 登出 </summary>
        public void Logout()
        {
            lock (mLockObj)
            {
                ThreadUtil.Interrupt(mTaskThread);

                if (mCurrentHttpTask != null)
                    mCurrentHttpTask.Cancel();

                setLoginState(STATE_LOGOUT_ACTIVE);
                mTaskThread = null;
            }
        }

        public void LogoutForMsgClosure()
        {
            lock (mLockObj)
            {
                ThreadUtil.Interrupt(mTaskThread);

                if (mCurrentHttpTask != null)
                    mCurrentHttpTask.Cancel();

                setLoginState(STATE_LOGOUT_FORCED_MSG_CLOSURE);
                mTaskThread = null;
            }
        }

        public void LogoutForAccountClosure()
        {
            lock (mLockObj)
            {
                ThreadUtil.Interrupt(mTaskThread);

                if (mCurrentHttpTask != null)
                    mCurrentHttpTask.Cancel();

                setLoginState(STATE_LOGOUT_FORCED_ACCOUNT_CLOSURE);
                mTaskThread = null;
            }
        }

        private void syncSetLoginState(int loginState)
        {
            lock (mLockObj)
            {
                setLoginState(loginState);
            }
        }

        private void setLoginState(int loginState)
        {
            mWxState = loginState;
        }

        private int getLoginState()
        {
            lock (mLockObj)
            {
                return mWxState;
            }
        }

        private bool syncCheckLoginingState()
        {
            lock (mLockObj)
            {
                return mWxState == STATE_LOGINING;
            }
        }

        private bool syncCheckLoginInState()
        {
            lock (mLockObj)
            {
                return mWxState == STATE_LOGINED;
            }
        }

        private WxLogoutCode syncGetLogoutCode(int syncCheckResultCode)
        {
            lock (mLockObj)
            {
                if (mWxState == STATE_LOGIN_CANCEL ||
                    mWxState == STATE_LOGOUT_ACTIVE)
                {
                    //主动登出
                    return WxLogoutCode.ACTIVE;
                }
                else if (mWxState == STATE_LOGOUT_FORCED_MSG_CLOSURE ||
                         syncCheckResultCode == WxResultCode.MSG_CLOSURE)
                {
                    //文件上传受限登出
                    return WxLogoutCode.FORCED_MSG_CLOSURE;
                }
                else if (mWxState == STATE_LOGOUT_FORCED_ACCOUNT_CLOSURE || 
                         syncCheckResultCode == WxResultCode.ACCOUNT_CLOSURE)
                {
                    //账号受限制(被封禁)登出
                    return WxLogoutCode.FORCED_ACCOUNT_CLOSURE;
                }
                else
                {
                    //其他离线状态
                    return WxLogoutCode.FORCED;
                }

                //switch (mWxState)
                //{
                //    case STATE_LOGIN_CANCEL:
                //    case STATE_LOGOUT_ACTIVE:
                //        return WxLogoutCode.ACTIVE;
                //    case STATE_LOGOUT_FORCED_MSG_CLOSURE:
                //        return WxLogoutCode.FORCED_MSG_CLOSURE;
                //    default:
                //        switch (wxResultCode)
                //        {
                //            case WxResultCode.ACCOUNT_CLOSURE:
                //            default:
                //                return WxLogoutCode.FORCED;
                //        }
                //}
            }
        }

        public string GetHost()
        {
            return mHost;
        }

        private void setHost(string host)
        {
            if (!string.IsNullOrEmpty(host))
                mHost = host;
        }

        public CookieContainer GetCookieContainer()
        {
            return mCookieContainer;
        }

        /// <summary> 线程任务 </summary>
        private void run()
        {
            //初始化cookie容器
            mCookieContainer = new CookieContainer();


            //获取微信登录随机设备Id
            string deviceId = WxUtil.GetRandomDeviceId();
#if Debug
            Console.WriteLine("WxTask login device id = " + deviceId);
#endif


            //获取登录uid------------------------------------------
            WxLoginUidResult loginUidResult = executeLoginUidHttpTask();
#if Debug
            Console.WriteLine("WxTask login uid = " + (loginUidResult == null ? "null" : loginUidResult.Uid));
#endif
            if (!syncCheckLoginingState())
                return;

            //检测uid
            if (!WxResultUtil.CheckLoginUidResult(loginUidResult))
            {
                callbackLoginFailed(WxLoginCode.UID_ERROR);
                return;
            }


            //获取微信登录二维码-----------------------------------------------
            byte[] qrCodeBytes = executeLoginQrCodeHttpTask(loginUidResult.Uid);
#if Debug
            Console.WriteLine("WxTask login qrcode = " + (qrCodeBytes == null ? "null" : "has qr code"));
#endif
            if (!syncCheckLoginingState())
                return;

            if (qrCodeBytes == null)
            {
                callbackLoginFailed(WxLoginCode.QRCODE_ERROR);
                return;
            }
            //二维码回调
            OnWxTaskLoginQrCodeHandler?.Invoke(qrCodeBytes);



            //检测扫描登录-----------------------------------------------------------------
            WxLoginScanResult loginScanResult = executeLoginScanHttpTask(loginUidResult.Uid);
#if Debug
            Console.WriteLine("WxTask login scan = " + (loginScanResult == null ? "null" : loginScanResult.ToString()));
#endif
            if (!syncCheckLoginingState())
                return;

            if (!WxResultUtil.CheckLoginScanResult(loginScanResult))
            {
                if (WxResultUtil.CheckLoginScanExpired(loginScanResult))
                    callbackLoginFailed(WxLoginCode.SCAN_EXPIRED);
                else
                    callbackLoginFailed(WxLoginCode.SCAN_ERROR);

                return;
            }


            //开始登录操作回调-----------------------------------
            OnWxTaskLoginPreHandler?.Invoke();


            //登录步骤完成，已获取各种唯一标识符，如果该步完成，手机端显示已登录--------------------------
            WxLoginIdsResult loginIdsResult = executeLoginRedirectUriHttpTask(loginScanResult.RedirectUri);
#if Debug
            Console.WriteLine("WxTask login ids = " + (loginIdsResult == null ? "null" : loginIdsResult.ToString()));
#endif
            //设置全局host
            if (WxResultUtil.CheckLoginIdsResult(loginIdsResult))
                setHost(loginIdsResult.RouteHost);

            if (!syncCheckLoginingState())
            {
                if (WxResultUtil.CheckLoginIdsResult(loginIdsResult))
                    executeLogout(GetHost(), loginIdsResult.Skey, loginIdsResult.Uin, loginIdsResult.Sid);

                return;
            }

            if (!WxResultUtil.CheckLoginIdsResult(loginIdsResult))
            {
                if (WxResultUtil.CheckLoginIdsAccountClosure(loginIdsResult))/*loginIdsResult != null && loginIdsResult.IsAccountException()*/
                    callbackLoginFailed(WxLoginCode.IDS_ACCOUNT_CLOSURE);
                else
                    callbackLoginFailed(WxLoginCode.IDS_ERROR);

                return;
            }


            //访问初始化接口,这里面才有昵称--------------------------------------------------------
            WxLoginInitResult loginInitResult = executeLoginInitHttpTask(GetHost(), loginIdsResult, deviceId);
#if Debug
            Console.WriteLine("WxTask login init = " + (loginInitResult == null ? "null" : loginInitResult.ToString()));
#endif
            if (!syncCheckLoginingState())
            {
                executeLogout(GetHost(), loginIdsResult.Skey, loginIdsResult.Uin, loginIdsResult.Sid);
                return;
            }

            if (!WxResultUtil.CheckResultSuccess(loginInitResult))
            {
                callbackLoginFailed(WxLoginCode.INIT_ERROR);
                executeLogout(GetHost(), loginIdsResult.Skey, loginIdsResult.Uin, loginIdsResult.Sid);
                return;
            }


            //创建登录账号对象--------------------------------------------------------
            WxAccount wxAccount = WxUtil.CreateWxAccount(loginIdsResult, loginInitResult, deviceId);


            //标记登录状态
            syncSetLoginState(STATE_LOGINED);


            //发送状态通知消息，一定要发
            if (syncCheckLoginInState())
                executeStatusNotifyHttpTask(GetHost(), wxAccount);


            //微信登录成功回调
            OnWxTaskLoginSuccessHandler?.Invoke(wxAccount);


            //微信初始化回调
            OnWxTaskLoginInitHandler?.Invoke(wxAccount, loginInitResult);


            //获取所有联系人，一定要先获取
            if (syncCheckLoginInState())
            {
                WxContactAllResult contactAllResult = executeGetContactAllHttpTask(GetHost(), wxAccount);
                if (WxResultUtil.CheckResultSuccess(contactAllResult) && syncCheckLoginInState())
                    OnWxTaskLoginGetContactAllHandler?.Invoke(wxAccount, contactAllResult.MemberList);
            }


            //开始循环同步消息
            int syncCode = WxResultCode.ACCOUNT_OFFLINE;
            if (syncCheckLoginInState())
                syncCode = executeSyncMsgLoop(GetHost(), wxAccount, loginInitResult.SyncKey);


            //先给登出回调，再做登出请求操作
            WxLogoutCode code = syncGetLogoutCode(syncCode);
            OnWxTaskLogoutHandler?.Invoke(wxAccount, code);
            string result = executeLogout(GetHost(), wxAccount);

#if Debug
            Console.WriteLine("WxTask logout WxLogoutCode = " + code + ", nickName=" + wxAccount.NickName);
#endif
        }

        /// <summary> 执行消息同步循环 </summary>
        /// <param name="wxAccount"></param>
        /// <param name="wxSyncKey"></param>
        private int executeSyncMsgLoop(string host, WxAccount wxAccount, WxSyncKey wxSyncKey)
        {
            //默认账号离线状态
            int code = WxResultCode.ACCOUNT_OFFLINE;

            while (true)
            {
                WxSyncCheckResult syncCheckResult = executeSyncCheckHttpTask(host, wxAccount, wxSyncKey);
#if Debug
                Console.WriteLine("host = " + host + ", synckey reuslt = " + (syncCheckResult == null ? "null" : syncCheckResult.Retcode.ToString()));
#endif
                if (!syncCheckLoginInState())
                {
                    //检测登录状态
                    break;
                }
                else if (syncCheckResult == null)
                {
                    //心跳请求异常
                }
                else if (syncCheckResult.IsStateSuccess)//0
                {
                    //心跳请求成功

                    if (syncCheckResult.Selector != 0)
                    {
                        //有消息需要同步

                        WxSyncMsgResult syncMsgResult = executeSyncMsgHttpTask(host, wxAccount, wxSyncKey);
#if Debug
                        Console.WriteLine("websync reuslt = " + (syncMsgResult == null ? "null" : syncMsgResult.BaseResponse.Ret.ToString()));
#endif

                        if (!syncCheckLoginInState())
                        {
                            //检测登录状态
                            break;
                        }
                        else if (syncMsgResult == null)
                        {
                            //同步消息请求异常
                        }
                        else if (syncMsgResult.IsStateSuccess)
                        {
                            //同步消息请求成功
                            wxSyncKey = syncMsgResult.SyncCheckKey;
                            OnWxTaskReceiveMsgHandler?.Invoke(wxAccount, syncMsgResult);
                        }
                        else
                        {
                            //同步消息请求失败
                            code = syncCheckResult.Retcode;
                            break;
                        }
                    }
                }
                else
                {
                    //心跳请求失败
                    code = syncCheckResult.Retcode;
                    break;
                }


                ThreadUtil.Sleep(1000);
                if (!syncCheckLoginInState())
                    break;
            }

            return code;
        }

        /// <summary> 获取登录随机uid </summary>
        /// <returns></returns>
        private WxLoginUidResult executeLoginUidHttpTask()
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetLoginUidParams());
                if (ht != null)
                {
                    string resp = ht.ExecuteString();
                    return WxRespUtil.ParseLoginUidResp(resp);
                }
            }
            catch { }

            return null;
        }

        /// <summary> 获取登录随机uid对应的二维码 </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private byte[] executeLoginQrCodeHttpTask(string uid)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetLoginQrCodeParams(uid));
                if (ht != null)
                {
                    byte[] qrCodeBytes = ht.ExecuteBytes();
                    return qrCodeBytes;
                }
            }
            catch { }

            return null;
        }

        /// <summary> 检测登录二维码扫描状态 </summary>
        /// <param name="uid"></param>
        private WxLoginScanResult executeLoginScanHttpTask(string uid)
        {
            try
            {
                string tip = "1";
                while (true)
                {
                    HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetLoginScanParams(uid, tip));
                    if (ht == null)
                        return null;

                    WxLoginScanResult result = WxRespUtil.ParseLoginScanResp(ht.ExecuteString());
#if Debug
                    Console.WriteLine("wx login scan result =" + (result == null ? "null" : result.Code.ToString()));
#endif
                    if (result == null)
                    {
                    }
                    else if (result.Code == WxResultCode.LOGIN_SCAN_SUCCESS)//201
                    {
                        //已扫描，未登录，这里可以获取头像
                        tip = "0";
                    }
                    else if (result.Code == WxResultCode.LOGIN_SCAN_LOGIN)//200
                    {
                        //点击登录
                        return result;
                    }
                    else if (result.Code == WxResultCode.LOGIN_SCAN_TIMEOUT)//408
                    {
                        //微信端返的连接超时，继续连接
                    }
                    else if (result.Code == WxResultCode.LOGIN_SCAN_EXPIRED)//400
                    {
                        //二维码过期
                        return result;
                    }

                    ThreadUtil.Sleep(500);

                    if (!syncCheckLoginingState())
                        return null;
                }
            }
            catch { }

            return null;
        }

        public WxLoginIdsResult executeLoginRedirectUriHttpTask(string redirectUri)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetLoginRedirectUriParams(redirectUri));
                if (ht != null)
                {
                    WxLoginIdsResult result = WxRespUtil.ParseLoginRedirectUriResp(ht.ExecuteString());
                    result.RouteHost = WxRespUtil.ParseHost(redirectUri);
                    result.DataTicket = ht.GetCookie(string.Format("http://{0}", result.RouteHost), "webwx_data_ticket");
                    return result;
                }
            }
            catch { }

            return null;
        }

        private WxLoginInitResult executeLoginInitHttpTask(string host, WxLoginIdsResult loginIdsResult, string deviceId)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetLoginInitParams(host, loginIdsResult, deviceId));
                if (ht != null)
                {
                    return ht.ExecuteJson<WxLoginInitResult>();
                }
            }
            catch { }

            return null;
        }

        private void executeStatusNotifyHttpTask(string host, WxAccount wxAccount)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetStatusNotify(host, wxAccount));
                if (ht != null)
                {
                    WxSendMsgResult msgResult = ht.ExecuteJson<WxSendMsgResult>();
#if Debug
                    if (msgResult == null)
                        Console.WriteLine("status notify result = null");
                    else
                        Console.WriteLine("status notify result ret code = " + msgResult.BaseResponse.Ret + ", msg id=" + msgResult.MsgID);
#endif
                }
            }
            catch { }
        }

        private WxContactAllResult executeGetContactAllHttpTask(string host, WxAccount wxAccount)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetContactAllParams(host, wxAccount));
                return ht.ExecuteJson<WxContactAllResult>();
            }
            catch { }

            return null;
        }

        private WxSyncCheckResult executeSyncCheckHttpTask(string host, WxAccount wxAccount, WxSyncKey wxSyncKey)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetSyncCheckParams(host, wxAccount, wxSyncKey));
                if (ht != null)
                {
                    WxSyncCheckResult result = WxRespUtil.ParseWxSyncKeyResult(ht.ExecuteString());
                    return result;
                }
            }
            catch { }

            return null;
        }

        private WxSyncMsgResult executeSyncMsgHttpTask(string host, WxAccount wxAccount, WxSyncKey wxSyncKey)
        {
            try
            {
                HttpTask ht = syncCreateHttpTask(WxHtpUtil.GetSyncMsgParams(host, wxAccount, wxSyncKey));
                if (ht != null)
                {
                    WxSyncMsgResult result = ht.ExecuteJson<WxSyncMsgResult>();
                    return result;
                }
            }
            catch { }

            return null;
        }

        private string executeLogout(string host, WxAccount wxAccount)
        {
            if (wxAccount == null)
                return null;
            else
                return executeLogout(host, wxAccount.Skey, wxAccount.Uin, wxAccount.Sid);
        }

        private string executeLogout(string host, string skey, string uin, string sid)
        {
            try
            {
                HttpTask ht = new HttpTask(WxHtpUtil.GetLogout(host, skey, uin, sid));
                ht.SetCustomCookieContainer(mCookieContainer);
                return ht.ExecuteString();
            }
            catch { }

            return null;
        }

        private void callbackLoginFailed(WxLoginCode code)
        {
            OnWxTaskLoginFailedHandler?.Invoke(code);
        }

        /// <summary> 同步创建网络任务 </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private HttpTask syncCreateHttpTask(HttpTaskParams param)
        {
            lock (mLockObj)
            {
                if (mTaskThread == null)
                {
                    return null;
                }
                else
                {
                    mCurrentHttpTask = new HttpTask(param);
                    mCurrentHttpTask.SetCustomCookieContainer(mCookieContainer);
                    return mCurrentHttpTask;
                }
            }
        }


        /// <summary> 执行登录步骤 </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        //private WxLoginScanResult executeLoginScan(string deviceId)
        //{
        //    //获取登录uid
        //    WxLoginUidResult loginUidResult = executeLoginUidHttpTask();
        //    if (!syncCheckLoginingState() || !WxRespUtil.CheckWxLoginUidResult(loginUidResult))
        //        return null;

        //    //获取微信登录二维码
        //    byte[] qrCodeBytes = executeLoginQrCodeHttpTask(loginUidResult.Uid);
        //    if (!syncCheckLoginingState() || qrCodeBytes == null)
        //        return null;

        //    //二维码回调
        //    OnWxTaskLoginQrCodeHandler?.Invoke(qrCodeBytes);

        //    //登录二维码扫描
        //    return executeLoginScanHttpTask(loginUidResult.Uid);
        //}
    }
}