using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ji.WPFHelper.ControlHelper;

namespace Ji.WPFHelper.ControlLibs
{
    public class CustomTreeViewItem : TreeViewItem
    {
        #region 展开控制的显示图片

        /// <summary> 展开时公司高亮图标 </summary>
        public static Brush IconLightExpandCompany = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/companyover.png", UriKind.RelativeOrAbsolute)) };

        /// <summary> 展开时公司图标 </summary>
        public static Brush IconCompany = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/company.png", UriKind.RelativeOrAbsolute)) };

        /// <summary> 展开时部门高亮图标 </summary>
        public static Brush IconLightExpandDepartment = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/unexpandover.png", UriKind.RelativeOrAbsolute)) };

        /// <summary> 展开时部门图标 </summary>
        public static Brush IconExpandDepartment = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/unexpand.png", UriKind.RelativeOrAbsolute)) };

        /// <summary> 折叠时部门高亮图标 </summary>
        public static Brush IconLightDepartment = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/expandover.png", UriKind.RelativeOrAbsolute)) };

        /// <summary> 折叠时部门图标 </summary>
        public static Brush IconDepartment = new ImageBrush { Stretch = Stretch.Fill, ImageSource = new BitmapImage(new Uri("pack://application:,,,/WPFUserControl;component/Images/expand.png", UriKind.RelativeOrAbsolute)) };

        #endregion 展开控制的显示图片

        ///// <summary> 当前绑定的数据 </summary>
        //private Common.DepartmentNodeItem Data { get; set; }

        /// <summary> 是否为公司 </summary>
        private bool IsCompany = false;

        #region 模版控件

        /// <summary> 当前 TreeViewItem 的顶级父级 TreeView </summary>
        protected TreeView treeview = null;

        /// <summary> 当前 TreeViewItem 的显示子集容器 </summary>
        protected ItemsPresenter ItemsPresenter;

        /// <summary> TreeViewItem 的模版顶级控件 </summary>
        protected Grid StyleMainGrid = null;

        /// <summary> TreeViewItem 的选中背景设置控件 </summary>
        protected Border StyleBorder = null;

        /// <summary> TreeViewItem 的选中前景设置控件 </summary>
        protected Border StyleForgroundBorder = null;

        /// <summary>
        /// 控制展开折叠的 ToggleButton 控件
        /// </summary>
        protected ToggleButton ExpandToggleButton = null;

        /// <summary>
        /// 设置当前控制的显示的 ToggleButton 背景，设置 Background
        /// </summary>
        protected Border ToggleButtonBoreder = null;

        /// <summary>
        /// 显示标题的 Border
        /// </summary>
        protected Border HeaderBorder = null;

        #endregion 模版控件

        /// <summary> 前景变换色 </summary>
        protected static LinearGradientBrush OverBrush = new LinearGradientBrush()
        {
            StartPoint = new Point(0.5, 0),
            EndPoint = new Point(0.5, 1),
            GradientStops = new GradientStopCollection()
            {
                new GradientStop(Colors.White,0),
                 new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFD2F1FA"),1)
            }
        };

        /// <summary> 前景边框色 </summary>
        protected static Brush OverBorderBrush = "#FFBAE1EC".ConvertToBrush();

        /// <summary> 选中边框色 </summary>
        protected static Brush SelectBorderBrush = "#FF48B0CE".ConvertToBrush();

        /// <summary> 设置展开子项时的动画持续时间，单位：毫秒 </summary>
        public double TottleViewTime
        {
            get { return (double)GetValue(TottleViewTimeProperty); }
            set { SetValue(TottleViewTimeProperty, value); }
        }

        public static readonly DependencyProperty TottleViewTimeProperty = DependencyProperty.Register("TottleViewTime", typeof(double), typeof(CustomTreeViewItem), new PropertyMetadata(1000d));

        /// <summary> 设置选中时的背景色 </summary>
        public Brush SelectedBackBrush
        {
            get { return (Brush)GetValue(SelectedBackBrushProperty); }
            set { SetValue(SelectedBackBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackBrushProperty =
            DependencyProperty.Register("SelectedBackBrush", typeof(Brush), typeof(CustomTreeViewItem), new PropertyMetadata("#FFCAF2FC".ConvertToBrush()));

        /// <summary> 设置鼠标悬停时的前景色 </summary>
        public Brush SelectedForgroundBrush
        {
            get { return (Brush)GetValue(SelectedForgroundBrushProperty); }
            set { SetValue(SelectedForgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectedForgroundBrushProperty =
            DependencyProperty.Register("SelectedForgroundBrush", typeof(Brush), typeof(CustomTreeViewItem), new PropertyMetadata(OverBrush));

        public CustomTreeViewItem()
        {
            this.Expanded += CustomTreeViewItem_Expanded;
            this.Loaded += CustomTreeViewItem_Loaded;
            this.Selected += CustomTreeViewItem_Selected;
            this.Unselected += CustomTreeViewItem_Unselected;
            //设置每个TreeViewItem 相距边距
            this.Padding = new Thickness(2);
        }

        protected virtual void CustomTreeViewItem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemsPresenter = this.FindVisualChild<ItemsPresenter>();
            //Data = this.Tag as Common.DepartmentNodeItem;

            //IsCompany = Data.parentId.Equals(Guid.Empty);

            if (ItemsPresenter != null)
            {
                ItemsPresenter.Margin = new Thickness(-19, 0, 0, 0);
                //if (Data != null && Data.IsLastDepart)
                //{
                //    ItemsPresenter.Margin = new Thickness(-20 - 20 + 16 * (this.GetDepth()), 0, 0, 0);
                //}
            }
        }

        private void CustomTreeViewItem_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            //保证动画加载点击的项上
            if (IsHitCurrentControl())
            {
                this.ItemsPresenter.ClipToBounds = true;
                if (this.ItemsPresenter != null)
                {
                    //if (Data != null && Data.IsLastDepart)
                    //{
                    //    //设置最后一项为人员时的动画
                    //    this.ItemsPresenter.SetMoveAnimationHoldIn(Data.listUser.Count, (TottleViewTime * (double)Data.listUser.Count) / 10000);
                    //}
                }
            }
        }

        #region 模版的样式处理

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Thickness v = new Thickness();
            v.Left = 12 * (this.GetDepth() - 1);

            if (ExpandToggleButton == null)
            {
                ExpandToggleButton = this.FindVisualChild<ToggleButton>();
                if (ExpandToggleButton != null)
                {
                    ExpandToggleButton.Loaded += toggleButton_Loaded;
                    Grid.SetColumn(ExpandToggleButton, 1);
                    v.Top = 4 + this.Padding.Top;
                    ExpandToggleButton.Margin = v;
                }
            }

            if (treeview == null)
            {
                treeview = this.FindVisualParent<TreeView>();
                if (treeview != null)
                {
                    treeview.PreviewMouseMove += treeview_PreviewMouseMove;
                    treeview.PreviewMouseLeftButtonDown += treeview_PreviewMouseLeftButtonDown;
                    treeview.PreviewMouseLeftButtonUp += treeview_PreviewMouseLeftButtonUp;
                }
            }

            var cp = this.FindVisualChild<ContentPresenter>();
            if (cp != null)
            {
                HeaderBorder = cp.FindVisualParent<Border>();
                v.Top = 0;
                v.Left = v.Left + 11;
                HeaderBorder.Margin = v;
            }

            InitStyleControls();
        }

        /// <summary> 添加设置背景与前景控件 </summary>
        protected virtual void InitStyleControls()
        {
            if (ExpandToggleButton != null)
            {
                StyleMainGrid = ExpandToggleButton.FindVisualParent<Grid>();
                if (StyleMainGrid != null)
                {
                    StyleForgroundBorder = new Border();
                    Grid.SetColumnSpan(StyleForgroundBorder, 3);
                    StyleForgroundBorder.Background = Brushes.Transparent;
                    StyleForgroundBorder.BorderBrush = OverBorderBrush;
                    StyleForgroundBorder.BorderThickness = new Thickness(0);
                    StyleMainGrid.Children.Insert(0, StyleForgroundBorder);

                    StyleBorder = new Border();
                    Grid.SetColumnSpan(StyleBorder, 3);
                    StyleBorder.Background = Brushes.Transparent;
                    StyleBorder.BorderBrush = SelectBorderBrush;
                    StyleBorder.BorderThickness = new Thickness(0);
                    StyleMainGrid.Children.Insert(0, StyleBorder);
                }
            }
        }

        private void toggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            //查找默认三角区域的容器
            ToggleButtonBoreder = ExpandToggleButton.FindVisualChild<Border>();
            if (ToggleButtonBoreder != null)
            {
                //需要设置所有父容器的对齐，因为设置为物理像素对齐后，默认对齐为，左 上
                ExpandToggleButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                ExpandToggleButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //手动设置居中
                //toggleButton.Margin = new Thickness(4, 4, 0, 0);
                //取消默认的Border的边框，使其可以手动计算位置
                ToggleButtonBoreder.Padding = new Thickness(0);
                ToggleButtonBoreder.BorderThickness = new Thickness(0);
                //设置图片显示的宽高，跟实际图片大小相同，保证图片正常显示
                ToggleButtonBoreder.Width = 9;
                ToggleButtonBoreder.Height = 9;

                ToggleButtonBoreder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                ToggleButtonBoreder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //取消默认的三角形显示
                ToggleButtonBoreder.Child = null;
                //设置新的背景
                SetMouseUnOverImage();
            }
        }

        #endregion 模版的样式处理

        #region 鼠标处理

        /// <summary> 判断鼠标是否在 TreeView 上按下 </summary>
        private bool ismousedown = false;

        protected virtual void treeview_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //保证动画加载点击的项上
            if (IsHitCurrentControl())
            {
                if (ToggleButtonBoreder != null)
                {
                    var mousepoint = Mouse.GetPosition(ToggleButtonBoreder);
                    if (mousepoint.X > 0 && mousepoint.Y > 0 && ToggleButtonBoreder.RenderSize.Height >= mousepoint.Y && ToggleButtonBoreder.RenderSize.Width >= mousepoint.X)
                    {
                        ismousedown = true;
                        SetMouseUnOverImage();
                    }
                }
            }
        }

        protected virtual void treeview_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (StyleForgroundBorder != null)
            {
                if (IsHitCurrentControl())
                {
                    StyleForgroundBorder.Background = SelectedForgroundBrush;
                    StyleForgroundBorder.BorderThickness = new Thickness(1);
                }
                else
                {
                    StyleForgroundBorder.Background = Brushes.Transparent;
                    StyleForgroundBorder.BorderThickness = new Thickness(0);
                }
            }

            if (ToggleButtonBoreder != null && !ismousedown)
            {
                //鼠标放到左边控制角
                var mousepoint = Mouse.GetPosition(ToggleButtonBoreder);
                if (mousepoint.X > 0 && mousepoint.Y > 0 && ToggleButtonBoreder.RenderSize.Height > mousepoint.Y && ToggleButtonBoreder.RenderSize.Width > mousepoint.X)
                {
                    SetMouseOverImage();
                }
                else
                {
                    SetMouseUnOverImage();
                }
            }
        }

        protected virtual void treeview_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ismousedown = false;
        }

        /// <summary> 鼠标放上去的时候 ToggleButton 显示处理 </summary>
        private void SetMouseUnOverImage()
        {
            //if (Data != null)
            {
                //有鼠标悬浮的时候，公司图标为高亮，部门图标由是否展开判断
                ToggleButtonBoreder.Background = IsCompany ? IconLightExpandCompany : (this.IsExpanded ? IconLightExpandDepartment : IconLightDepartment);
            }
        }

        /// <summary> 鼠标没有放上去的时候 ToggleButton 显示处理 </summary>
        private void SetMouseOverImage()
        {
            //if (Data != null)
            {
                //没有鼠标悬浮的时候，公司图标为非高亮，部门图标由是否展开判断
                ToggleButtonBoreder.Background = IsCompany ? IconCompany : (this.IsExpanded ? IconExpandDepartment : IconDepartment);
            }
        }

        #endregion 鼠标处理

        #region 选择处理

        /// <summary> 选中处理 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CustomTreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            if (IsHitCurrentControl())
            {
                SetSelect();
            }
        }

        /// <summary> 非选中处理 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CustomTreeViewItem_Unselected(object sender, RoutedEventArgs e)
        {
            SetUnSelect();
        }

        public virtual void SetSelect()
        {
            //还原默认的选中状态
            if (HeaderBorder != null)
            {
                //在此屏蔽默认触发器的Header的背景颜色
                HeaderBorder.Background = Brushes.Transparent;
                //触发器中设置显示文字颜色会根据选中状态改变，在此还原
                this.Foreground = Brushes.Black;

                StyleBorder.Background = SelectedBackBrush;
                StyleBorder.BorderThickness = new Thickness(1);
            }
        }

        public virtual void SetUnSelect()
        {
            StyleBorder.Background = Brushes.Transparent;
            StyleBorder.BorderThickness = new Thickness(0);
        }

        #endregion 选择处理

        /// <summary> 判断是否点中当前控件的 Header </summary>
        /// <returns></returns>
        public bool IsHitCurrentControl()
        {
            var point = System.Windows.Input.Mouse.GetPosition(this);
            var hitvisual = VisualTreeHelper.HitTest(this, point);
            if (hitvisual != null)
            {
                if (this.HeaderBorder != null)
                {
                    if (this.HeaderBorder.ActualHeight <= point.Y)
                    {
                        return false;
                    }
                }

                var viewitem = hitvisual.VisualHit.FindVisualParent<CustomTreeViewItem>();
                //保证动画加载点击的项上
                if (viewitem != null && viewitem.Equals(this))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static class BrushHelper
    {
        /// <summary> 十六进制颜色值转换为Brush </summary>
        /// <param name="brushStr">传入的颜色字符串</param>
        /// <returns>Brush</returns>
        public static System.Windows.Media.Brush ConvertToBrush(this string brushStr)
        {
            return new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(brushStr));
        }
    }
}