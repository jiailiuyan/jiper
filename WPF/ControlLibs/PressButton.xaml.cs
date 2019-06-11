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

namespace Ji.WPFHelper.ControlLibs
{
    /// <summary>
    /// AccuracyItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class PressButton : UserControl, ICommandSource
    {
        public Brush PressBrush
        {
            get { return (Brush)GetValue(PressBrushProperty); }
            set { SetValue(PressBrushProperty, value); }
        }

        public static readonly DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush), typeof(PressButton), new PropertyMetadata(Brushes.Transparent));

        public Brush UnPressBrush
        {
            get { return (Brush)GetValue(UnPressBrushProperty); }
            set { SetValue(UnPressBrushProperty, value); }
        }

        public static readonly DependencyProperty UnPressBrushProperty =
            DependencyProperty.Register("UnPressBrush", typeof(Brush), typeof(PressButton), new PropertyMetadata(Brushes.Transparent));

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(PressButton), new PropertyMetadata(Brushes.Transparent));

        public string ViewText
        {
            get { return (string)GetValue(ViewTextProperty); }
            set { SetValue(ViewTextProperty, value); }
        }

        public static readonly DependencyProperty ViewTextProperty =
            DependencyProperty.Register("ViewText", typeof(string), typeof(PressButton), new PropertyMetadata(string.Empty));

        public Brush PressTextBrush
        {
            get { return (Brush)GetValue(PressTextBrushProperty); }
            set { SetValue(PressTextBrushProperty, value); }
        }

        public static readonly DependencyProperty PressTextBrushProperty =
            DependencyProperty.Register("PressTextBrush", typeof(Brush), typeof(PressButton), new PropertyMetadata(null));

        public Brush UnPressTextBrush
        {
            get { return (Brush)GetValue(UnPressTextBrushProperty); }
            set { SetValue(UnPressTextBrushProperty, value); }
        }

        public static readonly DependencyProperty UnPressTextBrushProperty =
            DependencyProperty.Register("UnPressTextBrush", typeof(Brush), typeof(PressButton), new PropertyMetadata(null));

        public Thickness ViewTextMargin
        {
            get { return (Thickness)GetValue(ViewTextMarginProperty); }
            set { SetValue(ViewTextMarginProperty, value); }
        }

        public static readonly DependencyProperty ViewTextMarginProperty =
            DependencyProperty.Register("ViewTextMargin", typeof(Thickness), typeof(PressButton), new UIPropertyMetadata(new Thickness(0)));

        public event EventHandler Click = delegate { };

        //Command="{x:Static commands:aSeeCommands.CloseWindow}"
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(PressButton), new PropertyMetadata(null));

        public object CommandParameter
        {
            get;
            set;
        }

        public IInputElement CommandTarget
        {
            get;
            set;
        }

        public object ViewContent
        {
            get { return (object)GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }

        public static readonly DependencyProperty ViewContentProperty =
            DependencyProperty.Register("ViewContent", typeof(object), typeof(PressButton), new PropertyMetadata(null, ViewContentCallback));

        private static void ViewContentCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PressButton).ChangeViewContent();
        }

        public PressButton()
        {
            MouseOverBrush = UnPressBrush;

            this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

            InitializeComponent();
            this.Loaded += PressButton_Loaded;
            this.IsEnabledChanged += PressButton_IsEnabledChanged;
        }

        private void ChangeViewContent()
        {
            var element = this.ViewContent as FrameworkElement;
            if (element != null)
            {
                element.SetBinding(FrameworkElement.WidthProperty, new Binding() { Source = this, Path = new PropertyPath("ActualWidth"), Mode = BindingMode.OneWay });
                element.SetBinding(FrameworkElement.HeightProperty, new Binding() { Source = this, Path = new PropertyPath("ActualHeight"), Mode = BindingMode.OneWay });

                this.viewbutton.Content = this.ViewContent;
            }
        }

        private void PressButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                this.Opacity = 1;
            }
            else
            {
                this.Opacity = 0.5;
            }
        }

        private void PressButton_Loaded(object sender, RoutedEventArgs e)
        {
            SetUnPress(true);
        }

        private bool isdown = false;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            OnPreviewMouseLeftButtonDowning();
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            OnPreviewMouseLeftButtonUping();
        }

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            OnPreviewMouseLeftButtonDowning();
        }

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            base.OnPreviewTouchUp(e);
            OnPreviewMouseLeftButtonUping();
        }

        protected virtual void OnPreviewMouseLeftButtonDowning()
        {
            isdown = true;
            SetPress();
        }

        protected virtual void OnPreviewMouseLeftButtonUping()
        {
            SetUnPress();
            isdown = false;
            this.viewbutton.Background = this.MouseOverBrush;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (!isdown)
            {
                this.viewbutton.Background = this.MouseOverBrush;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!isdown)
            {
                if (isselect)
                {
                    this.viewbutton.Background = this.PressBrush;
                }
                else
                {
                    this.viewbutton.Background = this.UnPressBrush;
                }
            }

            if (isdown)
            {
                isdown = false;
            }
        }

        public void SetPress()
        {
            this.viewbutton.Background = this.PressBrush;
            this.viewtextblock.Foreground = this.PressTextBrush == null ? this.Foreground : this.PressTextBrush;

            if (this.Command != null)
            {
                this.Command.Execute(CommandTarget ?? this);
            }
            else
            {
                this.Click(this, EventArgs.Empty);
            }
        }

        public void SetUnPress(bool isinit = false)
        {
            if (isselect)
            {
                return;
            }

            if (isinit)
            {
                if (this.viewbutton.Background.Equals(this.PressBrush))
                {
                    return;
                }
                else
                {
                    this.viewbutton.Background = this.PressBrush;
                    this.viewbutton.Background = this.UnPressBrush;
                }
            }

            this.viewbutton.Background = this.UnPressBrush;
            this.viewtextblock.Foreground = this.PressTextBrush == null ? this.Foreground : this.UnPressTextBrush;
        }

        private bool isselect = false;

        public void SetSelect()
        {
            isselect = true;
            this.viewbutton.Background = this.PressBrush;
        }

        public void SetUnSelect()
        {
            isselect = false;
            this.viewbutton.Background = this.UnPressBrush;
        }
    }
}