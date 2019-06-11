using System;
using System.Collections.Generic;
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

namespace Ksy.Client.CommonHelper.WPF.ControlLibs.WindowRenders
{
    /// <summary>
    /// WindowTitle.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTitle : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WindowTitle), new PropertyMetadata(null));

        public bool CanMinimize
        {
            get { return (bool)GetValue(CanMinimizeProperty); }
            set { SetValue(CanMinimizeProperty, value); }
        }

        /// <summary> 注册 CanMin 依赖属性 </summary>
        public static readonly DependencyProperty CanMinimizeProperty =
            DependencyProperty.Register("CanMinimize", typeof(bool), typeof(WindowTitle), new PropertyMetadata(false, CanMinChangedCallback));

        private static void CanMinChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as WindowTitle);
            if (control != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                    {
                        control.min_btn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        control.min_btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }


        /// <summary> 获取或设置 </summary>
        public bool CanMax
        {
            get { return (bool)GetValue(CanMaxProperty); }
            set { SetValue(CanMaxProperty, value); }
        }

        /// <summary> 注册 CanMax 依赖属性 </summary>
        public static readonly DependencyProperty CanMaxProperty =
            DependencyProperty.Register("CanMax", typeof(bool), typeof(WindowTitle), new PropertyMetadata(true, CanMaxChangedCallback));

        private static void CanMaxChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as WindowTitle);
            if (control != null)
            {
                if (e.NewValue is bool)
                {
                    if ((bool)e.NewValue)
                    {
                        control.SetIsMax(false);
                    }
                    else
                    {
                        control.SetNoMax();
                    }
                }
            }
        }

        public event Action OnClosed { add { this.close_btn.OnClick += value; } remove { this.close_btn.OnClick -= value; } }

        public event Action OnMaxed { add { this.max_btn.OnClick += value; } remove { this.max_btn.OnClick -= value; } }

        public event Action OnNormaled { add { this.normal_btn.OnClick += value; } remove { this.normal_btn.OnClick -= value; } }

        public event Action OnMinimize { add { this.min_btn.OnClick += value; } remove { this.min_btn.OnClick -= value; } }

        public event Action OnDragMove;

        public WindowTitle()
        {
            InitializeComponent();
        }

        public void SetNoMax()
        {
            this.normal_btn.Visibility = Visibility.Collapsed;
            this.max_btn.Visibility = Visibility.Collapsed;
        }

        #region Title DragMove

        private Point? MLP = null;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.MLP != null && e.LeftButton == MouseButtonState.Pressed)
            {
                this.OnDragMove?.Invoke();
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.MLP = e.GetPosition(this);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.MLP = null;
        }

        #endregion

        public void SetIsMax(bool ismax)
        {
            if (ismax)
            {
                this.max_btn.Visibility = Visibility.Collapsed;
                this.normal_btn.Visibility = Visibility.Visible;
            }
            else
            {
                this.max_btn.Visibility = Visibility.Visible;
                this.normal_btn.Visibility = Visibility.Collapsed;
            }
        }

    }
}
