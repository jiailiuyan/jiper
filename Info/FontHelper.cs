using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Ji.DataHelper.Info
{
    public static class FontHelper
    {
        /// <summary>
        /// FontHelper.Measure(s, this.FontSize, this.FontFamily.Source);
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="typeFace"></param>
        /// <returns></returns>
        public static Size Measure(this string text, double fontSize, string typeFace)
        {
            FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(typeFace), fontSize, Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
    }
}