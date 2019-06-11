/* 迹I柳燕
 *
 * FileName:   StringHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  JilyHelper
 * @class      StringHelper
 * @extends
 *
 *             对于String的扩展方法
 *
 *========================================

 * (http://www.jiailiuyan.com)
 *
 *
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ji.DataHelper
{
    public static class StringHelper
    {
        private static readonly Regex reg = new Regex("^[\u4e00-\u9fa5a-zA-Z0-9]+$");
        public static string MatchExceptStart(this string value)
        {
            var start = 0;
            var end = value?.Length;
            while (end > 0 && end > start)
            {
                var m = reg.Match(value, start, 1);
                if (m.Success)
                {
                    var rs = new string(value.Skip(start).Take(end.Value - start).ToArray());
                    return rs;
                }
                start++;
            }
            return value;
        }


        public static string ToString(object obj)
        {
            return obj + "";
        }

        public static string ToSFormate(decimal? obj, string formate = "0.0000")
        {
            if (obj != null)
            {
                return ((decimal)obj).ToString(formate);
            }
            return 0.ToString(formate);
        }

        #region 字符编码转换

        /// <summary> string 转换到 byte[] </summary>
        /// <param name="body"> 将要转换的 string </param>
        /// <returns> 返回转换后的 byte[] </returns>
        public static byte[] ConvertToASCII(this string str)
        {
            return new ASCIIEncoding().GetBytes(str);
        }

        public static string ConvertToUTF8(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var retstr = string.Empty;
            buffer.ToList().ForEach(i => retstr += string.Format("%{0:X}", i));
            return retstr;
        }

        public static string ConvertToGB2312(this string str)
        {
            var buffer = Encoding.GetEncoding("GB2312").GetBytes(str);
            var retstr = string.Empty;
            buffer.ToList().ForEach(i => retstr += string.Format("%{0:X}", i));
            return retstr;
        }

        #endregion 字符编码转换

        public static string EncodeBase64(this string str)
        {
            return EncodeBase64(str, Encoding.Unicode);
        }

        public static string EncodeBase64(this string str, Encoding code)
        {
            var encode = "";
            var bytes = code.GetBytes(str);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = str;
            }
            return encode;
        }

        /// <summary> 全角转半角 </summary>
        public const string DoubleByte = "ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ～！＠＃＄％︿＆＊（）＿－＋｜＼｛｝［］：＂；＇＜＞，．？／０１２３４５６７８９";

        public const string SingleByte = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()_-+|\\{}[]:\";'<>,.?/0123456789";

        public static char ConvertToSingleByte(this char c)
        {
            var index = DoubleByte.IndexOf(c);
            return index != -1 ? SingleByte[c] : c;
        }

        public static char ConvertToDoubleByte(this char c)
        {
            var index = SingleByte.IndexOf(c);
            return index != -1 ? DoubleByte[c] : c;
        }

        public static string ConvertToSingleByte(this string str)
        {
            var sb = new StringBuilder();
            str.ToList().ForEach(c => sb.Append(ConvertToSingleByte(c)));
            return sb.ToString();
        }

        public static string ConvertToDoubleByte(this string str)
        {
            var sb = new StringBuilder();
            str.ToList().ForEach(c => sb.Append(ConvertToDoubleByte(c)));
            return sb.ToString();
        }

        /// <summary> 替换最后一个匹配字符串 </summary>
        /// <param name="str">            原字符串 </param>
        /// <param name="oldStr">         即将被替换的字符串 </param>
        /// <param name="newStr">         即将替换的字符串, null =&gt; string.Empty </param>
        /// <param name="comparisonType"> 字符串检查类型 </param>
        /// <returns> 返回替换后的字符串 </returns>
        public static string ReplaceLast(this string str, string oldStr, string newStr, StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                newStr = newStr ?? string.Empty;
                var index = str.LastIndexOf(oldStr, comparisonType);
                if (index != -1)
                {
                    return str.Remove(index, oldStr.Length).Insert(index, newStr);
                }
            }
            return str;
        }

        public static string ReplaceAllToSpace(this string str, List<string> oldStr)
        {
            oldStr.ForEach(i => str = str.Replace(i, ""));
            return str;
        }

        public static List<string> Splits(this string str, params string[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> SplitsWithTrim(this string str, params string[] args)
        {
            return str.Split(args, StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim()).ToList();
        }

        public static string Take(this string str, int startindex, int count)
        {
            return str.Skip(startindex).Take(count).ConverterToString();
        }

        public static string ConverterToString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        /// <summary> 检测是否包含中文字符 </summary>
        /// <returns></returns>
        public static bool ContainsChineseChar(this string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var fileName = System.IO.Path.GetFileName(path);
                var pattern = "^[\u4e00-\u9fa5]$";
                var rx = new Regex(pattern);
                foreach (var item in fileName)
                {
                    if (rx.IsMatch(item.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary> 验证文件名是否合法 </summary>
        /// <param name="fileName"> 需要检测的文件名 </param>
        /// <returns></returns>
        public static bool CheckForFileName(this string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var pattern = @"^[a-zA-Z]:(((\\(?! )[^/:*?<>\""|\\]+)+\\?)|(\\)?)\s*$";
                var regex = new Regex(pattern);
                return regex.IsMatch("C:\\" + fileName);
            }
            return false;
        }

        /// <summary> 是否匹配某一项字符 </summary>
        /// <param name="str">       </param>
        /// <param name="comparison"></param>
        /// <param name="parmas">    </param>
        /// <returns></returns>
        public static bool IsAnyEqual(this string str, params string[] parmas)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return IsAnyEqual(str, StringComparison.InvariantCultureIgnoreCase, parmas);
            }
            return false;
        }

        /// <summary> 是否匹配某一项字符 </summary>
        /// <param name="str">       </param>
        /// <param name="comparison"></param>
        /// <param name="parmas">    </param>
        /// <returns></returns>
        public static bool IsAnyEqual(this string str, StringComparison comparison, params string[] parmas)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return parmas.FirstOrDefault(item => item != null && item.Equals(str, comparison)) != null;
            }
            return false;
        }

        public static bool IsContins(this string str, params string[] parmas)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return parmas.All(i => str.ToLower().Contains(i.ToLower()));
            }
            return false;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        /// <summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("/n", "");
            hex = hex.Replace("//", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }


    }
}