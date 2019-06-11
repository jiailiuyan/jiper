/* 迹I柳燕
 *
 * FileName:   WindowControllerButton.cs
 * Version:    1.0
 * Date:       2018/10/26 15:51:19
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders
 * @class      WindowControllerButton
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders
{
    /// <summary>  </summary>
    public class WindowControllerButton : Border
    {
        public event Action OnClick = null;

        public Brush Icon
        {
            get { return (Brush)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Brush), typeof(WindowControllerButton), new PropertyMetadata(null));

        public Brush HorverBrush
        {
            get { return (Brush)GetValue(HorverBrushProperty); }
            set { SetValue(HorverBrushProperty, value); }
        }

        public static readonly DependencyProperty HorverBrushProperty =
            DependencyProperty.Register("HorverBrush", typeof(Brush), typeof(WindowControllerButton), new PropertyMetadata(Brushes.Transparent));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(WindowControllerButton), new PropertyMetadata(new Thickness(3)));

        public WindowControllerButton()
        {
            this.Width = 30;
            this.Height = 30;

            this.Background = Brushes.Transparent;
            var iv = new Rectangle() { };
            iv.SetBinding(Rectangle.FillProperty, new Binding("Icon") { Source = this });
            iv.SetBinding(Rectangle.MarginProperty, new Binding("IconMargin") { Source = this });
            this.Child = iv;
        }

        private Point? MLP = null;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.MLP = e.GetPosition(this);
            e.Handled = true;
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.Background = this.HorverBrush;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            this.Background = Brushes.Transparent;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (this.MLP != null)
            {
                this.OnClick?.Invoke();
                this.MLP = null;
                e.Handled = true;
            }
            base.OnPreviewMouseLeftButtonUp(e);
        }

    }
}
