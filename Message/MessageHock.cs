using System;
using System.Collections.Generic;
using static Ji.NativeMessage.NativeMedthod;

namespace Ji.NativeMessage
{
    public class MessageHock
    {
        private HookProc hookProc;
        private IntPtr hookHandle = IntPtr.Zero;

        public static readonly MessageHock Instance = new MessageHock();

        public delegate void aSeePointHandle(object data);

        public event aSeePointHandle OnGetGazeData = (p) => { };
        public event aSeePointHandle OnGetTrackingData = (p) => { };

        public List<IntPtr> SendList = new List<IntPtr>();

        private MessageHock()
        {
            this.hookProc = new HookProc(hookproc);
        }

        public bool Start()
        {
            this.hookHandle = SetWindowsHookEx(4, hookProc, IntPtr.Zero, GetCurrentThreadId());
            return ((int)this.hookHandle != 0);
        }

        public bool Stop()
        {
            SendList.Clear();
            return UnhookWindowsHookEx(this.hookHandle);
        }

        public void SendPoint(MessageEnum m, int x, int y)
        {
            this.SendList.ForEach(h => SendMessage(h, (int)m, x, y));
        }

        public void Register(IntPtr clienthandle)
        {
            var desktop = GetDesktopWindow();
            var handle = GetWindow(desktop, GW_CHILD);
            while (handle != IntPtr.Zero)
            {
                SendMessage(handle, (int)MessageEnum.Register, (int)clienthandle, 0);
                handle = GetWindow(handle, GW_HWNDNEXT);
            }
        }

        public void Register(IntPtr serverhandle, IntPtr clienthandle)
        {
            SendMessage(serverhandle, (int)MessageEnum.Register, (int)clienthandle, 0);
        }

        public IntPtr FindHandle(string currenttittlename)
        {
            return FindWindow(null, currenttittlename);
        }

        private int hookproc(int code, IntPtr wparam, ref CWPSTRUCT cwp)
        {
            if (code == 0)
            {
                switch (cwp.message)
                {
                    case (int)MessageEnum.Register:
                        {
                            var value = (IntPtr)cwp.wparam;
                            if (!this.SendList.Contains(value))
                            {
                                this.SendList.Add(value);
                            }
                            break;
                        }
                }
            }
            return CallNextHookEx(hookHandle, code, wparam, ref cwp);
        }

    }
}
