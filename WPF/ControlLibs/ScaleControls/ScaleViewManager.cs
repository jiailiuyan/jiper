/* 迹I柳燕
 *
 * FileName:   ScaleViewManager.cs
 * Version:    1.0
 * Date:       2018/11/7 16:18:28
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.ControlLibs.ScaleControls
 * @class      ScaleViewManager
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ji.SystemHelper;

namespace Ji.CommonHelper.WPF.ControlLibs.ScaleControls
{
    /// <summary>  </summary>
    public class ScaleViewManager
    {

        public const int DesignWidth = 1366;
        public const int DesignHeight = 738;

        public static int WorkWidth { get; private set; }

        public static int WorkHeight { get; private set; }

        public static double Sacle { get; private set; } = 1d;

        public static event Action<double> ScaleChanged = delegate { };

        public static void SetWorkArea()
        {
            var rs = ScreenResolution.RealScale;
            var width = ScreenResolution.RealScreenWidth / rs;
            var height = ScreenResolution.RealScreenHeight / rs;

            var ws = (width * 1d) / DesignWidth;
            var hs = (height * 1d) / DesignHeight;
            var s = ws > hs ? hs : ws;

            Sacle = s;
            WorkWidth = (int)width;
            WorkHeight = (int)height;

            ScaleChanged(Sacle);
        }
    }
}
