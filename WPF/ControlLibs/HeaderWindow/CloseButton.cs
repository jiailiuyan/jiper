using System.Windows;

namespace HeaderWindow
{
    internal class CloseButton : CaptionButton
    {
        static CloseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseButton), new FrameworkPropertyMetadata(typeof(CloseButton)));
        }

    }
}
