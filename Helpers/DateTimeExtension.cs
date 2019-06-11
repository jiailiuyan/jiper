using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static  class DateTimeExtension
    {
        //dt.ToShortDateString  ‎yyyy-MM-dd
        //dt.ToLongDateString ‎yyyy-MM-dd
        //dt.ToLongTimeString HH:mm:ss
        //dt.ToShortTimeString HH:mm

        /// <summary>
        /// 不允许使用 DateTime.ToLongDateString()
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 不允许使用 DateTime.ToLongTimeString()
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongTime(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 不允许使用 DateTime.ToShortDateString()
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 不允许使用 DateTime.ToShortTimeString()
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortTime(this DateTime dt)
        {
            return dt.ToString("HH:mm");
        }
    }
}
