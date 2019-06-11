// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-21-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-21-2017
// ***********************************************************************
// <copyright file="DesingerHelper.cs" company="健康致医">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Windows;

namespace System
{
    /// <summary>
    /// 设计器帮助类.
    /// </summary>
    public class DesignerHelper
    {
        #region Private 字段

        /// <summary>
        /// 是否为设计模式
        /// </summary>
        private static bool? _isInDesignMode;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 获取一个值，该值指示控件是否处于设计模式.
        /// </summary>
        /// <value><c>true</c> if this instance is in design mode static; otherwise, <c>false</c>.</value>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                                        .FromProperty(prop, typeof(FrameworkElement))
                                        .Metadata.DefaultValue;
                }

                return _isInDesignMode.Value;
            }
        }

        #endregion Public 属性
    }
}