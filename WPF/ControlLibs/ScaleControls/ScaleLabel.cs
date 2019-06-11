/* 迹I柳燕
 *
 * FileName:   ScaleLabel.cs
 * Version:    1.0
 * Date:       2018/11/7 16:42:19
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ScaleLabel
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
    public class ScaleLabel : Label
    {
        public ScaleLabel()
        {
            this.Loaded += this.ScaleLabel_Loaded;
        }

        private void ScaleLabel_Loaded(object sender, RoutedEventArgs e)
        {
            //Init();
            //throw new NotImplementedException();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Init();
        }

        protected void Init()
        {
            if (  this.FontSize > 0)
            {
                var s = ScaleViewManager.Sacle;
                s = s <= 1 ? 1 : s;
                this.FontSize = this.FontSize * s;

                //this.Width = this.ActualWidth * s;
                //this.Height = this.ActualHeight * s;
            }

        }
    }
}