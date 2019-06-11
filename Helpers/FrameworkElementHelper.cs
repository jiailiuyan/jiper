// ***********************************************************************
// Assembly         : Yuanbo.ChssClient.Common
// Author           : 刘晓青
// Created          : 12-21-2017
//
// Last Modified By : 刘晓青
// Last Modified On : 12-21-2017
// ***********************************************************************
// <copyright file="FrameworkElementHelper.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;

namespace System
{
    /// <summary>
    /// Class FrameworkElementHelper.
    /// </summary>
    public static class FrameworkElementHelper
    {
        #region Public 方法

        /// <summary>
        /// 获取视图控制模型.
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">数据源.</param>
        /// <returns>T.</returns>
        public static T GetViewModel<T>(this FrameworkElement source) where T : class
        {
            if (source.DataContext is T)
            {
                return (T)source.DataContext;
            }
            return null;
        }

        #endregion Public 方法
    }
}