using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ji.WPFHelper.ControlLibs
{
    public class DrawSelectedCanvas : Canvas
    {
        public Rect CurrentRect;

        public int ItemWidth
        {
            get { return (int)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(int), typeof(DrawSelectedCanvas), new PropertyMetadata(65));

        public int ItemHeight
        {
            get { return (int)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(int), typeof(DrawSelectedCanvas), new PropertyMetadata(65));

        public Brush StartBrush
        {
            get { return (Brush)GetValue(StartBrushProperty); }
            set { SetValue(StartBrushProperty, value); }
        }

        public static readonly DependencyProperty StartBrushProperty =
            DependencyProperty.Register("StartBrush", typeof(Brush), typeof(DrawSelectedCanvas), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6BFF0000"))));

        public bool IsSelfDraw
        {
            get { return (bool)GetValue(IsSelfDrawProperty); }
            set { SetValue(IsSelfDrawProperty, value); }
        }

        public static readonly DependencyProperty IsSelfDrawProperty =
            DependencyProperty.Register("IsSelfDraw", typeof(bool), typeof(DrawSelectedCanvas), new PropertyMetadata(false));

        public bool IsDrawOut
        {
            get { return (bool)GetValue(IsDrawOutProperty); }
            set { SetValue(IsDrawOutProperty, value); }
        }

        public static readonly DependencyProperty IsDrawOutProperty =
            DependencyProperty.Register("IsDrawOut", typeof(bool), typeof(DrawSelectedCanvas), new PropertyMetadata(true));

        private Rectangle CenterControl = new Rectangle() { IsHitTestVisible = false, Opacity = 0.5 };

        public Rect SelectRect
        {
            get { return new Rect(Canvas.GetLeft(CenterControl), Canvas.GetTop(CenterControl), CenterControl.ActualWidth, CenterControl.ActualHeight); }
        }

        public event EventHandler EndDraw;

        public DrawSelectedCanvas()
        {
            this.Children.Add(CenterControl);
        }

        private Point? MousePoint = null;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (IsSelfDraw)
            {
                var mousepoint = e.GetPosition(this);
                MousePoint = mousepoint;
                this.Start(MousePoint.Value);
            }
        }

        protected override void OnPreviewMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            Move(e.GetPosition(this));
        }

        protected override void OnPreviewMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (startPoint != null)
            {
                if (startPoint.Value.Equals(e.GetPosition(this)))
                {
                    ResetShow();
                }
                else
                {
                    if (EndDraw != null)
                    {
                        EndDraw(null, EventArgs.Empty);
                    }
                }
            }
            End();

            this.ReleaseMouseCapture();
        }

        private Point? startPoint;

        public void Start(Point point)
        {
            startPoint = point;
            this.BringToFront(CenterControl);

            this.CaptureMouse();
        }

        private void Move(Point point)
        {
            if (startPoint != null)
            {
                if (!IsDrawOut)
                {
                    point.X = point.X < 0 ? 0 : point.X;
                    point.Y = point.Y < 0 ? 0 : point.Y;
                    point.X = point.X > this.Width ? this.Width : point.X;
                    point.Y = point.Y > this.Height ? this.Height : point.Y;
                }

                this.CenterControl.Fill = StartBrush;
                if (startPoint.Value.X <= point.X && startPoint.Value.Y <= point.Y)
                {
                    SetShowPosition(startPoint.Value.X, startPoint.Value.Y, this.Width - point.X, this.Height - point.Y);
                }
                else if (startPoint.Value.X <= point.X && startPoint.Value.Y >= point.Y)
                {
                    SetShowPosition(startPoint.Value.X, point.Y, this.Width - point.X, this.Height - startPoint.Value.Y);
                }
                else if (startPoint.Value.X >= point.X && startPoint.Value.Y >= point.Y)
                {
                    SetShowPosition(point.X, point.Y, this.Width - startPoint.Value.X, this.Height - startPoint.Value.Y);
                }
                else if (startPoint.Value.X >= point.X && startPoint.Value.Y <= point.Y)
                {
                    SetShowPosition(point.X, startPoint.Value.Y, this.Width - startPoint.Value.X, this.Height - point.Y);
                }
            }
        }

        private void SetShowPosition(double lw, double th, double rw, double bh)
        {
            this.CenterControl.Width = this.Width - rw - lw;
            this.CenterControl.Height = this.Height - bh - th;

            Canvas.SetLeft(this.CenterControl, lw);
            Canvas.SetTop(this.CenterControl, th);

            CurrentRect = new Rect(lw, th, this.CenterControl.Width, this.CenterControl.Height);
        }

        public void End()
        {
            startPoint = null;
            this.CenterControl.Fill = null;
        }

        private void ResetShow()
        {
            this.CenterControl.Width = 0;
            this.CenterControl.Height = 0;
        }

        #region UpdateZOrder

        public void BringToFront(UIElement element)
        {
            this.UpdateZOrder(element, true);
        }

        public void SendToBack(UIElement element)
        {
            this.UpdateZOrder(element, false);
        }

        private void UpdateZOrder(UIElement element, bool bringToFront)
        {
            if (element == null || !base.Children.Contains(element))
            {
                return;
            }

            int elementNewZIndex = -1;
            if (bringToFront)
            {
                foreach (UIElement elem in base.Children)
                {
                    if (elem.Visibility != Visibility.Collapsed)
                    {
                        ++elementNewZIndex;
                    }
                }
            }
            else
            {
                elementNewZIndex = 0;
            }

            int offset = (elementNewZIndex == 0) ? +1 : -1;
            int elementCurrentZIndex = Canvas.GetZIndex(element);
            foreach (UIElement childElement in base.Children)
            {
                if (childElement == element)
                    Canvas.SetZIndex(element, elementNewZIndex);
                else
                {
                    int zIndex = Canvas.GetZIndex(childElement);
                    if (bringToFront && elementCurrentZIndex < zIndex ||
                        !bringToFront && zIndex < elementCurrentZIndex)
                    {
                        Canvas.SetZIndex(childElement, zIndex + offset);
                    }
                }
            }
        }

        #endregion UpdateZOrder
    }
}