using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.System
{
    class WindowsSettingHelper
    {
        [DllImport("user32.dll")]
        static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int width, int height, uint flags);
        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    var hwnd = new WindowInteropHelper(this).Handle;
        //    SetWindowPos(hwnd, IntPtr.Zero, 0, 0, (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, 0x40);
        //}
    }
}
