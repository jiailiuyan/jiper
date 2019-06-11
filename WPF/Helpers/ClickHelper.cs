using System;
using System.Windows;
using System.Windows.Input;

namespace Ji.CommonHelper.WPF.Helpers
{
    public class DoubleClickHelper
    {
        /// <summary>
        ///  Helper.LilyControl.DoubleClickHelper.CreatHelper(this.maingrid).PreviewDoubleClick +=
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static DoubleClickHelper CreatHelper(UIElement element)
        {
            return new DoubleClickHelper(element);
        }

        /// <summary> 双击事件 </summary>
        public event EventHandler<MouseButtonEventArgs> PreviewDoubleClick;

        protected DoubleClickHelper(UIElement element)
        {
            element.PreviewMouseLeftButtonDown += element_PreviewMouseLeftButtonDown;
        }

        private void element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OnDoubleClick(e);
            }
        }

        protected void OnDoubleClick(MouseButtonEventArgs e)
        {
            if (this.PreviewDoubleClick != null)
            {
                this.PreviewDoubleClick(this, e);
            }
        }
    }
}