/* 迹I柳燕
 *
 * FileName:   ImageButton.cs
 * Version:    1.0
 * Date:       2018/11/21 17:49:07
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ImageButton
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
using System.Windows.Media;

namespace Ji.CommonHelper.WPF.ControlLibs.ScaleControls
{
    /// <summary>  </summary>
    public class ImageButton : Border
    {

        //<Border
        //             Width = "20"
        //             Height="20"
        //             Margin="7,0,0,0"
        //             Background="#4CB1E5">
        //             <Border.OpacityMask>
        //                 <ImageBrush ImageSource = "notifyicon.png" />
        //             </ Border.OpacityMask >

        //         </ Border >

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null, ImageSourceChangedCallback));

        private static void ImageSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ImageButton);
            if (control != null)
            {
                control.OpacityMask = new ImageBrush((ImageSource)e.NewValue);
            }
        }

        public ImageButton()
        {
            this.Width = 16;
            this.Height = 16;

            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CB1E5"));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Init();
        }

        protected virtual void Init()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                var s = ScaleViewManager.Sacle;
                //s = s <= 1 ? 1 : s;

                this.Width = this.Width * s;
                this.Height = this.Height * s;
            }
        }
    }
}