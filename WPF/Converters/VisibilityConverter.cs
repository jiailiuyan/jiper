using System;
using System.Windows;
using System.Windows.Data;

namespace Ji.WPFHelper.ConverterHelper
{
    public class VisibilityConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new VisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "Hide")
            {
                return Visibility.Collapsed;
            }

            var isview = value != null && value is bool && (bool)value;

            if (parameter != null && parameter.ToString() == "Reverse")
            {
                isview = !isview;
            }
            //if (parameter == null)
            //{
            //    return isview ? Visibility.Visible : Visibility.Hidden;
            //}

            return isview ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}