using System;
using System.Globalization;
using System.Windows.Data;

namespace Ji.CommonHelper.WPF.Converters
{
    /// <summary> Double转保留一位小数转换器 </summary>
    public class DoubleToOneConverter : IValueConverter
    {
        #region Fields

        public static readonly DoubleToOneConverter Converter = new DoubleToOneConverter();

        #endregion Fields

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double default1 = 0d;
            if (value is double && double.TryParse(value.ToString(), out default1))
            {
                return default1.ToString("0.0");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}