/* 迹I柳燕
 *
 * FileName:   SortHelper.cs
 * Version:    1.0
 * Date:       2017/8/25 18:02:43
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.MathHelper
 * @class      SortHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Ji.CommonHelper.MathHelper
{
    /// <summary></summary>

    public sealed class NaturalStringComparer : IComparer<string>
    {
        private readonly int modifier = 1;

        public NaturalStringComparer(bool descending)
        {
            if (descending)
            {
                modifier = -1;
            }
        }

        public NaturalStringComparer() : this(false)
        {
        }

        public int Compare(string a, string b)
        {
            return SafeNativeMethods.StrCmpLogicalW(a ?? "", b ?? "") * modifier;
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
}