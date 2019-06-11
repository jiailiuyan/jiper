/* 迹I柳燕
 *
 * FileName:   VolumeHelper.cs
 * Version:    1.0
 * Date:       2017/9/11 10:14:26
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.System
 * @class      VolumeHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Runtime.InteropServices;
using System.Windows;
using Ji.CommonHelper.WPF.Helpers;

namespace Ji.CommonHelper.System
{
    /// <summary></summary>
    public static class VolumeHelper
    {
        #region Fields

        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int WM_APPCOMMAND = 0x319;

        private static IntPtr Handle = IntPtr.Zero;

        #endregion Fields

        #region Constructors

        static VolumeHelper()
        {
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                Handle = WindowHelper.GetHandle(Application.Current.MainWindow);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary> 减小音量 </summary>
        /// <param name="downValue"> 减小值 </param>
        public static void DownVolume(int downValue = 1)
        {
            for (int i = 0; i < downValue; i++)
            {
                SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public static void SetHandle(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary> 增大音量 </summary>
        /// <param name="upValue"> 增加值 </param>
        public static void UpVolume(int upValue = 1)
        {
            for (int i = 0; i < upValue; i++)
            {
                SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
            }
        }

        #endregion Methods
    }
}