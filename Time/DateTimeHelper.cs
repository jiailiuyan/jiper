/* 迹I柳燕
 *
 * FileName:   DateTimeHelper.cs
 * Version:    1.0
 * Date:       2018-11-30 17:10:16
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Time
 * @class      DateTimeHelper
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>  </summary>
    public static class J_DateTimeHelper
    {
        public static DateTime Parse(string datestr)
        {
            string[] formats = {"yyyyMM" ,"yyyy/MM","yyyy-MM","yyyy MM",
                "yyyyMMdd" ,"yyyy/MM/dd","yyyy-MM-dd","yyyy MM dd",
                "yyyyMdd" ,"yyyy/M/dd","yyyy-M-dd","yyyy M dd",
                "yyyyMMddHHmmss" ,"yyyy/MM/dd HH:mm:ss","yyyy-MM-dd HH:mm:ss","yyyy MM dd HH mm ss",
            };
            return DateTime.ParseExact(datestr, formats, new CultureInfo("en-US"), DateTimeStyles.None);
        }
        public static string GetTimeFloderPath(this DateTime time, string endstr = "")
        {
            return $"{time.ToString("yyyy")}/{time.ToString("MM")}/{time.ToString("dd")}{endstr}";
        }
    }
}
