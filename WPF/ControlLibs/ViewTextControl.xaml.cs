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
using Ji.DataHelper.Info;

namespace Ji.WPFHelper.ControlLibs
{
    /// <summary> ViewTextControl.xaml 的交互逻辑 </summary>
    public partial class ViewTextControl : UserControl
    {
        public bool IsAutoWidth
        {
            get { return (bool)GetValue(IsAutoWidthProperty); }
            set { SetValue(IsAutoWidthProperty, value); }
        }

        public static readonly DependencyProperty IsAutoWidthProperty =
            DependencyProperty.Register("IsAutoWidth", typeof(bool), typeof(ViewTextControl), new PropertyMetadata(false));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ViewTextControl), new PropertyMetadata("", TextChangedCallback));

        protected static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ViewTextControl);
            control.RefreshView(control.ActualWidth, e.NewValue + "");
        }

        public bool ViewUnderLine
        {
            get { return (bool)GetValue(ViewUnderLineProperty); }
            set { SetValue(ViewUnderLineProperty, value); }
        }

        public static readonly DependencyProperty ViewUnderLineProperty =
            DependencyProperty.Register("ViewUnderLine", typeof(bool), typeof(ViewTextControl), new PropertyMetadata(false));

        public TextAlignment TAlignment
        {
            get { return (TextAlignment)GetValue(TAlignmentProperty); }
            set { SetValue(TAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TAlignmentProperty =
            DependencyProperty.Register("TAlignment", typeof(TextAlignment), typeof(ViewTextControl), new PropertyMetadata(TextAlignment.Justify));

        public ViewTextControl()
        {
            InitializeComponent();
            this.SizeChanged += ViewTextControl_SizeChanged;

            this.Loaded += ViewTextControl_Loaded;
        }

        private void ViewTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshView(this.ActualWidth, this.Text);
            if (this.ViewUnderLine)
            {
                this.textblock.TextDecorations = TextDecorations.Underline;
            }
        }

        private void ViewTextControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                RefreshView(e.NewSize.Width, this.Text);
            }
        }

        public void RefreshView(double width, string text)
        {
            if (IsAutoWidth)
            {
                this.textblock.Text = text;
            }
            else
            {
                if (width == 0)
                {
                    return;
                }

                var size = FontHelper.Measure(text, this.FontSize, this.FontFamily.Source);
                var maxwidth = width - 6;
                if (maxwidth < size.Width)
                {
                    var t = "";
                    foreach (var item in text)
                    {
                        size = FontHelper.Measure(t + "....", this.FontSize, this.FontFamily.Source);
                        if (size.Width > maxwidth)
                        {
                            break;
                        }
                        t += item.ToString();
                    }
                    this.textblock.Text = t + "...";
                }
                else
                {
                    this.textblock.Text = text;
                }
                this.ToolTip = text;
            }
        }
    }
}