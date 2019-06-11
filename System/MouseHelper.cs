using System;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Ji.SystemHelper
{
    public static class KeyboardConstaint
    {
        public static readonly short VK_F1 = 0x70;
        public static readonly short VK_F2 = 0x71;
        public static readonly short VK_F3 = 0x72;
        public static readonly short VK_F4 = 0x73;
        public static readonly short VK_F5 = 0x74;
        public static readonly short VK_F6 = 0x75;
        public static readonly short VK_F7 = 0x76;
        public static readonly short VK_F8 = 0x77;
        public static readonly short VK_F9 = 0x78;
        public static readonly short VK_F10 = 0x79;
        public static readonly short VK_F11 = 0x7A;
        public static readonly short VK_F12 = 0x7B;

        public static readonly short VK_LEFT = 0x25;
        public static readonly short VK_UP = 0x26;
        public static readonly short VK_RIGHT = 0x27;
        public static readonly short VK_DOWN = 0x28;

        public static readonly short VK_NONE = 0x00;
        public static readonly short VK_ESCAPE = 0x1B;
        public static readonly short VK_EXECUTE = 0x2B;
        public static readonly short VK_CANCEL = 0x03;
        public static readonly short VK_RETURN = 0x0D;
        public static readonly short VK_ACCEPT = 0x1E;
        public static readonly short VK_BACK = 0x08;
        public static readonly short VK_TAB = 0x09;
        public static readonly short VK_DELETE = 0x2E;
        public static readonly short VK_CAPITAL = 0x14;
        public static readonly short VK_NUMLOCK = 0x90;
        public static readonly short VK_SPACE = 0x20;
        public static readonly short VK_DECIMAL = 0x6E;
        public static readonly short VK_SUBTRACT = 0x6D;

        public static readonly short VK_ADD = 0x6B;
        public static readonly short VK_DIVIDE = 0x6F;
        public static readonly short VK_MULTIPLY = 0x6A;
        public static readonly short VK_INSERT = 0x2D;

        public static readonly short VK_OEM_1 = 0xBA;  // ';:' for US
        public static readonly short VK_OEM_PLUS = 0xBB;  // '+' any country

        public static readonly short VK_OEM_MINUS = 0xBD;  // '-' any country

        public static readonly short VK_OEM_2 = 0xBF;  // '/?' for US
        public static readonly short VK_OEM_3 = 0xC0;  // '`~' for US
        public static readonly short VK_OEM_4 = 0xDB;  //  '[{' for US
        public static readonly short VK_OEM_5 = 0xDC;  //  '\|' for US
        public static readonly short VK_OEM_6 = 0xDD;  //  ']}' for US
        public static readonly short VK_OEM_7 = 0xDE;  //  ''"' for US
        public static readonly short VK_OEM_PERIOD = 0xBE;  // '.>' any country
        public static readonly short VK_OEM_COMMA = 0xBC;  // ',<' any country
        public static readonly short VK_SHIFT = 0x10;
        public static readonly short VK_CONTROL = 0x11;
        public static readonly short VK_MENU = 0x12;
        public static readonly short VK_LWIN = 0x5B;
        public static readonly short VK_RWIN = 0x5C;
        public static readonly short VK_APPS = 0x5D;

        public static readonly short VK_LSHIFT = 0xA0;
        public static readonly short VK_RSHIFT = 0xA1;
        public static readonly short VK_LCONTROL = 0xA2;
        public static readonly short VK_RCONTROL = 0xA3;
        public static readonly short VK_LMENU = 0xA4;
        public static readonly short VK_RMENU = 0xA5;

        public static readonly short VK_SNAPSHOT = 0x2C;
        public static readonly short VK_SCROLL = 0x91;
        public static readonly short VK_PAUSE = 0x13;
        public static readonly short VK_HOME = 0x24;

        public static readonly short VK_NEXT = 0x22;
        public static readonly short VK_PRIOR = 0x21;
        public static readonly short VK_END = 0x23;

        public static readonly short VK_NUMPAD0 = 0x60;
        public static readonly short VK_NUMPAD1 = 0x61;
        public static readonly short VK_NUMPAD2 = 0x62;
        public static readonly short VK_NUMPAD3 = 0x63;
        public static readonly short VK_NUMPAD4 = 0x64;
        public static readonly short VK_NUMPAD5 = 0x65;
        public static readonly short VK_NUMPAD5NOTHING = 0x0C;
        public static readonly short VK_NUMPAD6 = 0x66;
        public static readonly short VK_NUMPAD7 = 0x67;
        public static readonly short VK_NUMPAD8 = 0x68;
        public static readonly short VK_NUMPAD9 = 0x69;

        public static readonly short KEYEVENTF_EXTENDEDKEY = 0x0001;
        public static readonly short KEYEVENTF_KEYUP = 0x0002;

        public static readonly int GWL_EXSTYLE = -20;
        public static readonly int WS_DISABLED = 0X8000000;
        public static readonly int WM_SETFOCUS = 0X0007;

        public static readonly short V = 0x56;
    }

    public static class MouseHelper
    {
        /// <summary> 获取屏幕鼠标位置 </summary>
        /// <returns></returns>
        public static Ji.SystemHelper.MouseDriver.POINT GetRealMousePoint()
        {
            Ji.SystemHelper.MouseDriver.POINT currentpoint;
            Ji.SystemHelper.MouseDriver.GetCursorPos(out currentpoint);
            return currentpoint;
        }
    }

    public class MouseDriver
    {
        private const string LockStr = "MouseDriver";
        private static MouseDriver instance;

        /// <summary></summary>
        public static MouseDriver Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockStr)
                    {
                        if (instance == null)
                        {
                            instance = new MouseDriver();
                        }
                    }
                }
                return instance;
            }
        }

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("Kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Auto)]
        private static extern int GetTickCount();

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        [Flags()]
        private enum MOUSEEVENTF
        {
            MOVE = 0x0001,  // mouse move
            LEFTDOWN = 0x0002,  // left button down
            LEFTUP = 0x0004,  // left button up
            RIGHTDOWN = 0x0008,  // right button down
            RIGHTUP = 0x0010,  // right button up
            MIDDLEDOWN = 0x0020,  // middle button down
            MIDDLEUP = 0x0040,  // middle button up
            XDOWN = 0x0080,  // x button down
            XUP = 0x0100,  // x button down
            WHEEL = 0x0800,  // wheel button rolled
            VIRTUALDESK = 0x4000,  // map to entire virtual desktop
            ABSOLUTE = 0x8000,  // absolute move
        }

        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;

            [FieldOffset(4)]
            public MOUSEINPUT mi;

            [FieldOffset(4)]
            public KEYBDINPUT ki;

            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        public enum MouseMoveType
        {
            Left,
            Right,
            Up,
            Down
        }

        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary> 获取鼠标的坐标 </summary>
        /// <param name="lpPoint"> 传址参数，坐标point类型 </param>
        /// <returns> 获取成功返回真 </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        public static int NextValue = 16;

        /// <summary> 上一次的鼠标坐标，如果小于50像素的，平滑处理 </summary>
        //private int lastX = 0;

        //private int lastY = 0;

        public void MoveMouse(MouseMoveType type)
        {
            POINT currentpoint;
            if (GetCursorPos(out currentpoint))
            {
                switch (type)
                {
                    case MouseMoveType.Left: Move(currentpoint.X - NextValue, currentpoint.Y); return;
                    case MouseMoveType.Right: Move(currentpoint.X + NextValue, currentpoint.Y); return;
                    case MouseMoveType.Up: Move(currentpoint.X, currentpoint.Y - NextValue); return;
                    case MouseMoveType.Down: Move(currentpoint.X, currentpoint.Y + NextValue); return;
                }
            }
        }

        ///标记上一次鼠标是否是左键按下，以便恢复正常
        //private bool isLastMouseLeftDown = false;

        /// <summary> This function moves the cursor to a specific point at the screen. </summary>
        /// <param name="x"> X coordinate of the posotion as pixel </param>
        /// <param name="y"> Y coordinate of the posotion as pixel </param>
        /// <returns> Returns 0 if there was an error otherwise 1. </returns>
        public void Move(double x, double y, bool isleftdown = false)
        {
            //if (GlobalSettings.Current.IsMouseMovePaused)
            //    return;

            float ScreenWidth = ScreenResolution.RealScreenWidth;
            float ScreenHeight = ScreenResolution.RealScreenHeight;

            double WidthFactor = ScreenResolution.RealScale;
            double HeightFactor = ScreenResolution.RealScale;

            INPUT input_move = new INPUT();
            input_move.mi.dx = (int)Math.Round(x * WidthFactor * (65535 / ScreenWidth), 0);
            input_move.mi.dy = (int)Math.Round(y * HeightFactor * (65535 / ScreenHeight), 0);

            input_move.mi.mouseData = 0;

            if (isleftdown)
                input_move.mi.dwFlags = (int)(MOUSEEVENTF.MOVE | MOUSEEVENTF.ABSOLUTE | MOUSEEVENTF.LEFTDOWN);
            else
                input_move.mi.dwFlags = (int)(MOUSEEVENTF.MOVE | MOUSEEVENTF.ABSOLUTE);

            INPUT[] input = { input_move };
            SendInput(1, input, Marshal.SizeOf(input_move));
        }

        /// <summary>
        ///  Currently not used in the gaze project /Martin This function simulates a simple
        ///  mouseclick at the current cursor position.
        /// </summary>
        /// <returns> All right if it is 2. All below indicates an error. </returns>
        public static uint Click()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;

            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;

            INPUT[] input = { input_down, input_up };
            return SendInput(2, input, Marshal.SizeOf(input_down));
        }

        /// <summary>
        ///  Currently not used in the gaze project /Martin This function simulates a simple
        ///  mouseclick at the current cursor position.
        /// </summary>
        /// <returns> All right if it is 2. All below indicates an error. </returns>
        public static uint ClickRightButton()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.RIGHTDOWN;

            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.RIGHTUP;

            INPUT[] input = { input_down, input_up };
            return SendInput(2, input, Marshal.SizeOf(input_down));
        }

        /// <summary> 鼠标中键单击 </summary>
        /// <returns></returns>
        public static uint ClickMidButton()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.MIDDLEDOWN;

            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.MIDDLEUP;

            INPUT[] input = { input_down, input_up };
            return SendInput(2, input, Marshal.SizeOf(input_down));
        }

        /// <summary> 鼠标左键按下 </summary>
        /// <returns></returns>
        public static uint LBtnDown()
        {
            INPUT input_down = new INPUT();
            input_down.mi.dx = 0;
            input_down.mi.dy = 0;
            input_down.mi.mouseData = 0;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;

            INPUT[] input = { input_down };
            return SendInput(1, input, Marshal.SizeOf(input_down));
        }

        /// <summary> 鼠标左键释放 </summary>
        /// <returns></returns>
        public static uint LBtnUp()
        {
            INPUT input_up = new INPUT();
            input_up.mi.dx = 0;
            input_up.mi.dy = 0;
            input_up.mi.mouseData = 0;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;

            INPUT[] input = { input_up };
            return SendInput(1, input, Marshal.SizeOf(input_up));
        }

        /// <summary> 发送完整按键信号，按下，释放 </summary>
        /// <param name="VKCode"> VKCode </param>
        public void SendKeyCommand(short VKCode)
        {
            short key = VKCode;
            SendKeyDown(key);
            SendKeyUp(key);
        }

        /// <summary> 发送按键按下信号 </summary>
        /// <param name="key"> VKCode </param>
        public void SendKeyDown(short key)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = (int)InputType.INPUT_KEYBOARD;
            input[0].ki.wVk = key;
            input[0].ki.time = GetTickCount();

            if (SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary> 发送按键释放信号 </summary>
        /// <param name="key"> VKCode </param>
        public void SendKeyUp(short key)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = (int)InputType.INPUT_KEYBOARD;
            input[0].ki.wVk = key;
            input[0].ki.dwFlags = KeyboardConstaint.KEYEVENTF_KEYUP;
            input[0].ki.time = GetTickCount();

            if (SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) < input.Length)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}