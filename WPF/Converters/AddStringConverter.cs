using System;
using System.Globalization;
using System.Windows.Data;

namespace Ji.WPFHelper.ConverterHelper
{
    public class AddStringConverter : IValueConverter
    {
        #region Fields

        public static readonly IValueConverter Instance = new AddStringConverter();

        #endregion Fields

        #region Methods

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                return value + parameter.ToString();
            }

            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}