using JZYD.TY.Common.Http.Tasker.Params;
using JZYD.TY.Common.Util;
using JZYD.TY.WeChat.Http.Model.Req;
using JZYD.TY.WeChat.Http.Model.Result;
using JZYD.TY.WeChat.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JZYD.TY.WeChat.Http
{
    /// <summary>
    /// 微信htp工具类
    /// </summary>
    public class WxHtpUtil
    {
        
        /// <summary>
        /// 获取登录uid相关请求参数
        /// </summary>
        /// <returns></returns>
        /// url->https://login.wx.qq.com/jslogin
        /// ?appid=wx782c26e4c19acffb
        /// &redirect_uri=https%3A%2F%2Fwx.qq.com%2Fcgi-bin%2Fmmwebwx-bin%2Fwebwxnewloginpage
        /// &fun=new
        /// &lang=zh_CN
        /// &_=1499308667244
        public static HttpTaskParams GetLoginUidParams()
        {
            HttpTaskParams htp = HttpTaskParams.NewGet(WxHttpApi.GetLoginUrl(WxHttpApi.URL_LOGIN_UID));
            htp.AddStringParam("appid", "wx782c26e4c19acffb");
            htp.AddStringParam("fun", "new");
            htp.AddStringParam("lang", "zh_CN");
            htp.AddStringParam("_", TimeUtil.CurrentTimeMillis().ToString());
            htp.AddStringParam("redirect_uri", WxHttpApi.URL_WX_JS_LOGIN_REDIRECT_URI);
            return htp;
        }

        /// <summary>
        /// 获取二维码图片请求
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// url->https://login.weixin.qq.com/qrcode/Ifv-J92fbw==
        public static HttpTaskParams GetLoginQrCodeParams(string uid)
        {
            string url = string.Format("{0}/{1}", WxHttpApi.GetLoginUrl(WxHttpApi.URL_LOGIN_QRCODE), uid);//uid不能encoding
            return HttpTaskParams.NewGet(url);
        }

        /// <summary>
        /// 获取二维码扫描结果请求
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        /// url->https://login.wx.qq.com/cgi-bin/mmwebwx-bin/login
        /// ?loginicon=true
        /// &uuid=Ifv-J92fbw==
        /// &tip=0&
        /// r=-366287686&
        /// _=1499309823465
        public static HttpTaskParams GetLoginScanParams(string uid, string tip)
        {
            HttpTaskParams htp = HttpTaskParams.NewGet(WxHttpApi.GetLoginUrl(WxHttpApi.URL_LOGIN_QRCODE_SCAN_CHECK));
            htp.AddStringParam("uuid", uid);
            htp.AddStringParam("tip", tip);
            htp.AddStringParam("loginicon", "true");
            long millis = TimeUtil.CurrentTimeMillis();
            htp.AddStringParam("r", ((millis / 1000)).ToString());//时间戳取反
            htp.AddStringParam("_", millis.ToString());
            return htp;
        }

        /// <summary>
        /// 获取登录跳转链接
        /// </summary>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        /// url->https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxnewloginpage
        /// ?ticket=AV0lBiU3dZOD5hBV9bpyx9b9@qrticket_0
        /// &uuid=4dUx4jLLIA==
        /// &lang=zh_CN
        /// &scan=1499308941";
        public static HttpTaskParams GetLoginRedirectUriParams(string redirectUri)
        {
            string url = string.Format("{0}&fun={1}&version={2}&lang={3}", redirectUri, "new", "v2", "zh_CN");
            if (WxHttpApi.IsHttpScheme())
                url = Regex.Replace(url, WxHttpApi.URL_SCHEME_HTTPS, WxHttpApi.URL_SCHEME_HTTP);

            return HttpTaskParams.NewGet(url);
        }

        /// <summary>
        /// 获取微信登录初始化接口
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// demo：https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit
        /// ?r=665841145
        /// &lang=zh_CN
        /// &pass_ticket=zXbwZhRifdRRNcPjsjxyemYskxzSRQOxZF6T3HJuTpsdgPENMSMXylIlmsnrEZ5s
        public static HttpTaskParams GetLoginInitParams(string host, WxLoginIdsResult result, string deviceId)
        {
            //r时间戳取反
            string url = string.Format("{0}?r={1}&lang={2}&pass_ticket={3}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_INIT), 
                (TimeUtil.CurrentTimeMillis()/1000), 
                "zh_CN",
                encoding(result.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=utf-8";
            htp.CustomStringContentBody = JsonConvert.SerializeObject(WxReqUtil.GetBaseReq(result, deviceId));
#if Debug
            Console.WriteLine("init content body = "+ htp.CustomStringContentBody);
#endif
            return htp;
        }

        /// <summary>
        /// 获取微信状态通知
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxstatusnotify
        /// ?lang=zh_CN
        /// &pass_ticket=zXbwZhRifdRRNcPjsjxyemYskxzSRQOxZF6T3HJuTpsdgPENMSMXylIlmsnrEZ5s
        public static HttpTaskParams GetStatusNotify(string host, WxAccount wxAccount)
        {
            string url = string.Format("{0}?lang={1}&pass_ticket={2}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_STATUS_NOTIFY), 
                "zh_CN",
                encoding(wxAccount.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=utf-8";
            htp.CustomStringContentBody = JsonConvert.SerializeObject(WxReqUtil.GetStatusNotifyReq(wxAccount));
            return htp;
        }

        /// <summary>
        /// 获取同步检查参数
        /// </summary>
        /// <param name="account"></param>
        /// <param name="deviceId"></param>
        /// <param name="wxSyncKey"></param>
        /// <returns></returns>
        /// https://webpush.wx2.qq.com/cgi-bin/mmwebwx-bin/synccheck
        /// ?r=1501480039641
        /// &skey=%40crypt_77e833b9_e705acd2a3250d78bdbb1137d87afad5&sid=TlG7spFxhWig3TpY
        /// &uin=243082945
        /// &deviceid=e835949820036438
        /// &synckey=1_653730215%7C2_653730255%7C3_653730169%7C1000_1501462441
        /// &_=1501479916949
        public static HttpTaskParams GetSyncCheckParams(string host, WxAccount wxAccount, WxSyncKey wxSyncKey)
        {
            HttpTaskParams htp = HttpTaskParams.NewGet(WxHttpApi.GetWebPushUrl(host, WxHttpApi.URL_WX_MSG_CHECK));
            htp.AddStringParam("skey", wxAccount.Skey);
            htp.AddStringParam("uin", wxAccount.Uin);
            htp.AddStringParam("sid", wxAccount.Sid);
            htp.AddStringParam("deviceid", wxAccount.DeviceId);
            htp.AddStringParam("synckey", WxReqUtil.GetSyncKey2String(wxSyncKey));
            long millis = TimeUtil.CurrentTimeMillis();
            htp.AddStringParam("r", millis.ToString());
            htp.AddStringParam("_", (millis/1000).ToString());//时间戳取反 
            return htp;
        }

        /// <summary>
        /// 同步获取更新消息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="syncKey"></param>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsync
        /// ?sid=XbUcBWzGt6NPu5jO
        /// &skey=@crypt_55085584_cf738edf865a9c101021022d1c732ae6
        /// &lang=zh_CN
        /// &pass_ticket=lBIvUtbeb5e%252BMIDHLr5QVehuJDPbW3NEgMSh87ILAOv3ue1HdLtSV%252B8AK0ULznEL
        public static HttpTaskParams GetSyncMsgParams(string host, WxAccount account, WxSyncKey syncKey)
        {
            string url = string.Format("{0}?sid={1}&skey={2}&pass_ticket={3}&lang={4}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_MSG_PULL),
                encoding(account.Sid),
                encoding(account.Skey),
                encoding(account.PassTicket), 
                "zh_CN");

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxWebSyncReq req = new WxWebSyncReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.SyncKey = syncKey;
            req.rr = (~(TimeUtil.CurrentTimeMillis() / 1000)).ToString();//时间戳取反
            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 获取微信所有联系人
        /// </summary>
        /// <param name="wxAccount"></param>
        /// <returns></returns>
        /// https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact
        /// ?lang=zh_CN
        /// &pass_ticket=9%252FuIrFpoM6CCrYsAV7mzDF0aFkHiXGlotDlmHbQokhM%253D
        /// &r=1501480039623
        /// &seq=0
        /// &skey=@crypt_77e833b9_e705acd2a3250d78bdbb1137d87afad5
        public static HttpTaskParams GetContactAllParams(string host, WxAccount wxAccount)
        {
            string url = string.Format("{0}?lang=zh_CN&pass_ticket={1}&r={2}&seq=0&skey={3}",
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_CONTACT_ALL),
                encoding(wxAccount.PassTicket), 
                TimeUtil.CurrentTimeMillis(),
                encoding(wxAccount.Skey));

            return HttpTaskParams.NewGet(url);
        }

        /// <summary>
        /// 批量获取联系人,包含个人和组
        /// </summary>
        /// <param name="wxAccount"></param>
        /// <param name="groupUserNames"></param>
        /// <returns></returns>
        /// //https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxbatchgetcontact
        /// ?type=ex
        /// &r=1498291861743
        /// &lang=zh_CN
        /// &pass_ticket=ga8bxGb9YQJLWhU6RA6FMamJy0wqnD7QuWP9k%252F9tlTb8O1uUkgcZ7YbjNg3bEkYH
        public static HttpTaskParams GetBatchContactParams(string host, WxAccount wxAccount, List<string> groupUserNameIds)
        {
            string url = string.Format("{0}?type=ex&r={1}&lang=zh_CN&pass_ticket={2}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_CONTACT_BATCH), 
                TimeUtil.CurrentTimeMillis(),
                encoding(wxAccount.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=utf-8";

            WxBatchContactReq req = new WxBatchContactReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(wxAccount);
            req.Count = CollectionUtil.Size(groupUserNameIds);
            req.List = WxReqUtil.GetBatchContactItemList(groupUserNameIds);
            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 群加人
        /// </summary>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxupdatechatroom
        /// ?fun=addmember
        /// &lang=zh_CN
        /// &pass_ticket=IcRBsfjC16rssd%252Fd2Fp50Znh%252FuykKjmO7zl61fim6SSnxqLdRXXS%252FlqDrVs%252BpfCG
        public static HttpTaskParams GetGroupContactAddMember(string host, WxAccount account, string userNameId, string groupUserNameId)
        {
            string url = string.Format("{0}?fun=addmember&lang=zh_CN&pass_ticket={1}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_EX_GROUP_ADD_MEMBER), 
                encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxGroupMemberReq req = new WxGroupMemberReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.AddMemberList = userNameId;
            req.ChatRoomName = groupUserNameId;
            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 群名片方式邀请入群
        /// </summary>
        /// <param name="host"></param>
        /// <param name="account"></param>
        /// <param name="userNameId"></param>
        /// <param name="groupUserNameId"></param>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxupdatechatroom
        /// ?fun=invitemember
        /// &lang=zh_CN
        /// &pass_ticket=i4nvgEN0wMPH%252B%252FvRDYsdcNS%252FQyGjlV4yB9NDJAm6tnP5aaHWBjNYtCvWnObgo1k4
        public static HttpTaskParams GetGroupContactInviteMember(string host, WxAccount account, string inviteUserNameId, string groupUserNameId)
        {
            string url = string.Format("{0}?fun=invitemember&lang=zh_CN&pass_ticket={1}",
            WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_EX_GROUP_ADD_MEMBER),
            encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxGroupMemberInviteReq req = new WxGroupMemberInviteReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.InviteMemberList = inviteUserNameId;
            req.ChatRoomName = groupUserNameId;
            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 群踢人
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userNameId"></param>
        /// <param name="groupUserNameId"></param>
        /// <returns></returns>
        public static HttpTaskParams GetGroupContactDelMember(string host, WxAccount account, string userNameId, string groupUserNameId)
        {
            string url = string.Format("{0}?fun=delmember&lang=zh_CN&pass_ticket={1}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_GROUP_DEL_MEMBER), 
                encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxGroupMemberReq req = new WxGroupMemberReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.DelMemberList = userNameId;
            req.ChatRoomName = groupUserNameId;
            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxlogout?redirect=1&type=1&skey=%40crypt_55085584_86ecfa5dcc8c6c64484e0014d33882de
        public static HttpTaskParams GetLogout(string host, string skey, string uin, string sid)
        {
            string url = string.Format("{0}?redirect={1}type={2}&skey={3}&uin={4}&sid={5}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_LOGOUT), 
                "1",
                "1",
                encoding(skey),
                encoding(uin),
                encoding(sid));

            return HttpTaskParams.NewPost(url);
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="toUserNameId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg
        /// ?lang=zh_CN
        /// &pass_ticket=4eHHuCPjQTzJL8d25%252FYZlo%252BzC%252FtbrmnGPwyNWPJTS8pqc%252FDZ%252FhdLzBJISu6rUn4E
        public static HttpTaskParams GetSendTextMsg(string host, WxAccount account, string toUserNameId, string text)
        {
            string url = string.Format("{0}?fun=async&lang=zh_CN&pass_ticket={1}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_SEND_MSG), 
                encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxSendMsgReq req = new WxSendMsgReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.Scene = 0;

            WxSendMsg msg = new WxSendMsg();
            msg.Type = 1;
            msg.Content = text;
            msg.FromUserName = account.UserName;
            msg.ToUserName = toUserNameId;
            string msgId = TimeUtil.CurrentTimeMillis() + TextUtil.RandomNumber(4);
            msg.LocalID = msgId;
            msg.ClientMsgId = msgId;
            req.Msg = msg;

            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 微信上传图片
        /// </summary>
        /// <param name="account"></param>
        /// <param name="toUserNameId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        private static int fileCount = 0;
        public static HttpTaskParams GetUploadImage(string host, WxAccount account, string toUserNameId, string imagePath)
        {
            string url = string.Format("{0}?f=json", WxHttpApi.GetFileUploadUrl(host, WxHttpApi.URL_WX_FILE_UPLOAD));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            FileInfo info = new FileInfo(imagePath);
            htp.AddStringParam("id", "WU_FILE_"+ fileCount);
            htp.AddStringParam("name", info.Name);
            htp.AddStringParam("type", "image/jpeg");
            htp.AddStringParam("lastModifiedDate", info.LastWriteTime.ToString("r", DateTimeFormatInfo.InvariantInfo));
            htp.AddStringParam("size", info.Length.ToString());
            htp.AddStringParam("mediatype", "pic");
            htp.AddStringParam("webwx_data_ticket", account.DataTicket);
            htp.AddStringParam("pass_ticket", account.PassTicket);
            htp.AddFileParam("filename", imagePath, WxReqUtil.GetImageMimeType(info));
            htp.AddStringParam("uploadmediarequest", WxReqUtil.GetUploadMediaRequestBody(account, toUserNameId, info, imagePath));
            return htp;
        }

        /// <summary>
        /// 微信发送图片消息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="toUserNameId"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        ///https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsgimg
        ///?lang=zh_CN
        ///&f=sjon&&pass_ticket=4eHHuCPjQTzJL8d25%252FYZlo%252BzC%252FtbrmnGPwyNWPJTS8pqc%252FDZ%252FhdLzBJISu6rUn4E
        public static HttpTaskParams GetSendImageMsg(string host, WxAccount account, string toUserNameId, string mediaId)
        {
            string url = string.Format("{0}?fun=async&f=json&lang=zh_CN&pass_ticket={1}", 
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_SEND_IMAGE_MSG), 
                encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxSendMsgReq req = new WxSendMsgReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.Scene = 0;

            WxSendMediaMsg msg = new WxSendMediaMsg();
            msg.Type = 3;//图片
            msg.MediaId = mediaId;
            msg.FromUserName = account.UserName;
            msg.ToUserName = toUserNameId;
            string msgId = TimeUtil.CurrentTimeMillis() + TextUtil.RandomNumber(4);
            msg.LocalID = msgId;
            msg.ClientMsgId = msgId;
            msg.Content = string.Empty;

            req.Msg = msg;

            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        /// <summary>
        /// 上传视频文件
        /// </summary>
        /// <param name="host"></param>
        /// <param name="account"></param>
        /// <param name="toUserNameId"></param>
        /// <param name="videoPath"></param>
        /// <returns></returns>
        /// https://file.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json
        public static HttpTaskParams GetUploadVideo(string host, WxAccount account, string toUserNameId, string videoPath)
        {
            string url = string.Format("{0}?f=json", WxHttpApi.GetFileUploadUrl(host, WxHttpApi.URL_WX_FILE_UPLOAD));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            FileInfo info = new FileInfo(videoPath);
            htp.AddStringParam("id", "WU_FILE_" + fileCount);
            htp.AddStringParam("name", info.Name);
            htp.AddStringParam("type", WxReqUtil.GetVideoMimeType(info));//"video/mp4"
            htp.AddStringParam("lastModifiedDate", info.LastWriteTime.ToString("r", DateTimeFormatInfo.InvariantInfo));
            htp.AddStringParam("size", info.Length.ToString());
            htp.AddStringParam("chunks", "1");//分段数,这里一次上传，不分段
            htp.AddStringParam("chunk", "0");//第几段
            htp.AddStringParam("mediatype", "video"); 
            htp.AddStringParam("uploadmediarequest", WxReqUtil.GetUploadMediaRequestBody(account, toUserNameId, info, videoPath));
            htp.AddStringParam("webwx_data_ticket", account.DataTicket);
            htp.AddStringParam("pass_ticket", account.PassTicket);
            htp.AddFileParam("filename", videoPath);

            return htp;
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="host"></param>
        /// <param name="account"></param>
        /// <param name="toUserNameId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendvideomsg?fun=async&f=json&pass_ticket=%252FX8NCpFJfNmU%252Be%252FgE2dwRCN2D8Y8EBSKunfckAJmPfCeVD4YaubkaS2e767qiyAO
        /// https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendvideomsg?fun=async&f=json&lang=zh_CN&pass_ticket=KwMEOlgC57Dc1AwBwr7oaHekXX0P8fJE%252Bl0cLVocIrxdWNr%252F9URPGCAEmf%252FzfjEK
        public static HttpTaskParams GetSendVideoMsg(string host, WxAccount account, string toUserNameId, string mediaId)
        {
            string url = string.Format("{0}?fun=async&f=json&lang=zh_CN&pass_ticket={1}",
                WxHttpApi.GetCommonUrl(host, WxHttpApi.URL_WX_SEND_VIDEO_MSG),
                encoding(account.PassTicket));

            HttpTaskParams htp = HttpTaskParams.NewPost(url);
            htp.ContentType = "application/json;charset=UTF-8";

            WxSendMsgReq req = new WxSendMsgReq();
            req.BaseRequest = WxReqUtil.GetIdentifyReq(account);
            req.Scene = 0;

            WxSendMediaMsg msg = new WxSendMediaMsg();
            msg.Type = 43;//视频
            msg.MediaId = mediaId;
            msg.FromUserName = account.UserName;
            msg.ToUserName = toUserNameId;
            string msgId = TimeUtil.CurrentTimeMillis() + TextUtil.RandomNumber(4);
            msg.LocalID = msgId;
            msg.ClientMsgId = msgId;
            msg.Content = string.Empty;

            req.Msg = msg;

            htp.CustomStringContentBody = JsonConvert.SerializeObject(req);
            return htp;
        }

        private static string encoding(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            else
                return Uri.EscapeDataString(content);
        }
    }
}
