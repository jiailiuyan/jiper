using System;
using System.Windows;
using System.Windows.Data;

namespace HeaderWindow
{
    internal class EffectShowConverter : IValueConverter
    {
        public static readonly IValueConverter Converter = new EffectShowConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is WindowState)
            {
                if ((WindowState)value == WindowState.Maximized)
                {
                    return new Thickness(0);
                }
                else
                {
                    double span = 0;
                    if (parameter != null && double.TryParse(parameter.ToString(), out span))
                    {
                        return new Thickness(span);
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
