using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.System
{
    class WindowsMessageHelper
    {
        const int WM_COPYDATA = 0x004A;
        const int GW_CHILD = 0x5;
        const int GW_HWNDNEXT = 0x2;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var desktop = GetDesktopWindow();
            var handle = GetWindow(desktop, GW_CHILD);
            while (handle != IntPtr.Zero)
            {
                SendMessageing(handle);
                handle = GetWindow(handle, GW_HWNDNEXT);
            }
        }

        private void SendMessageing(IntPtr WINDOW_HANDLER)
        {
            if (WINDOW_HANDLER != IntPtr.Zero)
            {
                //byte[] sarr = System.Text.Encoding.Default.GetBytes(this.textBox1.Text);
                //int len = sarr.Length;
                //COPYDATASTRUCT cds;
                //cds.dwData = (IntPtr)100;
                //cds.lpData = this.textBox1.Text;
                //cds.cbData = len + 1;
                //SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref cds);
            }
        }



        //protected override void DefWndProc(ref System.Windows.Forms.Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case WM_COPYDATA:
        //            COPYDATASTRUCT mystr = new COPYDATASTRUCT();
        //            Type mytype = mystr.GetType();
        //            mystr = (COPYDATASTRUCT)m.GetLParam(mytype);
        //            this.textBox1.Text = mystr.lpData;
        //            break;
        //        default:
        //            base.DefWndProc(ref m);
        //            break;
        //    }
        //}
    }
}
