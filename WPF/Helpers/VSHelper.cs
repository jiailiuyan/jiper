/* 迹I柳燕
 *
 * FileName:   VSHelper.cs
 * Version:    1.0
 * Date:       2017/2/21 11:27:41
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Helpers
 * @class      VSHelper
 * @extends
 *
 *========================================
 *
 */

using System.ComponentModel;
using System.Diagnostics;

namespace Ji.CommonHelper.WPF.Helpers
{
    /// <summary></summary>
    public static class VSHelper
    {
        public static bool IsDesingMode
        {
            get
            {
                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

        public static bool IsVSMode
        {
            get { return Process.GetCurrentProcess().ProcessName == "devenv"; }
        }
    }
}