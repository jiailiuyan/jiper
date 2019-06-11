using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public class WindowHelper
    {
        public bool haveMainWindow = false;
        public IntPtr mainWindowHandle = IntPtr.Zero;
        public int processId = 0;

        public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

        public IntPtr GetMainWindowHandle(int processId)
        {
            if (!this.haveMainWindow)
            {
                this.mainWindowHandle = IntPtr.Zero;
                this.processId = processId;
                EnumThreadWindowsCallback callback = new EnumThreadWindowsCallback(this.EnumWindowsCallback);
                EnumWindows(callback, IntPtr.Zero);
                GC.KeepAlive(callback);

                this.haveMainWindow = true;
            }
            return this.mainWindowHandle;
        }

        public bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
        {
            int num;
            GetWindowThreadProcessId(new HandleRef(this, handle), out num);
            if ((num == this.processId) && this.IsMainWindow(handle))
            {
                this.mainWindowHandle = handle;
                return false;
            }
            return true;
        }

        public bool IsMainWindow(IntPtr handle)
        {
            return (!(GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && IsWindowVisible(new HandleRef(this, handle)));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumWindows(EnumThreadWindowsCallback callback, IntPtr extraData);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(HandleRef hWnd);

        private static Hashtable processWnd = new Hashtable();

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);

        public static IntPtr GetCurrentWindowHandle()
        {
            IntPtr ptrWnd = IntPtr.Zero;
            uint uiPid = (uint)Process.GetCurrentProcess().Id;
            return (new WindowHelper()).GetMainWindowHandle((int)uiPid);
        }

        private const int SW_SHOWDEFAULT = 0xA;

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        ///<summary>将窗口置于前台</summary>
        ///<param name="hwnd">带到前台的窗口</param>
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        public static int ShowWindow(IntPtr hwnd)
        {
            SetForegroundWindow(hwnd);
            return 0;
        }

        public static void ShowCurrentWindowHandle()
        {
            ShowWindow(GetCurrentWindowHandle());
        }

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        private const int MAX_PATH = 260;
        private const int CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019;

        public static string GetAllUsersDesktopFolderPath()
        {
            StringBuilder sbPath = new StringBuilder(MAX_PATH);
            SHGetFolderPath(IntPtr.Zero, CSIDL_COMMON_DESKTOPDIRECTORY, IntPtr.Zero, 0, sbPath);
            return sbPath.ToString();
        }
    }
}