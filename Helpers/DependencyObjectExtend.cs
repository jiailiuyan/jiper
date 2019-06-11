// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-25-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-25-2017
// ***********************************************************************
// <copyright file="DependencyObjectExtend.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace System
{
    /// <summary>
    /// Class DependencyObjectExtend.
    /// </summary>
    public static class DependencyObjectExtend
    {
        #region Public 方法

        /// <summary>
        /// WPF中查找元素的父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The i_dp.</param>
        /// <returns>T.</returns>
        public static T FindParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(obj);
            if (dobj != null)
            {
                if (dobj is T)
                    return (T)dobj;
                else
                {
                    dobj = FindParent<T>(dobj);
                    if (dobj != null && dobj is T)
                        return (T)dobj;
                }
            }
            return null;
        }

        /// <summary>
        /// WPF查找指定类型子元素
        /// </summary>
        /// <typeparam name="T">The type of the child item.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>childItem.</returns>
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// WPF查找所有指定类型子元素.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj">The dep object.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> FindVisualChilds<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChilds<T>(child))
                        yield return childOfChild;
                }
            }
        }

        #endregion Public 方法
    }
}