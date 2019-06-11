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

namespace SelectedControl
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl, ISelectedItem
    {
        public UserControl1()
        {
            InitializeComponent();
            this.Background = Brushes.AliceBlue;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.Width = 100;
            this.Height = 100;
        }

        #region ISelectedItem 成员

        bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
            }
        }

        public bool HitTestPoint(UIElement rootElement, Point point)
        {
            var p = rootElement.TranslatePoint(point, this);
            return new Rect(this.RenderSize).Contains(p);
        }

        public bool HitTestRect(UIElement rootElement, Rect rect)
        {
            return new Rect(this.RenderSize).IntersectsWith(rect);
        }

        public Rect GetViewRect(UIElement rootElement)
        {
            var p = this.TranslatePoint(new Point(), rootElement);
            return new Rect(p, this.RenderSize);
        }

        public void MovePosition(Point defferpoint)
        {
            var x = this.Margin.Left - defferpoint.X;
            var y = this.Margin.Top - defferpoint.Y;

            this.Margin = new Thickness(x, y, 0, 0);
        }

        #endregion

        #region ISelectedItem 成员


        public bool CanVerticalMove
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanHorizontalMove
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
