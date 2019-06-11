using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ji.SystemHelper
{
    public static class ScreenHelper
    {
        public static void SetViewToDefaultScreen(this Window window, bool isfullscreent = true)
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 0)
            {
                var first = screens.FirstOrDefault(i => i.Primary);
                if (isfullscreent)
                {
                    window.Left = first.WorkingArea.Left;
                    window.Top = first.WorkingArea.Top;

                    window.Width = first.Bounds.Width;
                    window.Height = first.Bounds.Height;
                }
                else
                {
                    // Ji 查询当前程序窗口所在屏幕
                    // Ps 极端情况下检测不到当前程序所在，因此判断
                    var screen = screens.FirstOrDefault(i => i.Bounds.Contains((int)window.Left, (int)window.Top));
                    if (screen != null)
                    {
                        var left = (int)window.Left - screen.Bounds.Left;
                        var top = (int)window.Top - screen.Bounds.Top;

                        window.Left = left;
                        window.Top = top;
                    }
                }
#if !Debug
                window.Topmost = true;
#endif
            }
        }

        public static void SetViewToSecondScreen(this Window window)
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 1)
            {
                var second = screens.FirstOrDefault(i => !i.Primary);

                window.Left = second.WorkingArea.Left;
                window.Top = second.WorkingArea.Top;

                //不覆盖工具栏
                //window.Width = second.WorkingArea.Width;
                //window.Height = second.WorkingArea.Height;

                window.Width = second.Bounds.Width;
                window.Height = second.Bounds.Height;

#if !Debug
                window.Topmost = true;
#endif
            }
            else if (screens.Length == 1)
            {
                var first = screens[0];
                window.Left = first.WorkingArea.Left;
                window.Top = first.WorkingArea.Top;

                window.Width = first.Bounds.Width;
                window.Height = first.Bounds.Height;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}