using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Ji.SystemHelper;

namespace Ji.InputHelper
{
    public class MouseExtension
    {
        #region Fields

        public static int NextValue = 16;
        private const string LockStr = "MouseDriver";
        private static MouseExtension instance;

        #endregion Fields

        #region Enums

        public enum MouseMoveType
        {
            Left, Right, Up, Down
        }

        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
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

        #endregion Enums

        #region Properties

        /// <summary></summary>
        public static MouseExtension Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockStr)
                    {
                        if (instance == null)
                        {
                            instance = new MouseExtension();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion Properties

        #region Methods

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

        /// <summary> 获取鼠标的坐标 </summary>
        /// <param name="lpPoint"> 传址参数，坐标point类型 </param>
        /// <returns> 获取成功返回真 </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

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

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary> This function moves the cursor to a specific point at the screen. </summary>
        /// <param name="x"> X coordinate of the posotion as pixel </param>
        /// <param name="y"> Y coordinate of the posotion as pixel </param>
        /// <returns> Returns 0 if there was an error otherwise 1. </returns>
        public void Move(double x, double y, bool isleftdown = false)
        {
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

        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("Kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Auto)]
        private static extern int GetTickCount();

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        #endregion Methods

        #region Structs

        public struct POINT
        {
            #region Fields

            public int X;
            public int Y;

            #endregion Fields

            #region Constructors

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            #endregion Constructors
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
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        #endregion Structs
    }
}