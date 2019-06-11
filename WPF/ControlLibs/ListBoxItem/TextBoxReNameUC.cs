using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ji.WPFHelper.ControlHelper;

namespace Ji.WPFHelper.ControlLibs
{
    internal class TextBoxReNameUC : TextBox
    {
        public ListBoxItemUC ParentUC
        {
            get { return (ListBoxItemUC)GetValue(ParentUCProperty); }
            set { SetValue(ParentUCProperty, value); }
        }

        public static readonly DependencyProperty ParentUCProperty =
            DependencyProperty.Register("ParentUC", typeof(ListBoxItemUC), typeof(TextBoxReNameUC), new PropertyMetadata(null, ParentUCChanged));

        public TextBoxReNameUC()
        {
            this.Loaded += delegate
            {
                this.IsVisibleChanged += SPReNameTextBox_IsVisibleChanged;
                //this.PreviewMouseDown += delegate
                //{
                //    if (this.Text != null)
                //    {
                //        this.Select(0, this.Text.Length);
                //    }
                //};
            };

            this.Unloaded += delegate
            {
                this.IsVisibleChanged -= SPReNameTextBox_IsVisibleChanged;
            };

            this.AddHandler(TextBox.PreviewMouseDownEvent, new MouseButtonEventHandler(TextBoxReNameUC_PreviewMouseDown), true);
            this.AddHandler(TextBox.PreviewMouseUpEvent, new MouseButtonEventHandler(TextBoxReNameUC_PreviewMouseUp), true);
            this.AddHandler(TextBox.PreviewMouseMoveEvent, new MouseEventHandler(TextBoxReNameUC_PreviewMouseMove), true);
        }

        private void TextBoxReNameUC_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnPreviewMouseDown(e);
        }

        private void TextBoxReNameUC_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            OnPreviewMouseMove(e);
        }

        private void TextBoxReNameUC_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnPreviewMouseUp(e);
        }

        private static void ParentUCChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxReNameUC tb = d as TextBoxReNameUC;
            ListBoxItemUC listbox = e.NewValue as ListBoxItemUC;
            if (tb != null && listbox != null)
            {
                listbox.PreviewMouseDown += (sender, args) => { args.Handled = true; };
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            var caretIndex = this.CaretIndex;
            var selectionStart = this.SelectionStart;
            var selectLength = this.SelectionLength;
            if (!this.IsFocused)
            {
                this.Focusable = true;
                this.Focus();

                Mouse.Capture(this);

                #region 计算TextBox的光标位置

                if (this.CaretIndex < caretIndex) // 往左选择
                {
                    if (selectLength > this.SelectionLength)
                    {
                        this.SelectionStart = this.CaretIndex;
                    }
                    else if (selectLength == this.SelectionLength)
                    {
                        this.SelectionStart = this.SelectionStart + this.SelectionLength;
                    }
                    else
                    {
                        this.SelectionStart = this.CaretIndex;
                    }
                }
                else if (this.CaretIndex > caretIndex)
                {
                    if (selectLength > this.SelectionLength)
                    {
                        this.CaretIndex = this.SelectionStart;
                    }
                    else if (selectLength == this.SelectionLength)
                    {
                        this.SelectionStart = this.SelectionStart;
                    }
                    else
                    {
                        this.CaretIndex = this.SelectionStart + this.SelectionLength;
                    }
                }
                else //全选时控制操作
                {
                    if (selectLength == this.SelectionLength)
                    {
                        this.CaretIndex = this.SelectionStart;
                    }
                    else
                    {
                        this.CaretIndex = this.SelectionStart + this.SelectionLength;
                    }
                }

                this.SelectionLength = 0;

                #endregion 计算TextBox的光标位置
            }
            base.OnPreviewMouseDown(e);
        }

        private void SPReNameTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                Mouse.Capture(null);
            }
            else
            {
                this.Focus();
                this.SelectAll();
            }
        }

        public void EndInput()
        {
            if (ParentUC != null)
            {
                string newname = this.Text.Trim();
                //bool canchange = false;
                if (this.Text != null && !string.IsNullOrWhiteSpace(newname))
                {
                    var control = ParentUC.FindVisualParent<object>();
                    if (control != null)
                    {
                        //if (control.ViewModel.Items.FirstOrDefault(i => i.Name == newname) == null)
                        //{
                        //    canchange = true;
                        //}
                    }
                }

                //if (canchange)
                //{
                //    if (this.Text != ParentUC.Item.Name)
                //    {
                //        ParentUC.Item.Name = newname;
                //    }
                //}
                //else
                //{
                //    if (ParentUC.Item.Name != newname)
                //    {
                //        if (ParentUC.IsEditor)
                //        {
                //        }
                //        this.Text = ParentUC.Item.Name;
                //    }
                //}

                ParentUC.IsEditor = false;
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            var hitVisual = VisualTreeHelper.HitTest(this, Mouse.GetPosition(this as IInputElement));
            if (this.Equals(hitVisual))
            {
                EndInput();
                return;
            }

            //if (this.Text != ParentUC.Item.Name)
            //{
            //    EndInput();
            //}
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                EndInput();
            }
            base.OnKeyDown(e);
        }
    }
}