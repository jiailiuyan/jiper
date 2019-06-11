using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Ji.WPFHelper.ConverterHelper
{
    public class BoolConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new BoolConverter();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "Reverse")
            {
                var isview = value != null && value is bool && (bool)value;
                isview = !isview;
                return isview;
            }

            return !string.IsNullOrWhiteSpace(value.ToString());
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}