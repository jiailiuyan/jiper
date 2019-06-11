using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ji.WPFHelper.Bindings
{
    public class BindingProperty : DependencyObject
    {
        public static BindingBase GetValue(DependencyObject obj) { return (BindingBase)obj.GetValue(ValueProperty); }

        public static void SetValue(DependencyObject obj, BindingBase value) { obj.SetValue(ValueProperty, value); }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(BindingBase), typeof(BindingProperty), new PropertyMetadata(null, ValueChangedCallback));

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bb = (e.NewValue as BindingBase);
            if (bb != null)
            {
                bb.SetBinding(d as FrameworkElement);
            }
        }
    }
}