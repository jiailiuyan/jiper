using System;
using System.Windows;
using System.Windows.Controls;
using Ji.WPFHelper.ControlHelper;

namespace Ji.WPFHelper.ControlLibs
{
    /// <summary>
    /// BoneControl.xaml 的交互逻辑
    /// </summary>
    partial class ListBoxItemUC : UserControl
    {
        //public Bone Item
        //{
        //    get { return (Bone)GetValue(ItemProperty); }
        //    set { SetValue(ItemProperty, value); }
        //}
        //public static readonly DependencyProperty ItemProperty =
        //    DependencyProperty.Register("Item", typeof(Bone), typeof(ListBoxItemUC), new PropertyMetadata(null));

        public bool IsEditor
        {
            get { return (bool)GetValue(IsEditorProperty); }
            set { SetValue(IsEditorProperty, value); }
        }

        public static readonly DependencyProperty IsEditorProperty =
            DependencyProperty.Register("IsEditor", typeof(bool), typeof(ListBoxItemUC), new PropertyMetadata(false, IsEditorChanged));

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ListBoxItemUC), new PropertyMetadata(new Thickness(0, 0, 5, 0)));

        private static void IsEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ListBoxItemUC;
            if (control != null)
            {
                control.OnEditChanging();
            }
        }

        public event EventHandler OnEditChanged;

        protected void OnEditChanging()
        {
            if (OnEditChanged != null)
            {
                OnEditChanged(this, EventArgs.Empty);
            }
        }

        public ListBoxItemUC()
        {
            InitializeComponent();

            this.Loaded += ListBoxItemUC_Loaded;

            this.PreviewDragEnter += ListBoxItemUC_PreviewDragEnter;
            this.PreviewDragLeave += ListBoxItemUC_PreviewDragLeave;
            this.PreviewDrop += ListBoxItemUC_PreviewDrop;
        }

        private void ListBoxItemUC_PreviewDragEnter(object sender, DragEventArgs e)
        {
            SetShowBorderMain(true, false);
        }

        private void ListBoxItemUC_PreviewDragLeave(object sender, DragEventArgs e)
        {
            SetShowBorderMain(false);
        }

        private void ListBoxItemUC_PreviewDrop(object sender, DragEventArgs e)
        {
            SetShowBorderMain(false);
        }

        private void ListBoxItemUC_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Item.IsSelected)
            //{
            //    var listbox = this.FindVisualParent<ListBox>();
            //    if (listbox != null)
            //    {
            //        var listboxitem = listbox.ItemContainerGenerator.ContainerFromItem(Item) as ListBoxItem;
            //        if (listboxitem != null)
            //        {
            //            listboxitem.IsSelected = true;
            //        }
            //    }
            //}
        }

        public void SetShowBorderMain(bool isShow = true, bool isLast = false)
        {
            if (isShow)
            {
                this.Opacity = 0.1d;
                //this.borderMain.BorderBrush = Brushes.WhiteSmoke;
                //this.borderMain.BorderThickness = isLast ? new Thickness(0, 0, 0, 2) : new Thickness(0, 2, 0, 0);
            }
            else
            {
                this.Opacity = 1;
                //this.borderMain.BorderThickness = new Thickness(0);
            }
        }
    }
}