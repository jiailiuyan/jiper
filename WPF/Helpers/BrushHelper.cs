/* 迹I柳燕
 *
 * FileName:   BrushHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.WPFHelper.ImageHelper
 * @class      BrushHelper
 * @extends
 *
 *             WPF 扩展
 *             对于Brush的扩展方法
 *
 *========================================

 * 
 *
 * 
 *
 */

using System;
using System.Windows.Media;

namespace Ji.WPFHelper.ImageHelper
{
    public static class BrushHelper
    {
        /// <summary> 十六进制颜色值转换为Brush </summary>
        /// <param name="brushStr">传入的颜色字符串</param>
        /// <returns>Brush</returns>
        public static Brush ConvertToBrush(this string brushStr)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(brushStr));
        }

        public static Brush ConvertToBrush(int r, int g, int b)
        {
            return new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
        }

        public static Brush ConvertToBrush(int a, int r, int g, int b)
        {
            return new SolidColorBrush(Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b));
        }

        private static Random random = new Random();

        /// <summary> 随机生成Brush </summary>
        /// <param name="isTransparent">是否可透明</param>
        /// <returns>生成的Brush</returns>
        public static Brush GetRandomBrush(bool isTransparent = false)
        {
            var r = Convert.ToString(random.Next(1, 150), 16);
            r = r.Length < 2 ? r + "F" : r;

            var g = Convert.ToString(random.Next(1, 150), 16);
            g = g.Length < 2 ? g + "F" : g;

            var b = Convert.ToString(random.Next(1, 150), 16);
            b = b.Length < 2 ? b + "F" : b;

            if (isTransparent)
            {
                var f = Convert.ToString(random.Next(1, 150), 16);
                f = f.Length < 2 ? b + "F" : b;

                return ("#" + f + r + g + b).ConvertToBrush();
            }
            else
            {
                return ("#FF" + r + g + b).ConvertToBrush();
            }
        }
    }
}