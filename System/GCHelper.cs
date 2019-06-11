/* 迹I柳燕
 *
 * FileName:   GCHelper.cs
 * Version:    1.0
 * Date:       2017/3/1 17:47:00
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.System
 * @class      GCHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.CommonHelper.System
{
    /// <summary>  </summary>
    public static class GCHelper
    {
        public static void FlushMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
            catch { }
        }

        /// <summary>
        /// set process working size
        /// </summary>
        /// <param name="process">Gets process</param>
        /// <param name="minimumWorkingSetSize">Gets minimum working size</param>
        /// <param name="maximumWorkingSetSize">Gets maximum working size</param>
        /// <returns>Returns value</returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
    }
}