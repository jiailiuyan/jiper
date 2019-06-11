using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.NativeHelper
{
    public static class NativeHelper
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        //ShowWindow参数
        public const int SW_SHOWNORMAL = 1;

        public const int SW_RESTORE = 9;
        public const int SW_SHOWNOACTIVATE = 4;

        //SendMessage参数
        public const int WM_KEYDOWN = 0X100;

        public const int WM_KEYUP = 0X101;
        public const int WM_SYSCHAR = 0X106;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;
        public const int WM_CHAR = 0X102;

        public const int MOUSEEVENTF_MOVE = 0x0001;      //移动鼠标
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下
        public const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下
        public const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040; //模拟鼠标中键抬起
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标

        ///<summary>将消息加入线索消息队列</summary>
        ///<param name="hwnd">接收消息的那个窗口的句柄。如设为HWND_BROADCAST，表示投递给系统中的所有顶级窗口。如设为零，表示投递一条线程消息（参考PostThreadMessage）</param>
        ///<param name="wMsg">消息标识符</param>
        ///<param name="wParam">具体由消息决定</param>
        ///<param name="lParam">具体由消息决定</param>
        [DllImport("user32.dll", EntryPoint = "PostMessageA")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, uint lParam);

        public static uint MakeLong(short lowPart, short highPart)
        {
            return (((ushort)lowPart) | (uint)(highPart << 16));
        }

        public const int MK_LBUTTON = 0x1;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDOWN = 0x201;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        ///<summary>返回窗口坐标</summary>
        ///<param name="hwnd">想获得范围矩形的那个窗口的句柄</param>
        ///<param name="lpRect">屏幕坐标中随同窗口装载的矩形</param>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]//定义与API相兼容结构体，实际上是一种内存转换
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标
        public static extern int GetCursorPos(ref POINTAPI lpPoint);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(int hWnd, StringBuilder lpString, int nMaxCont);

        //void Mdain()
        //{
        //    POINTAPI point = new POINTAPI();//必须用与之相兼容的结构体，类也可以
        //    //add some wait time

        //    GetCursorPos(ref point);//获取当前鼠标坐标

        //    int hwnd = WindowFromPoint(point.X, point.Y);//获取指定坐标处窗口的句柄
        //    StringBuilder name = new StringBuilder(256);
        //    GetWindowText(hwnd, name, 256);
        //    //LogX.Tip(name.ToString());
        //    GetClassName(hwnd, name, 256);
        //    //LogX.Tip(name.ToString());

        //    dddd.Content = hwnd;
        //    //TrackerWindow((IntPtr)hwnd);
        //}
    }
}