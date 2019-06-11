/* 迹I柳燕
 *
 * FileName:   ProcessHelper.cs
 * Version:    1.0
 * Date:       2017/7/19 12:14:19
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.System
 * @class      ProcessHelper
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
    /// <summary></summary>
    public class ProcessHelper
    {
        public static void KillSelf()
        {
            var tt = Process.GetProcessById(Process.GetCurrentProcess().Id);
            tt.Kill();
        }

        public static void KillByPid(int procid, bool forceclose = false)
        {
            try
            {
                var process = Process.GetProcesses().FirstOrDefault(i => i.Id == procid);
                if (process != null)
                {
                    if (forceclose && !process.CloseMainWindow())
                    {
                        process.Kill();
                    }
                    else
                    {
                        process.Kill();
                    }
                }
            }
            catch { }
        }

        public static void KillByPid(string procid)
        {
            int pid = -1;
            if (!string.IsNullOrWhiteSpace(procid) && int.TryParse(procid, out pid))
            {
            }
        }
    }
}