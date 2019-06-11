using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Ji.WPFHelper.ControlHelper
{
    public static class FindVisualHelper
    {
        /// <summary> 向上查找指定类型的逻辑容器 </summary>
        /// <typeparam name="T"> 欲查找的控件类型 </typeparam>
        /// <param name="descendent">     查找的子容器 </param>
        /// <param name="isContainsSelf"> 查找是否包含自身 </param>
        /// <returns> 返回第一个查找到的父控件 T? </returns>
        public static T FindVisualParent<T>(this DependencyObject descendent, bool isContainsSelf = true) where T : class
        {
            T ancestor = null;
            for (var scan = isContainsSelf ? descendent : VisualTreeHelper.GetParent(descendent); scan != null && ((ancestor = scan as T) == null);)
            {
                scan = VisualTreeHelper.GetParent(scan);
            }
            return ancestor;
        }

        public static bool? IsFirstVisualParent<T, R>(this DependencyObject descendent, bool isContainsSelf = true)
            where T : class
            where R : class
        {
            var scan = isContainsSelf ? descendent : VisualTreeHelper.GetParent(descendent);
            while (scan != null)
            {
                if (scan.GetType().GetInterface(typeof(T).Name, true) != null)
                {
                    return true;
                }
                if (scan.GetType().GetInterface(typeof(R).Name, true) != null)
                {
                    return false;
                }

                scan = VisualTreeHelper.GetParent(scan);
            }
            return null;
        }

        /// <summary> 查找指定类型的子物体 </summary>
        /// <typeparam name="T"> 欲查找的控件类型 </typeparam>
        /// <param name="descendent">     查找的根容器 </param>
        /// <param name="isContainsSelf"> 查找是否包含自身 </param>
        /// <returns> 返回第一个查找到的子控件 T? </returns>
        public static T FindVisualChild<T>(this DependencyObject descendent, bool isContainsSelf = true) where T : DependencyObject
        {
            T found = null;
            if (descendent is T && isContainsSelf)
            {
                found = descendent as T;
            }
            else
            {
                var childlen = VisualTreeHelper.GetChildrenCount(descendent);
                for (int i = 0; i < childlen; i++)
                {
                    var child = VisualTreeHelper.GetChild(descendent, i);
                    var target = child as T;
                    if (target == null)
                    {
                        found = FindVisualChild<T>(child, false);
                        if (found != null)
                        {
                            break;
                        }
                    }
                    else
                    {
                        found = (T)child;
                        break;
                    }
                }
            }
            return found;
        }

        /// <summary> 查找指定类型的子物体 </summary>
        /// <typeparam name="T"> 欲查找的控件类型 </typeparam>
        /// <param name="descendent">     查找的根容器 </param>
        /// <param name="isContainsSelf"> 查找是否包含自身 </param>
        /// <returns> 返回所有查找到的子控件 T? </returns>
        public static IEnumerable<T> FindAllVisualChild<T>(this DependencyObject descendent, bool isContainsSelf = true) where T : DependencyObject
        {
            T found = null;
            if (descendent is T && isContainsSelf)
            {
                found = descendent as T;
            }
            else
            {
                var childlen = VisualTreeHelper.GetChildrenCount(descendent);
                for (int i = 0; i < childlen; i++)
                {
                    var child = VisualTreeHelper.GetChild(descendent, i);
                    var target = child as T;
                    if (target == null)
                    {
                        foreach (var item in FindAllVisualChild<T>(child, false))
                        {
                            var t = (T)item;
                            if (t != null)
                            {
                                yield return t;
                            }
                        }
                    }
                    else
                    {
                        var t = (T)child;
                        if (t != null)
                        {
                            yield return t;
                        }
                    }
                }
            }
            var tf = (T)found;
            if (tf != null)
            {
                yield return tf;
            }
        }

        /// <summary> 在列表控件中中查询子元素 </summary>
        /// <typeparam name="T"> 欲查找的控件类型 </typeparam>
        /// <param name="selector"> 元数据列表 </param>
        /// <param name="data">     控件的数据源 </param>
        /// <returns> 返回查找到的类型 T? </returns>
        public static T FindControlInSelector<T>(this Selector selector, object data) where T : DependencyObject
        {
            var item = selector.ItemContainerGenerator.ContainerFromItem(data);
            if (item != null)
            {
                return item.FindVisualChild<T>();
            }
            return default(T);
        }
    }
}