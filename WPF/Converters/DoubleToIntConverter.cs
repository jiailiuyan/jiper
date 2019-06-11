using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ji.CommonHelper.WPF.Converters
{
    /// <summary> Double转int类型转换器 </summary>
    public class DoubleToIntConverter : IValueConverter
    {
        public static readonly DoubleToIntConverter Converter = new DoubleToIntConverter();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double default1 = 0.0;
            Double.TryParse(value.ToString(), out default1);
            return default1.ToString("0");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}