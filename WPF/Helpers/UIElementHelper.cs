/* 迹I柳燕
 *
 * FileName:   UIElementHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.WPFHelper.ControlHelper
 * @class      UIElementHelper
 * @extends
 *
 *             WPF 扩展
 *             对于 UIElement 的扩展方法
 *
 *========================================

 *
 *
 *
 *
 */

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ji.WPFHelper.ControlHelper
{
    public static class UIElementHelper
    {
        /// <summary> 测试指定前点是否点中指定类型的元素 </summary>
        /// <typeparam name="T">查询的指定类型</typeparam>
        /// <param name="reference">测试点击的容器</param>
        /// <param name="mousepoint">点击测试的点</param>
        /// <returns>f返回的指定类型</returns>
        public static T IsHitVisual<T>(this UIElement reference, Point? mousepoint = null) where T : DependencyObject
        {
            var point = mousepoint ?? Mouse.GetPosition(reference);
            var hitvisualIntegerUpDown = VisualTreeHelper.HitTest(reference, point);
            if (hitvisualIntegerUpDown != null && hitvisualIntegerUpDown.VisualHit != null)
            {
                return hitvisualIntegerUpDown.VisualHit.FindVisualParent<T>();
            }
            return default(T);
        }

        public static bool RemoveFromUIParent(this FrameworkElement reference)
        {
            if (reference != null && reference.Parent != null)
            {
                var panel = (reference.Parent as System.Windows.Controls.Panel);
                if (panel != null)
                {
                    panel.Children.Remove(reference);
                }

                var cc = (reference.Parent as System.Windows.Controls.ContentControl);
                if (cc != null)
                {
                    cc.Content = null;
                }
            }

            return true;
        }
    }
}