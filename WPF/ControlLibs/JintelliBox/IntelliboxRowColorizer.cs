using System.Windows.Data;
using System.Windows.Media;
using System;
using System.Windows.Controls;

namespace JintelliBox
{
    /// <summary>
    ///  Provides an abstract implementation of an <see cref="IValueConverter" /> that converts a
    ///  ListBoxItem into a Brush based on the index of the ListBoxItem in its parent collection.
    /// </summary>
    public abstract class IntelliboxRowColorizer : IValueConverter
    {
        /// <summary>
        ///  Must be overridden by derived classes to return the brushes that the derived class wants
        ///  to use when converting from a ListBoxItem to a System.Windows.Media.Brush
        /// </summary>
        /// <returns>
        ///  The System.Windows.Media.Brushes that should be used when converting a ListBoxItem to a Brush.
        /// </returns>
        protected abstract Brush[] GetBrushes();

        /// <summary>
        ///  Converts a ListBoxItem to the System.Windows.Media.Brush based upon its ordinal position
        ///  within its containing ListBox.
        /// </summary>
        /// <param name="value">      A ListBoxItem or a derived class. </param>
        /// <param name="targetType"> Should be a System.Windows.Media.Brush type. </param>
        /// <param name="parameter">  NOT USED. The converter parameter to use. </param>
        /// <param name="culture">    NOT USED. The culture to use in the converter. </param>
        /// <returns> The brush to use for the background of the ListBoxItem. </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target type must be a  System.Windows.Media.Brush");

            var boxItem = value as ListBoxItem;
            if (boxItem != null)
            {
                var listCtrl = ItemsControl.ItemsControlFromItemContainer(boxItem);
                if (listCtrl != null)
                {
                    var index = listCtrl.ItemContainerGenerator.IndexFromContainer(boxItem);

                    var brushes = GetBrushes();
                    if (brushes == null || brushes.Length < 1)
                        return null;

                    return brushes[index % brushes.Length];
                }
            }

            return null;
        }

        /// <summary>
        ///  Throws an <see cref="InvalidOperationException" /> when called, since it is impossible
        ///  to reverse the conversion done by the Convert() method.
        /// </summary>
        /// <param name="value">      N/A </param>
        /// <param name="targetType"> N/A </param>
        /// <param name="parameter">  N/A </param>
        /// <param name="culture">    N/A </param>
        /// <returns> N/A </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Impossible to convert from a Brush to a ListViewItem!");
        }
    }
}