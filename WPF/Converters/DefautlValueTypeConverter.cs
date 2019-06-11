/* 迹I柳燕
 *
 * FileName:   DefautlValueTypeConverter.cs
 * Version:    1.0
 * Date:       2018/11/12 18:32:50
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Converters
 * @class      DefautlValueTypeConverter
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
    public class DefautlValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value?.ToString()?.Trim() == "")
            {
                return parameter;
            }

            return value;
        }
    }
}
