/* 迹I柳燕
 *
 * FileName:   UrlHelper.cs
 * Version:    1.0
 * Date:       2017/9/19 14:24:05
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Data
 * @class      UrlHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Text;

namespace Ji.CommonHelper.Data
{
    /// <summary></summary>
    public static class UrlHelper
    {
        #region Fields

        private const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        #endregion Fields

        #region Methods

        /// <summary> Url 转义 默认使用Utf8编码 等同于HttpUtility.UrlEncode </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UrlEncode(string obj)
        {
            return UrlEncode(obj, Encoding.UTF8);
        }

        /// <summary> Url 转义 等同于HttpUtility.UrlEncode </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UrlEncode(string obj, Encoding encoding)
        {
            if (!string.IsNullOrWhiteSpace(obj))
            {
                var sb = new StringBuilder();
                foreach (char item in obj)
                {
                    if (unreservedChars.IndexOf(item) != -1)
                    {
                        sb.Append(item);
                    }
                    else
                    {
                        byte[] byStr = encoding.GetBytes(new char[] { item });
                        for (int i = 0; i < byStr.Length; i++)
                        {
                            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
                        }
                    }
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        #endregion Methods
    }
}