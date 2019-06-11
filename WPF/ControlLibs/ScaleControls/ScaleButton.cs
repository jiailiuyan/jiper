/* 迹I柳燕
 *
 * FileName:   ScaleButton.cs
 * Version:    1.0
 * Date:       2018/11/7 16:14:41
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ScaleButton
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Ji.CommonHelper.WPF.ControlLibs.ScaleControls
{
    /// <summary>  </summary>
    public class ScaleButton : Button
    {

        public ScaleButton()
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
            this.Width = 80;
            this.Height = 30;
            this.FontSize = 12;

            var s = ScaleViewManager.Sacle;
            s = s <= 1 ? 1 : s;
            this.FontSize = this.FontSize * s;

            this.Width = this.Width * s;
            this.Height = this.Height * s;
        }
    }
}
