using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.System
{
    class DisposeHelper
    {
        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        //SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    public class FlushMemory
    {
        [DllImport("kernel32.dll")]

        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Flush()
        {

            GC.Collect();

            GC.WaitForPendingFinalizers();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }

        }

    }
}
