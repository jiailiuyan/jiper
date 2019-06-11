/* 迹I柳燕
 *
 * FileName:   WindowRendering.cs
 * Version:    1.0
 * Date:       2018/10/26 15:36:49
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders
 * @class      WindowRendering
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using Ji.CommonHelper.WPF.Helpers;

namespace Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders
{
    /// <summary>
    /// </summary>
    [ContentProperty("ChildView")]
    public class WindowRendering : Window
    {
        private Grid MainPanel { get; set; } = new Grid();
        protected  Border ChildPanel { get; set; } = new Border();
        protected WindowTitle TitlePanel { get; set; } = new WindowTitle();

        public bool CanMove
        {
            get { return (bool)GetValue(CanMoveProperty); }
            set { SetValue(CanMoveProperty, value); }
        }

        public static readonly DependencyProperty CanMoveProperty =
            DependencyProperty.Register("CanMove", typeof(bool), typeof(WindowRendering), new PropertyMetadata(true));

        public bool CanMinimize
        {
            get { return (bool)GetValue(CanMinimizeProperty); }
            set { SetValue(CanMinimizeProperty, value); }
        }

        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.Register("CanMinimize", typeof(bool), typeof(WindowRendering), new PropertyMetadata(false));

        public bool CanMax
        {
            get { return (bool)GetValue(CanMaxProperty); }
            set { SetValue(CanMaxProperty, value); }
        }

        public static readonly DependencyProperty CanMaxProperty =
            DependencyProperty.Register("CanMax", typeof(bool), typeof(WindowRendering), new PropertyMetadata(true));

        protected virtual bool HaveTitle { get; } = true;

        /// <summary>
        /// 获取或设置
        /// </summary>
        public object ChildView
        {
            get { return (object)GetValue(ChildViewProperty); }
            set { SetValue(ChildViewProperty, value); }
        }

        /// <summary>
        /// 注册 ChildView 依赖属性
        /// </summary>
        public static readonly DependencyProperty ChildViewProperty =
            DependencyProperty.Register("ChildView", typeof(object), typeof(WindowRendering), new PropertyMetadata(null, ChildViewChangedCallback));

        private static void ChildViewChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as WindowRendering);
            if (control != null)
            {
                control.ChildPanel.Child = e.NewValue as UIElement;
            }
        }

        public WindowRendering()
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
            //this.Topmost = true;

            this.MainPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            this.MainPanel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            this.MainPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.MainPanel.VerticalAlignment = VerticalAlignment.Stretch;
            this.MainPanel.Background = Brushes.Transparent;

            if (HaveTitle)
            {
                this.TitlePanel.Height = 30;
                this.TitlePanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.TitlePanel.VerticalAlignment = VerticalAlignment.Stretch;
                this.TitlePanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4CB0E4"));
                this.TitlePanel.SetBinding(WindowTitle.TitleProperty, new Binding("Title") { Source = this });
                this.TitlePanel.SetBinding(WindowTitle.CanMinimizeProperty, new Binding("CanMinimize") { Source = this });
                this.TitlePanel.SetBinding(WindowTitle.CanMaxProperty, new Binding("CanMax") { Source = this });

                this.TitlePanel.OnDragMove += this.TitlePanel_OnDragMove;
                this.TitlePanel.OnClosed += this.TitlePanel_OnClosed;
                this.TitlePanel.OnMaxed += this.TitlePanel_OnMaxed;
                this.TitlePanel.OnNormaled += this.TitlePanel_OnNormaled;
                this.TitlePanel.OnMinimize += this.TitlePanel_OnMinimize;

                Grid.SetRow(this.TitlePanel, 0);
                this.MainPanel.Children.Add(this.TitlePanel);

                this.TitlePanel.SetIsMax(false);
            }

            this.ChildPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.ChildPanel.VerticalAlignment = VerticalAlignment.Stretch;
            this.ChildPanel.Background = Brushes.Transparent;

            Grid.SetRow(this.ChildPanel, 1);
            this.MainPanel.Children.Add(this.ChildPanel);

            this.Content = this.MainPanel;
        }

        #region 标题操作

        private void TitlePanel_OnClosed()
        {
            this.Close();
        }

        private void TitlePanel_OnMinimize()
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitlePanel_OnDragMove()
        {
            if (CanMove)
            {
                this.DragMove();
            }
        }

        private void TitlePanel_OnNormaled()
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
                this.TitlePanel.SetIsMax(false);
            }
        }

        private void TitlePanel_OnMaxed()
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                this.TitlePanel.SetIsMax(true);
            }
        }

        #endregion 标题操作

        #region 置顶操作

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //if (!(VSHelper.IsDesingMode || VSHelper.IsVSMode))
            //{
            //    HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(new HwndSourceHook(WndProc));
            //}
        }

        /// <summary>
        /// 对于被MessageBox类覆盖时的置顶处理
        /// </summary>
        private bool enable = false;

        /// <summary>
        /// 对于焦点被切换时的指定处理
        /// </summary>
        private bool active = false;

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //窗口置顶事件
            switch (msg)
            {
                //是否可用
                case 0xA:
                    {
                        if ((int)wParam == 0)
                        {
                            if (!enable)
                            {
                                this.Topmost = false;
                                enable = true;
                            }
                        }
                        else if ((int)wParam == 1)
                        {
                            if (enable)
                            {
                                this.Topmost = true;
                                enable = false;
                            }
                        }
                        break;
                    }

                //是否激活
                case 0x1C:
                    {
                        if ((int)wParam == 0)
                        {
                            if (!active)
                            {
                                this.Topmost = false;
                                active = true;
                            }
                        }
                        else if ((int)wParam == 1)
                        {
                            if (active)
                            {
                                if (!enable)
                                {
                                    this.Topmost = true;
                                }
                                active = false;
                            }
                        }
                        break;
                    }

                default: break;
            }

            return IntPtr.Zero;
        }

        #endregion 置顶操作

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (newContent == null)
            {
                this.ChildPanel.Child = null;
            }
            else if (newContent != this.MainPanel)
            {
                if (base.Content != this.MainPanel)
                {
                    base.Content = this.MainPanel;
                }

                var cur = newContent as FrameworkElement;
                this.ChildPanel.Child = cur;
            }
        }
    }
}