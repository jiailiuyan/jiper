using Microsoft.Windows.Shell;
using System.Windows;

namespace HeaderWindow
{
    internal class MaximizeButton : CaptionButton
    {
        static MaximizeButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaximizeButton), new FrameworkPropertyMetadata(typeof(MaximizeButton)));
        }

        protected override void OnClick()
        {
            base.OnClick();
            Window w = Window.GetWindow(this);
            if (w.WindowState == System.Windows.WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(w);
            }
            else
            {
                SystemCommands.MaximizeWindow(w);
            }
        }
    }
}
