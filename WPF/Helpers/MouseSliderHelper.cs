using System;
using System.Windows;
using System.Windows.Input;

namespace Ji.WPFHelper.ControlHelper
{
    public enum ActionType
    {
        Up,
        Down,
        Left,
        Right
    }

    public class MouseType : EventArgs
    {
        public ActionType Type { get; set; }
        public MouseEventArgs MouseArgs { get; set; }

        public MouseType(ActionType type)
        {
            this.Type = type;
        }
    }

    public class MouseSliderHelper
    {
        /// <summary> 位移的长度判断是否执行左右或者上下滑动 </summary>
        public double SpanValue = 100;

        /// <summary> 标识是否执行垂直检查 </summary>
        public bool IsVerctor = true;

        public event EventHandler<MouseType> MouseAction;

        private FrameworkElement Element = null;

        private Point? MousePoint = null;

        public MouseSliderHelper(FrameworkElement element)
        {
            this.Element = element;
            this.Element.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;
            this.Element.PreviewMouseMove += Element_PreviewMouseMove;
            this.Element.PreviewMouseLeftButtonUp += Element_PreviewMouseLeftButtonUp;

            this.Element.PreviewTouchDown += Element_PreviewTouchDown;
            this.Element.PreviewTouchMove += Element_PreviewTouchMove;
            this.Element.PreviewTouchUp += Element_PreviewTouchUp;
        }

        private void Element_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            MousePoint = e.GetTouchPoint(this.Element).Position;
        }

        private void Element_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (MousePoint != null)
            {
                var point = e.GetTouchPoint(this.Element).Position;
                Move(point);
            }
        }

        private void Element_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            MousePoint = null;
        }

        private void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MousePoint = e.GetPosition(this.Element);
        }

        private void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (MousePoint != null)
            {
                var point = e.GetPosition(this.Element);
                Move(point);
            }
        }

        private void Element_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MousePoint = null;
        }

        private void Move(Point point)
        {
            var spanx = point.X - MousePoint.Value.X;
            var spany = point.Y - MousePoint.Value.Y;

            // x < y 时 垂直位移大于水平位移，判断为上下滑动
            if (Math.Abs(spanx) < Math.Abs(spany) && IsVerctor)
            {
                //移动位置小于初始位置，判断为向上滑动
                if (spany < 0)
                {
                    if (Math.Abs(spany) > SpanValue)
                    {
                        //先取消鼠标Move继续执行，然后再执行滑动操作
                        MousePoint = null;
                        MouseAction(this, new MouseType(ActionType.Up));
                    }
                }
                //反之向下滑动
                else
                {
                    if (Math.Abs(spany) > SpanValue)
                    {
                        //先取消鼠标Move继续执行，然后再执行滑动操作
                        MousePoint = null;
                        MouseAction(this, new MouseType(ActionType.Down));
                    }
                }
            }
            //反之，水平位移
            else
            {
                //移动位置小于初始位置，判断为向左滑动
                if (spanx < 0)
                {
                    if (Math.Abs(spanx) > SpanValue)
                    {
                        //先取消鼠标Move继续执行，然后再执行滑动操作
                        MousePoint = null;
                        MouseAction(this, new MouseType(ActionType.Left));
                    }
                }
                //反之向右滑动
                else
                {
                    if (Math.Abs(spanx) > SpanValue)
                    {
                        //先取消鼠标Move继续执行，然后再执行滑动操作
                        MousePoint = null;
                        MouseAction(this, new MouseType(ActionType.Right));
                    }
                }
            }
        }
    }
}