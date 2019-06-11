// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-26-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-26-2017
// ***********************************************************************
// <copyright file="DeepCloneableExtension.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

//using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace System
{
    /// <summary>
    /// 深拷贝扩展类
    /// </summary>
    public static class DeepCloneableExtension
    {
        #region Public 方法

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">待拷贝对象</param>
        /// <param name="destination">拷贝到的位置</param>
        public static void Copy<T>(this T source, T destination)
        {
            Copy(destination, source, typeof(T));
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T DeepClone<T>(this T source) where T : new()
        {
            Type typeFromHandle = typeof(T);
            object obj = Activator.CreateInstance(typeFromHandle);
            Copy(source, obj, typeFromHandle);
            return (T)((object)obj);
        }

        /// <summary>
        /// 深拷贝序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T DeepCloneAsSerializable<T>(this T source) where T : class
        {
            T result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, source);
                memoryStream.Position = 0L;
                result = (binaryFormatter.Deserialize(memoryStream) as T);
            }
            return result;
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T DeepCloneJson<T>(this T source) where T : class, new()
        {
            //if (source == null)
            //{
                return null;
            //}
            //var s = JsonConvert.SerializeObject(source);
            //return (T)JsonConvert.DeserializeObject(s, source.GetType());
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T DeepCloneJson<T>(this T source, Type type) where T : class
        {
            //if (source == null && typeof(T) == type)
            //{
                return null;
            //}
            //var s = JsonConvert.SerializeObject(source);
            //return (T)JsonConvert.DeserializeObject(s, type);
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="dst">源</param>
        /// <param name="src">目标</param>
        /// <param name="type">类型</param>
        private static void Copy(object dst, object src, Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetInterface("IList") != null)
                {
                    IList list = src as IList;
                    IList list2 = dst as IList;
                    foreach (object current in list)
                    {
                        Type type2 = current.GetType();
                        if (type2.IsPrimitive || type2.IsValueType || type2 == typeof(string))
                            list2.Add(current);
                        else
                            list2.Add(current.DeepClone<object>());
                    }
                    return;
                }
                if (type.GetInterface("IDictionary") != null)
                {
                    IDictionary dictionary = src as IDictionary;
                    IDictionary dictionary2 = dst as IDictionary;
                    foreach (object current2 in dictionary.Keys)
                    {
                        object obj = dictionary[current2];
                        Type type3 = current2.GetType();
                        if (type3.IsPrimitive || type3.IsValueType || type3 == typeof(string))
                        {
                            if (type3.IsPrimitive || type3.IsValueType || type3 == typeof(string))
                                dictionary2.Add(current2, obj);
                            else
                                dictionary2.Add(current2, obj.DeepClone<object>());
                        }
                        else if (type3.IsPrimitive || type3.IsValueType || type3 == typeof(string))
                            dictionary2.Add(current2.DeepClone<object>(), obj);
                        else
                            dictionary2.Add(current2.DeepClone<object>(), obj.DeepClone<object>());
                    }
                    return;
                }
            }
            PropertyInfo[] properties = type.GetProperties();
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(src, propertyInfo.GetValue(dst, null), null);
                    }
                    else
                    {
                        object value = propertyInfo.GetValue(dst, null);
                        if (value != null)
                        {
                            propertyInfo.SetValue(src, value.DeepClone<object>(), null);
                        }
                    }
                }
            }
            FieldInfo[] fields = type.GetFields();
            FieldInfo[] array2 = fields;
            for (int i = 0; i < array2.Length; i++)
            {
                FieldInfo fieldInfo = array2[i];
                if (!fieldInfo.IsStatic)
                {
                    if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.IsValueType || fieldInfo.FieldType == typeof(string))
                    {
                        fieldInfo.SetValue(src, fieldInfo.GetValue(dst));
                    }
                    else
                    {
                        object value = fieldInfo.GetValue(dst);
                        if (value != null)
                        {
                            fieldInfo.SetValue(src, value.DeepClone<object>());
                        }
                    }
                }
            }
        }

        #endregion Private 方法
    }
}