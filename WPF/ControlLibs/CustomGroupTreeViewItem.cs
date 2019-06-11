using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ji.WPFHelper.ControlHelper;

namespace Ji.WPFHelper.ControlLibs
{
    public class CustomGroupTreeViewItem : CustomTreeViewItem
    {
        ///// <summary> 当前绑定的数据
        /////  因为Common.DepartmentNodeItem 可访问性低，因此不能设置此项模版数据为继承，所以单独处理
        ///// </summary>
        //private Common.DepartmentNodeItem Data { get; set; }

        public event EventHandler SelectedChanged;

        private bool isCustomSelected = false;

        public bool IsCustomSelected
        {
            get { return isCustomSelected; }
            set
            {
                if (value)
                {
                    this.SetSelect();
                }
                else
                {
                    this.SetUnSelect();
                }

                if (isCustomSelected != value)
                {
                    isCustomSelected = value;
                    SelectedChanging();
                }
            }
        }

        protected override void CustomTreeViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            base.CustomTreeViewItem_Loaded(sender, e);
        }

        #region 样式处理

        protected override void InitStyleControls()
        {
            if (ExpandToggleButton != null)
            {
                StyleMainGrid = ExpandToggleButton.FindVisualParent<Grid>();
                if (StyleMainGrid != null)
                {
                    //调整样式层级，使得选中当做前景色
                    StyleBorder = new Border();
                    Grid.SetColumnSpan(StyleBorder, 3);
                    StyleBorder.Background = Brushes.Transparent;
                    StyleBorder.BorderBrush = SelectBorderBrush;
                    StyleBorder.BorderThickness = new Thickness(0);
                    StyleMainGrid.Children.Insert(0, StyleBorder);

                    StyleForgroundBorder = new Border();
                    Grid.SetColumnSpan(StyleForgroundBorder, 3);
                    StyleForgroundBorder.Background = Brushes.Transparent;
                    StyleForgroundBorder.BorderBrush = OverBorderBrush;
                    StyleForgroundBorder.BorderThickness = new Thickness(0);
                    StyleMainGrid.Children.Insert(0, StyleForgroundBorder);
                }
            }
        }

        #endregion 样式处理

        #region 选中处理

        protected override void CustomTreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            //屏蔽父级的选中处理，改为由 Ctrl + Mouse 选中进行处理
            //base.CustomTreeViewItem_Selected(sender, e);
        }

        protected override void CustomTreeViewItem_Unselected(object sender, RoutedEventArgs e)
        {
            //屏蔽父级的取消选中处理，增加 Ctrl 按键处理
            //base.CustomTreeViewItem_Unselected(sender, e);
            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                this.IsCustomSelected = false;
            }
        }

        protected override void treeview_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool iscontroldown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            if (this.IsHitCurrentControl())
            {
                if (iscontroldown)
                {
                    this.IsCustomSelected = !this.IsCustomSelected;
                }
                else
                {
                    this.IsCustomSelected = true;
                }
            }
            else
            {
                if (!iscontroldown)
                {
                    this.IsCustomSelected = false;
                }
            }
        }

        #endregion 选中处理

        protected void SelectedChanging()
        {
            if (this.SelectedChanged != null)
            {
                this.SelectedChanged(this, EventArgs.Empty);
            }
        }
    }
}