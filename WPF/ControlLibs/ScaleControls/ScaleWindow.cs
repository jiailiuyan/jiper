/* 迹I柳燕
 *
 * FileName:   ScaleWindow.cs
 * Version:    1.0
 * Date:       2018/11/21 13:46:18
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ScaleWindow
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
using Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders;

namespace Ji.CommonHelper.WPF.ControlLibs.ScaleControls
{
    /// <summary>  </summary>
    public class ScaleWindow : Window
    {
        public ScaleWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;

            this.Loaded += this.ScaleLabel_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Init();
        }

        private void ScaleLabel_Loaded(object sender, RoutedEventArgs e)
        {
            ScaleViewManager.ScaleChanged += this.ScaleViewManager_ScaleChanged;
        }

        private void ScaleViewManager_ScaleChanged(double obj)
        {
            Init();
        }

        protected virtual void Init()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                var s = ScaleViewManager.Sacle;
                //s = s <= 1 ? 1 : s;
                this.FontSize = this.FontSize * s;

                this.Width = this.Width * s;
                this.Height = this.Height * s;
            }
        }
    }
}
