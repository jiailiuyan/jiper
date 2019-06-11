// ***********************************************************************
// 程序集            : Ksy.Client.CommonHelper
// 作者              : Lyon
// 创建日期          : 05-03-2018
//
// 最后编辑者        : Lyon
// 最后编辑时间      : 05-03-2018
// ***********************************************************************
// <版权所有 文件="ObjectHelpers.cs" 组织="Lyon">
//     Copyright © Lyon
// </版权所有>
// ***********************************************************************
using System.Reflection;

namespace System
{
    /// <summary>
    /// Class ObjectHelpers.
    /// </summary>
    public static class ObjectHelpers
    {
        #region Public 方法

        /// <summary>
        /// 泛型类型转换
        /// </summary>
        /// <typeparam name="T">要转换的基础类型</typeparam>
        /// <param name="val">要转换的值</param>
        /// <returns>T.</returns>
        public static T ConvertType<T>(this object val)
        {
            var res = default(T);
            if (val != null)
            {
                Type tp = typeof(T);
                //string直接返回转换
                if (tp == typeof(string))
                    if (val is string)
                        return (T)val;
                    else
                        return (T)((object)val.ToString());
                if (!(val is string))
                    val = val.ToString();
                if (tp == typeof(int) && ((string)val).Contains("."))
                    val = ((string)val).Remove(((string)val).IndexOf('.'));
                //反射获取TryParse方法
                var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                                new Type[] { typeof(string), tp.MakeByRefType() },
                                                new ParameterModifier[] { new ParameterModifier(2) });
                var parameters = new object[] { val, res };
                //成功返回转换后的值，否则返回类型的默认值
                if ((bool)TryParse.Invoke(null, parameters))
                    res = (T)parameters[1];
            }
            else if (Nullable.GetUnderlyingType(typeof(T)) != null)
                return (T)(object)null;
            return res;
        }

        #endregion Public 方法
    }
}