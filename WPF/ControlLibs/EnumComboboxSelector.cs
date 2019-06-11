using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Ji.DataHelper;

namespace Ji.CommonHelper.WPF.ControlLibs
{
    /// <summary>
    /// 具有默认选择器的枚举选择Combobox控件
    /// </summary>
    public class EnumComboboxSelector : ComboBox
    {
        private Dictionary<object, string> EnumsDic = null;

        public new object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static new readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(EnumComboboxSelector), new PropertyMetadata(null, SelectedItemChangedCallback));

        private static void SelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as EnumComboboxSelector);
            if (control != null)
            {
                control.SetSelected();
            }
        }

        public EnumComboboxSelector()
        {
            this.DisplayMemberPath = "Value";
            this.SelectedValuePath = "Key";
        }

        private void SetSelected()
        {
            if (this.SelectedItem != null && this.EnumsDic == null)
            {
                this.EnumsDic = EnumHelper.GetOrderDescriptions(this.SelectedItem);
                this.ItemsSource = this.EnumsDic;
            }
        }
    }
}