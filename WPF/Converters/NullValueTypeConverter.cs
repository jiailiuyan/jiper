/* 迹I柳燕
 *
 * FileName:   NullValueTypeConverter.cs
 * Version:    1.0
 * Date:       2018/11/12 17:58:56
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.WPFHelper.ConverterHelper
 * @class      NullValueTypeConverter
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
using System.Windows.Data;

namespace Ji.WPFHelper.ConverterHelper
{
    /// <summary>  </summary>
    public class NullValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString()?.Trim() == "")
            {
                return null;
            }

            return value;
        }
    }
}
