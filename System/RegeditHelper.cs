/* 迹I柳燕
 *
 * FileName:   RegeditHelper.cs
 * Version:    1.0
 * Date:       2018-12-14 10:41:55
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.SystemHelper
 * @class      RegeditHelper
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ji.SystemHelper
{
    /// <summary>  </summary>
    public class RegeditHelper
    {

        public static string DoRegedit(string cmd, string title = "")
        {
            // 此处只是取一个不同的名称而已，因此生成算法无所谓
            var filemd5 = EncryptWithMD5(cmd);

            var cachepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            cachepath = System.IO.Path.Combine(cachepath, @"Kingsoft\HaaS\Regs");
            if (!System.IO.Directory.Exists(cachepath))
            {
                System.IO.Directory.CreateDirectory(cachepath);
            }
            var regfilepath = cachepath + $"\\{title}_{filemd5}.reg";
            System.IO.File.WriteAllText(regfilepath, cmd);

            var proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine($"regedit.exe /s \"{regfilepath}\"");
            proc.StandardInput.WriteLine("exit");
            var outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();

            Debug.WriteLine("");
            Debug.WriteLine(outStr);
            Debug.WriteLine("");

            return outStr;
        }

        private static string EncryptWithMD5(string source)
        {
            var sor = Encoding.UTF8.GetBytes(source);
            var md5 = MD5.Create();
            var result = md5.ComputeHash(sor);
            var strbul = new StringBuilder(40);
            for (var i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));
            }
            return strbul.ToString();
        }

    }
}
