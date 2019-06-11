/* 迹I柳燕
 *
 * FileName:   CMDHelper.cs
 * Version:    1.0
 * Date:       2017/2/23 10:28:53
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.System
 * @class      CMDHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.System
{
    /// <summary>  </summary>
    public class CMDHelper
    {
        public static string StartCmd(string cmd)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(cmd);
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}