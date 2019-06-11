using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ji.WPFHelper.Bindings
{
    public static class BindingTextHelper
    {
        public static void SetBinding(this BindingBase bingdingbase, FrameworkElement element)
        {
            if (element != null)
            {
                DependencyProperty bindingproperty = null;
                if (element is ContentControl)
                {
                    bindingproperty = ContentControl.ContentProperty;
                }
                else if (element is TextBlock)
                {
                    bindingproperty = TextBlock.TextProperty;
                }
                else if (element is TextBox)
                {
                    bindingproperty = TextBox.TextProperty;
                }

                if (bindingproperty != null)
                {
                    element.SetBinding(bindingproperty, bingdingbase);
                }
            }
        }
    }
}