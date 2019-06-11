// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-26-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-26-2017
// ***********************************************************************
// <copyright file="AttributeExtend.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 特性扩展.
    /// </summary>
    public static class AttributeExtend
    {
        #region Public 方法

        /// <summary>
        /// 获取特性.
        /// </summary>
        /// <typeparam name="TAttribute">特性类型.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>TAttribute.</returns>
        public static TAttribute GetAttribute<TAttribute>(this object obj) where TAttribute : Attribute
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is Enum)
            {
                FieldInfo field = obj.GetType().GetField(obj.ToString());
                if (field == null)
                    return null;

                TAttribute descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;
                return descriptionAttribute ?? null;
            }
            MemberInfo memberInfo = obj.GetType();
            var atts = memberInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (atts.Count() > 0)
                return atts[0] as TAttribute;
            return null;
        }

        #endregion Public 方法
    }

    /// <summary>
    /// 描述扩展类
    /// </summary>
    public static class DescriptionExtension
    {
        #region Public 方法

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="value">枚举对象</param>
        /// <returns>System.String.</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return (descriptionAttribute == null) ? value.ToString() : descriptionAttribute.Description;
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="action">The action.</param>
        /// <returns>System.String.</returns>
        public static string GetDescription<T, T1>(this T obj, Expression<Func<T, T1>> action)
        {
            string name = (action == null) ? string.Empty : GetPropertyName(action);
            return obj.GetDescription(name);
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public static string GetDescription<T>(this T obj, string name = null)
        {
            string result;
            if (obj is Enum)
                result = (obj as Enum).GetDescription();
            else
            {
                Type typeFromHandle = typeof(T);
                if (string.IsNullOrWhiteSpace(name))
                {
                    DescriptionAttribute descriptionAttribute = typeFromHandle.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().FirstOrDefault();
                    result = ((descriptionAttribute == null) ? string.Empty : descriptionAttribute.Description);
                }
                else
                {
                    MemberInfo memberInfo = typeFromHandle.GetProperty(name);
                    if (memberInfo == null)
                        memberInfo = typeFromHandle.GetField(name);
                    if (memberInfo == null)
                        result = string.Empty;
                    else
                    {
                        DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                        if (descriptionAttribute == null)
                            result = string.Empty;
                        else
                            result = descriptionAttribute.Description;
                    }
                }
            }
            return result;
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>System.String.</returns>
        private static string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> action)
        {
            MemberExpression memberExpression = (MemberExpression)action.Body;
            return memberExpression.Member.Name;
        }

        #endregion Private 方法
    }
}