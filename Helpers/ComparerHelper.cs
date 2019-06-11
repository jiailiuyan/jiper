/* 迹I柳燕
 *
 * FileName:   CompareHelper.cs
 * Version:    1.0
 * Date:       2018/10/20 18:14:21
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Helpers
 * @class      CompareHelper
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Ji.DataHelper;

namespace Ji.CommonHelper.Helpers
{
    /// <summary>  </summary>
    public class ComparerHelper
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        public static readonly IComparer<string> NSComparer = new NaturalStringComparer();

        public static readonly IComparer<string> NSRealStrComparer = new NaturalRealStringComparer();

        public class NaturalStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return StrCmpLogicalW(x, y);
            }
        }

        ///// <summary>
        ///// 预期排除的特殊符号
        ///// 此排除只排除 Start
        ///// </summary>
        //public static readonly List<char> ExceptChars = new List<char>()
        //{
        //   ' ','*','△',' ','*',' '
        //};

        public class NaturalRealStringComparer : IComparer<string>
        {

            public int Compare(string x, string y)
            {
                var ex = StringHelper.MatchExceptStart(x);
                var ey = StringHelper.MatchExceptStart(y);
                if (!string.IsNullOrWhiteSpace(x) && ex == ey)
                {
                    return x.Length - y.Length;
                }
                return StrCmpLogicalW(ex, ey);
            }
        }

    }
}
