/* 迹I柳燕
 *
 * FileName:   TestConverter.cs
 * Version:    1.0
 * Date:       2018/11/12 18:24:40
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Converters
 * @class      TestConverter
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Globalization;
using System.Windows.Data;

namespace Ji.WPFHelper.ConverterHelper
{
    /// <summary>  </summary>
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            return value;
        }
    }
}