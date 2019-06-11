using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelectedControl
{
    /// <summary>
    /// SelectedControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectedControl : UserControl
    {
        public ObservableCollection<ISelectedItem> Items { get; set; }
        public UIElement RootElement { get; set; }

        public ObservableCollection<ISelectedItem> SelectedItems { get; private set; }

        public Point? MousePoint { get; set; }

        public Point? LastMousePoint { get; set; }

        private IInputElement MouseCapture;

        public bool ShowSelectBorder
        {
            get { return (bool)GetValue(ShowSelectBorderProperty); }
            set { SetValue(ShowSelectBorderProperty, value); }
        }
        public static readonly DependencyProperty ShowSelectBorderProperty =
            DependencyProperty.Register("ShowSelectBorder", typeof(bool), typeof(SelectedControl), new PropertyMetadata(true));

        public bool ShowMoveBorder
        {
            get { return (bool)GetValue(ShowMoveBorderProperty); }
            set { SetValue(ShowMoveBorderProperty, value); }
        }
        public static readonly DependencyProperty ShowMoveBorderProperty =
            DependencyProperty.Register("ShowMoveBorder", typeof(bool), typeof(SelectedControl), new PropertyMetadata(true));

        public event EventHandler<SelectedArgs> SelectedChanged;
        protected void SelctedChanging(ObservableCollection<ISelectedItem> items, SelctedType type = SelctedType.PointSelected)
        {
            if (this.SelectedChanged != null)
            {
                this.SelectedChanged(this, new SelectedArgs(items, type));
            }
        }

        public SelectedControl()
        {
            this.SelectedItems = new ObservableCollection<ISelectedItem>();
            this.SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
            //this.IsHitTestVisible = false;
            InitializeComponent();
        }

        void SelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null)
                        {
                            foreach (ISelectedItem item in e.NewItems)
                            {
                                var border = new Border() { Tag = item, BorderBrush = Brushes.Red, BorderThickness = new Thickness(2) };
                                if (!ShowSelectBorder)
                                {
                                    border.Visibility = System.Windows.Visibility.Collapsed;
                                }

                                this.maincontrol.Children.Add(border);
                                var rect = item.GetViewRect(this.RootElement);
                                border.Width = rect.Width;
                                border.Height = rect.Height;
                                Canvas.SetLeft(border, rect.Left);
                                Canvas.SetTop(border, rect.Top);
                            }
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null)
                        {
                            foreach (ISelectedItem item in e.OldItems)
                            {
                                var i = this.maincontrol.Children.Cast<Border>().FirstOrDefault(s => s.Tag == item);
                                this.maincontrol.Children.Remove(i);
                            }
                        }
                        break;
                    }

                case NotifyCollectionChangedAction.Reset:
                    {
                        this.maincontrol.Children.Clear();
                        break;
                    }
            }
        }

        public void InitHit(UIElement rootElement, ObservableCollection<ISelectedItem> items)
        {
            this.Items = items;
            this.SelectedItems.Clear();

            if (this.RootElement != null)
            {
                this.RootElement.RemoveHandler(UIElement.PreviewMouseDownEvent, new MouseButtonEventHandler(RootElement_PreviewMouseDown));
                this.RootElement.RemoveHandler(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(RootElement_PreviewMouseMove));
                this.RootElement.RemoveHandler(UIElement.PreviewMouseUpEvent, new MouseButtonEventHandler(RootElement_PreviewMouseUp));

                this.RootElement.RemoveHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(RootElement_MouseDown));
                this.RootElement.RemoveHandler(UIElement.MouseMoveEvent, new MouseEventHandler(RootElement_MouseMove));
                this.RootElement.RemoveHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(RootElement_MouseUp));
            }

            this.RootElement = rootElement;

            this.RootElement.AddHandler(UIElement.PreviewMouseDownEvent, new MouseButtonEventHandler(RootElement_PreviewMouseDown), true);
            this.RootElement.AddHandler(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(RootElement_PreviewMouseMove), true);
            this.RootElement.AddHandler(UIElement.PreviewMouseUpEvent, new MouseButtonEventHandler(RootElement_PreviewMouseUp), true);

            this.RootElement.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(RootElement_MouseDown), true);
            this.RootElement.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(RootElement_MouseMove), true);
            this.RootElement.AddHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(RootElement_MouseUp), true);
        }

        public void SetSelcteds(List<ISelectedItem> selecteds)
        {
            int sc = this.SelectedItems.Count - 1;
            for (int i = sc; i >= 0; i--)
            {
                var item = this.SelectedItems[i];
                if (!selecteds.Contains(item))
                {
                    item.IsSelected = false;
                    this.SelectedItems.Remove(item);
                }
            }

            foreach (var item in selecteds)
            {
                if (!this.SelectedItems.Contains(item))
                {
                    item.IsSelected = true;
                    this.SelectedItems.Add(item);
                }
            }
        }

        void RootElement_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            List<ISelectedItem> selecteds = new List<ISelectedItem>();
            var p = e.GetPosition(sender as IInputElement);

            //忽略重复选中
            var ishitselected = this.SelectedItems.FirstOrDefault(i => i.HitTestPoint(this.RootElement, p));
            if (ishitselected == null)
            {
                SelectItemsHitPointTest(p, selecteds);
                SetSelcteds(selecteds);
            }

            if (ishitselected == null && selecteds.Count == 0)
            {
                this.SelectedItems.Clear();
            }

            SelctedChanging(this.SelectedItems);

            if (this.SelectedItems.Count > 0)
            {
                MouseCapture = Mouse.Captured;
                Mouse.Capture(sender as IInputElement);
                LastMousePoint = MousePoint = p;

                foreach (var item in this.SelectedItems)
                {
                    item.MouseDownPosition(p);
                }
            }
        }

        void RootElement_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (MousePoint != null)
            {
                var newpoint = e.GetPosition(sender as IInputElement);
                var defferpoint = new Point(MousePoint.Value.X - newpoint.X, MousePoint.Value.Y - newpoint.Y);
                if (!(defferpoint.X == 0 && defferpoint.Y == 0))
                {
                    foreach (var item in this.SelectedItems)
                    {
                        if (ShowMoveBorder)
                        {
                            var i = this.maincontrol.Children.Cast<Border>().FirstOrDefault(s => s.Tag == item);
                            if (!ShowSelectBorder)
                            {
                                i.Visibility = System.Windows.Visibility.Visible;
                            }

                            if (item.CanHorizontalMove)
                            {
                                var x = Canvas.GetLeft(i) - defferpoint.X;
                                Canvas.SetLeft(i, x);
                            }

                            if (item.CanVerticalMove)
                            {
                                var y = Canvas.GetTop(i) - defferpoint.Y;
                                Canvas.SetTop(i, y);
                            }
                        }
                        else
                        {
                            item.MouseMovePosition(defferpoint);
                        }
                    }
                    MousePoint = newpoint;
                }
            }
        }

        void RootElement_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MousePoint != null)
            {
                var newpoint = e.GetPosition(sender as IInputElement);
                var defferpoint = new Point(LastMousePoint.Value.X - newpoint.X, LastMousePoint.Value.Y - newpoint.Y);
                if (!(defferpoint.X == 0 && defferpoint.Y == 0))
                {
                    foreach (var item in this.SelectedItems)
                    {
                        item.MouseUpPosition(defferpoint);

                        var i = this.maincontrol.Children.Cast<Border>().FirstOrDefault(s => s.Tag == item);
                        if (!ShowSelectBorder)
                        {
                            i.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                }

                MousePoint = null;
                Mouse.Capture(null);
            }

            LastMousePoint = null;
        }

        void RootElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        void RootElement_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
        void RootElement_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void SelectItemsHitPointTest(Point point, List<ISelectedItem> selecteds)
        {
            foreach (var item in this.Items)
            {
                if (item.HitTestPoint(RootElement, point))
                {
                    selecteds.Add(item);
                }
            }
        }

        private void SelectItemsHitRectTest(Rect rect, List<ISelectedItem> selecteds)
        {
            foreach (var item in this.Items)
            {
                if (item.HitTestRect(RootElement, rect))
                {
                    selecteds.Add(item);
                }
            }
        }

    }
}
