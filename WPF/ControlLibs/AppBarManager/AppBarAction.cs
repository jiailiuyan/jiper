/* 迹I柳燕
 *
 * FileName:   AppBarAction.cs
 * Version:    1.0
 * Date:       2016/11/25 11:30:18
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.WPF.ControlLibs.AppBarManager
 * @class      AppBarAction
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;
using Ji.SystemHelper;

namespace Ji.WPF.ControlLibs.AppBarManager
{
    /// <summary>  </summary>
    public class AppBarAction
    {
        public const int ABM_NEW = 0;
        public const int ABM_REMOVE = 1;
        public const int ABM_QUERYPOS = 2;
        public const int ABM_SETPOS = 3;
        public const int ABM_GETSTATE = 4;
        public const int ABM_GETTASKBARPOS = 5;
        public const int ABM_ACTIVATE = 6;
        public const int ABM_GETAUTOHIDEBAR = 7;
        public const int ABM_SETAUTOHIDEBAR = 8;
        public const int ABM_WINDOWPOSCHANGED = 9;
        public const int ABM_SETSTATE = 10;
        public const int ABN_STATECHANGE = 0;
        public const int ABN_POSCHANGED = 1;
        public const int ABN_FULLSCREENAPP = 2;
        public const int ABN_WINDOWARRANGE = 3;

        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_EXITSIZEMOVE = 0x0232;

        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
        private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        /// <summary> Is AppBar registered </summary>
        public bool IsBarRegistered { get; private set; }

        /// <summary> Number of AppBar's message for WndProc </summary>
        public int WndProcCallBack { get; private set; }

        public static AppBarAction Creat()
        {
            return new AppBarAction();
        }

        protected AppBarAction()
        {
        }

        public void RegisterBar(IntPtr handle, AppBarLocation appbarlocation, int realwidth, int realheight, int viewwidth, int viewheight)
        {
            if (!IsBarRegistered)
            {
                var abd = new APPBARDATA();
                abd.cbSize = Marshal.SizeOf(abd);
                abd.hWnd = handle;

                this.WndProcCallBack = RegisterWindowMessage("AppBarMessage");
                abd.uCallbackMessage = this.WndProcCallBack;

                uint ret = SHAppBarMessage(ABM_NEW, ref abd);
                IsBarRegistered = true;

                ABSetPos(handle, appbarlocation, realwidth, realheight, viewwidth, viewheight);
            }
        }

        public void UnregisterBar(IntPtr handle)
        {
            var abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = handle;

            if (IsBarRegistered)
            {
                SHAppBarMessage(ABM_REMOVE, ref abd);
                IsBarRegistered = false;
            }
        }

        public void ABSetPos(IntPtr hadle, AppBarLocation appbarlocation, int realwidth, int realheight, int viewwidth, int viewheight)
        {
            if (IsBarRegistered)
            {
                var abd = new APPBARDATA();
                abd.cbSize = Marshal.SizeOf(abd);
                abd.hWnd = hadle;
                abd.uEdge = (int)appbarlocation;

                switch (appbarlocation)
                {
                    case AppBarLocation.Left:
                        {
                            abd.rc.left = 0;
                            abd.rc.top = 0;
                            abd.rc.right = (int)(viewwidth * ScreenResolution.RealScale);
                            abd.rc.bottom = realheight;
                            break;
                        }

                    case AppBarLocation.Right:
                        {
                            abd.rc.left = realwidth - (int)(viewwidth * ScreenResolution.RealScale);
                            abd.rc.top = 0;
                            abd.rc.right = realwidth;
                            abd.rc.bottom = realheight;
                            break;
                        }

                    case AppBarLocation.Top:
                        {
                            abd.rc.left = 0;
                            abd.rc.top = 0;
                            abd.rc.right = realwidth;
                            abd.rc.bottom = (int)(viewheight * ScreenResolution.RealScale);
                            break;
                        }

                    case AppBarLocation.Bottom:
                        {
                            abd.rc.left = 0;
                            abd.rc.top = realheight - (int)(viewheight * ScreenResolution.RealScale);
                            abd.rc.right = realwidth;
                            abd.rc.bottom = realheight;
                            break;
                        }

                    default: break;
                }

                SHAppBarMessage(ABM_QUERYPOS, ref abd);
                SHAppBarMessage(ABM_SETPOS, ref abd);

                var left = abd.rc.left;
                MoveWindow(abd.hWnd, left, abd.rc.top, abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top, false);
            }
        }
    }
}