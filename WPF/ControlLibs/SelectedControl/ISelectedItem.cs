using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SelectedControl
{
    public interface ISelectedItem
    {
        bool IsSelected { get; set; }

        bool CanVerticalMove { get; }
        bool CanHorizontalMove { get; }

        bool HitTestPoint(UIElement rootElement, Point point);

        bool HitTestRect(UIElement rootElement, Rect rect);

        Rect GetViewRect(UIElement rootElement);

        void MouseDownPosition(Point point);

        void MouseMovePosition(Point defferpoint);

        void MouseUpPosition(Point defferpoint);

    }
}
