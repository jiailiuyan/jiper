/* 迹I柳燕
 *
 * FileName:   WindowHelper.cs
 * Version:    1.0
 * Date:       2017/9/11 10:17:15
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Helpers
 * @class      WindowHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;

namespace Ji.CommonHelper.WPF.Helpers
{
    /// <summary></summary>
    public static class WindowHelper
    {
        #region Fields

        private static Dictionary<int, IntPtr> Handles = new Dictionary<int, IntPtr>();

        #endregion Fields

        #region Methods

        public static IntPtr GetHandle(this Window window)
        {
            if (window != null)
            {
                var hashcode = window.GetHashCode();
                if (!Handles.ContainsKey(hashcode))
                {
                    Handles.Add(hashcode, new WindowInteropHelper(window).Handle);
                }
                return Handles[hashcode];
            }
            return IntPtr.Zero;
        }

        #endregion Methods
    }
}