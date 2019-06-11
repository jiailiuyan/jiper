using System;
using System.Runtime.InteropServices;

namespace Ji.NativeMessage
{
    internal static class NativeMedthod
    {
        internal const int GW_CHILD = 0x5;
        internal const int GW_HWNDNEXT = 0x2;

        internal delegate int HookProc(int code, IntPtr wparam, ref CWPSTRUCT cwp);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetWindowsHookEx(int type, HookProc hook, IntPtr instance, int threadID);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int CallNextHookEx(IntPtr hookHandle, int code, IntPtr wparam, ref CWPSTRUCT cwp);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowThreadProcessId(IntPtr hwnd, int ID);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("kernel32")]
        internal static extern int GetCurrentThreadId();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        internal struct CWPSTRUCT
        {
            public int lparam;
            public int wparam;
            public int message;
            public IntPtr hwnd;
        }

    }
}
