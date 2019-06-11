using Microsoft.Windows.Shell;
using System.Windows;

namespace HeaderWindow
{
    internal class MinimizeButton : CaptionButton
    {
        static MinimizeButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimizeButton), new FrameworkPropertyMetadata(typeof(MinimizeButton)));
        }

        protected override void OnClick()
        {
            base.OnClick();
            SystemCommands.MinimizeWindow(Window.GetWindow(this));
        }
    }
}
