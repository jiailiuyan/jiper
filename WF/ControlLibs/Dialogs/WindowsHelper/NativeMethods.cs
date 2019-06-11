using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public static class NativeMethods
    {
        public const int HWND_TOP = 0x0;
        public const int SW_SHOW = 5;
        public const int SW_HIDE = 0;

        ///<summary>
        ///最小化窗口
        ///      hwnd:Integer型,欲最小化的那个窗口的句柄
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int CloseWindow(IntPtr hwnd);

        ///<summary>
        ///设备窗口使能状态
        ///      hwnd:Integer型,窗口句柄
        ///      fEnable:Integer型,非零允许窗口，零禁止
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int EnableWindow(IntPtr hwnd, int fEnable);

        ///<summary>
        ///在目录标题或控制窗口中设置窗口文本
        ///      hwnd:Integer型,要设置文字的窗口的句柄
        ///      lpString:String型，要设到hwnd窗口中的文字
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hwnd, string lpString);

        ///<summary>
        ///显示窗口
        ///      hwnd:Integer型,窗口句柄，要向这个窗口应用由nCmdShow指定的命令
        ///      nCmdShow:Integer型,为窗口指定可视性方面的一个命令。请用下述任何一个常数
        ///      SW_HIDE
        ///      隐藏窗口，活动状态给令一个窗口
        ///      SW_MINIMIZE
        ///      最小化窗口，活动状态给令一个窗口
        ///      SW_RESTORE
        ///      用原来的大小和位置显示一个窗口，同时令其进入活动状态
        ///      SW_SHOW
        ///      用当前的大小和位置显示一个窗口，同时令其进入活动状态
        ///      SW_SHOWMAXIMIZED
        ///      最大化窗口，并将其激活
        ///      SW_SHOWMINIMIZED
        ///      最小化窗口，并将其激活
        ///      SW_SHOWMINNOACTIVE
        ///      最小化一个窗口，同时不改变活动窗口
        ///      SW_SHOWNA
        ///      用当前的大小和位置显示一个窗口，不改变活动窗口
        ///      SW_SHOWNOACTIVATE
        ///      用最近的大小和位置显示一个窗口，同时不改变活动窗口
        ///      SW_SHOWNORMAL
        ///      与SW_RESTORE相同
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        ///<summary>返回窗口线索及过程ID</summary>
        ///<param name="hwnd">指定窗口句柄</param>
        ///<param name="lpdwProcessId">指定一个变量，用于装载拥有那个窗口的一个进程的标识符</param>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

        ///<summary>将句柄返回给过程对象</summary>
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary> 为指定的进程分配内存地址:成功则返回分配内存的首地址 </summary>
        [DllImport("kernel32.dll")]
        public static extern int VirtualAllocEx(int hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        /// <summary> 将数据写入内存中 </summary>
        /// <param name="hProcess"> 由OpenProcess返回的进程句柄 </param>
        /// <param name="lpBaseAddress"> 要写的内存首地址,再写入之前,此函数将先检查目标地址是否可用,并能容纳待写入的数据 </param>
        /// <param name="lpBuffer"> 指向要写的数据的指针 </param>
        /// <param name="nSize"> 要写入的字节数 </param>
        /// <param name="vNumberOfBytesRead"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        /// <summary> 从指定内存中读取字节集数据 </summary>
        /// <param name="hProcess"> 被读取者的进程句柄 </param>
        /// <param name="lpBaseAddress"> 开始读取的内存地址 </param>
        /// <param name="lpBuffer"> 数据存储变量 </param>
        /// <param name="nSize"> 要写入多少字节 </param>
        /// <param name="vNumberOfBytesRead"> 读取长度 </param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        #region Delegates

        public delegate bool EnumWindowsCallBack(IntPtr hWnd, int lParam);

        #endregion Delegates

        #region USER32

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.Dll")]
        public static extern int GetDlgCtrlID(IntPtr hWndCtl);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int MapWindowPoints(IntPtr hWnd, IntPtr hWndTo, ref POINT pt, int cPoints);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, out WINDOWINFO pwi);

        [DllImport("User32.Dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder param, int length);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hWnd, StringBuilder param, int length);

        [DllImport("user32.Dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsCallBack lpEnumFunc, int lParam);

        [DllImport("user32.Dll")]
        public static extern bool EnumWindows(EnumWindowsCallBack lpEnumFunc, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr hParent, POINT pt, ChildFromPointFlags flags);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, StringBuilder param);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, char[] chars);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);

        #endregion USER32
    }
}