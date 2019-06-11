// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-26-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-26-2017
// ***********************************************************************
// <copyright file="CollectionExtension.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Linq
{
    /// <summary>
    /// 收集器扩展类
    /// </summary>
    public static class CollectionExtension
    {
        #region Public 方法

        /// <summary>
        /// 收集器添加对象集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">收集器</param>
        /// <param name="items">可枚举对象</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            try
            {
                if (items == null || items.Count() == 0)
                {
                    return;
                }
                items.ForEach(i => source.Add(i));
            }
            catch { }
        }

        /// <summary>
        /// 收集器移除对象集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">收集器</param>
        /// <param name="items">可枚举对象</param>
        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            try
            {
                foreach (var item in items.ToList())
                {
                    if (source.Contains(item))
                    {
                        source.Remove(item);
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        /// <summary>
        /// 获取可枚举对象的子项数量
        /// </summary>
        /// <param name="items">可枚举对象</param>
        /// <returns>System.Int32.</returns>
        public static int Count(this IEnumerable items)
        {
            int count = 0;
            if (items != null)
            {
                foreach (var item in items) count++;
            }
            return count;
        }

        /// <summary>
        /// 可枚举对象 ForEach
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="items">可枚举对象</param>
        /// <param name="action">操作</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T current in items) action(current);
        }

        /// <summary>
        /// 可枚举对象是否为空
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="items">可枚举对象</param>
        /// <returns><c> true </c> if the specified items is empty; otherwise, <c> false </c>.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || items.Count<T>() == 0;
        }

        /// <summary>
        /// 可枚举对象转化为 提供支持数据绑定的通用集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="items">可枚举对象</param>
        /// <returns>BindingList&lt;T&gt;.</returns>
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> items)
        {
            BindingList<T> bindingList = new BindingList<T>();
            bindingList.AddRange(items);
            return bindingList;
        }

        /// <summary>
        /// 可枚举对象转化为动态收集器
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="items">可枚举对象</param>
        /// <returns>ObservableCollection&lt;T&gt;.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }

        #endregion Public 方法
    }
}