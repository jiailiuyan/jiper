using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.System
{
    class AutoItXHelper
    {
    } /// <summary>  </summary>
    //public static class WindowAPIUtils
    //{
    //    public static IntPtr GetOpenFileDialog()
    //    {
    //        var wt = "[Title:打开; CLASS:#32770]";
    //        var exsit = AutoItX.WinWait(wt, "", 1) == 1;
    //        if (exsit)
    //        {
    //            var handle = AutoItX.WinGetHandle(wt);
    //            return handle;
    //        }
    //        return IntPtr.Zero;
    //    }

    //    /// <summary>
    //    /// 设置文件选择窗口选择内容并确认
    //    /// </summary>
    //    /// <param name="filepath"> 文件绝对路径 其中[\]需要用[\\]</param>
    //    /// <returns></returns>
    //    public static bool SetFileSelecter(string filepath)
    //    {
    //        var wh = GetOpenFileDialog();
    //        if (wh != IntPtr.Zero)
    //        {
    //            var active = AutoItX.WinActivate(wh) == 1;
    //            if (active)
    //            {
    //                var ct = "[CLASS:Edit; INSTANCE:1]";
    //                var ch = AutoItX.ControlGetHandle(wh, ct);
    //                var bt = "[CLASS:Button; INSTANCE:1]";
    //                var bh = AutoItX.ControlGetHandle(wh, bt);
    //                if (ch != IntPtr.Zero && bh != IntPtr.Zero)
    //                {
    //                    AutoItX.Sleep(50);
    //                    AutoItX.ControlSetText(wh, ch, filepath);
    //                    AutoItX.Sleep(50);
    //                    AutoItX.ControlClick(wh, bh);
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }
    //}
}
