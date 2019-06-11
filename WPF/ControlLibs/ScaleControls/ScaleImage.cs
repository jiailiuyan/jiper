/* 迹I柳燕
 *
 * FileName:   ScaleImage.cs
 * Version:    1.0
 * Date:       2018/11/21 15:50:19
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ScaleImage
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

namespace Ji.CommonHelper.WPF.ControlLibs.ScaleControls
{
    /// <summary>  </summary>
    public class ScaleImage : Image
    {
        public ScaleImage()
        {
            this.Loaded += this.ScaleLabel_Loaded;
        }

        private void ScaleLabel_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            ScaleViewManager.ScaleChanged += this.ScaleViewManager_ScaleChanged;
        }

        private void ScaleViewManager_ScaleChanged(double obj)
        {
            Init();
        }

        protected void Init()
        {
            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                var s = ScaleViewManager.Sacle;
                s = s <= 1 ? 1 : s;

                this.Width = this.ActualWidth * s;
                this.Height = this.ActualHeight * s;

                var m = this.Margin;
                this.Margin = new Thickness(m.Left * s, m.Top * s, m.Right * s, m.Bottom * s);
            }

        }

    }
}
