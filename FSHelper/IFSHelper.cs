using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.FSHelper
{
    public interface IFSHelper
    {
        byte[] DownloadFileBytes(string serverurl, string skey);

        string DownloadFile(string serverurl, string skey);

        string UploadFile(string serverurl, string sfilename);

        string UploadFile(string serverurl, byte[] content);

        bool DeleteFile(string serverurl, string skey);

        string ServerUrl { get; set; }
        string Sqbm { get; set; }
        string ClientModel { get; set; }
    }
}