using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Ji.LinqHelper;

namespace Ji.DataHelper
{
    public static class ClassInfoHelper
    {
        /// <summary> 从当前类型一直向上查找指定名称的字段 </summary>
        /// <param name="type">        查找的起始类型 </param>
        /// <param name="fieldName">   字段的名称 </param>
        /// <param name="bindingAttr"> 搜索的标志 </param>
        /// <returns> 返回查询到的 FieldInfo </returns>
        public static FieldInfo FindField(this Type type, string fieldName, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                var field = type.GetField(fieldName, bindingAttr);
                if (field != null)
                {
                    return field;
                }
                else if (type.BaseType != null)
                {
                    return type.BaseType.FindField(fieldName, bindingAttr);
                }
            }
            return null;
        }

        /// <summary> 从当前类型一直向上查找指定名称的属性 </summary>
        /// <param name="type">        查找的起始类型 </param>
        /// <param name="fieldName">   属性的名称 </param>
        /// <param name="bindingAttr"> 搜索的标志 </param>
        /// <returns> 返回查询到的 PropertyInfo </returns>
        public static PropertyInfo FindProperty(this Type type, string propertyName, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                var property = type.GetProperty(propertyName, bindingAttr);
                if (property != null)
                {
                    return property;
                }
                else if (type.BaseType != null)
                {
                    return type.BaseType.FindProperty(propertyName, bindingAttr);
                }
            }
            return null;
        }

        /// <summary> 从当前类型一直向上查找指定名称的属性 </summary>
        /// <param name="data">         查找的起始类型 </param>
        /// <param name="propertyName"> 属性的名称 </param>
        /// <param name="bindingAttr">  搜索的标志 </param>
        /// <returns> 返回查询到的 PropertyInfo </returns>
        public static T FindProperty<T>(this object data, string propertyName, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic) where T : class
        {
            if (data != null)
            {
                var p = FindProperty(data.GetType(), propertyName, bindingAttr);
                if (p != null)
                {
                    return p.GetValue(data, new object[0]) as T;
                }
            }
            return null;
        }

        /// <summary> 查询当前程序集所拥有的指定类型字段字段元数据集合 </summary>
        /// <typeparam name="T"> 指定查询的类型 </typeparam>
        /// <param name="assembly">    查选的程序集 </param>
        /// <param name="bindingAttr"> 查询的指定参数 </param>
        /// <returns> 查询到的字段元数据集合 </returns>
        public static IList<FieldInfo> FindFieldInAssembly<T>(this Assembly assembly, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic) where T : class
        {
            IList<FieldInfo> datas = new List<FieldInfo>();

            var classTypes = assembly.GetTypes();
            foreach (var item in classTypes)
            {
                var fields = item.GetFields(bindingAttr);
                if (fields.Count() > 0)
                {
                    var fieldType = typeof(T);
                    //此处在大量数据时曾试过并行获取 Parallel.ForEach
                    //不幸的发现会更慢
                    foreach (var field in fields)
                    {
                        if (field.FieldType.Equals(fieldType))
                        {
                            datas.Add(field);
                        }
                    }
                }
            }

            return datas;
        }

        /// <summary> 查询当前程序集所拥有的指定类型静态字段集合 </summary>
        /// <typeparam name="T"> 指定查询的类型 </typeparam>
        /// <param name="assembly">    查选的程序集 </param>
        /// <param name="bindingAttr"> 查询的指定参数 </param>
        /// <returns> 查询到指定静态类型的值集合 </returns>
        public static IList<T> FindStaticFieldValueInAssembly<T>(this Assembly assembly, BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public) where T : class
        {
            IList<T> datas = new List<T>();

            var fieldList = assembly.FindFieldInAssembly<T>(bindingAttr);
            if (fieldList.Count > 0)
            {
                //此处在大量数据时曾试过并行获取 Parallel.ForEach
                //不幸的发现会更慢
                fieldList.ForEach(i =>
                {
                    var data = i.GetValue(null) as T;
                    if (data != null)
                    {
                        datas.Add(data);
                    }
                });
            }

            return datas;
        }

        /// <summary> 查询当前程序集所拥有的指定类型接口的接口元数据集合 </summary>
        /// <typeparam name="T"> 指定查询类型的接口 </typeparam>
        /// <param name="assembly">    查选的程序集 </param>
        /// <param name="bindingAttr"> 查询的指定参数 </param>
        /// <returns> 查询到的字段元数据集合 </returns>
        public static IDictionary<Type, Type> FindInterfacesInAssembly<T>(this Assembly assembly, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic) where T : class
        {
            IDictionary<Type, Type> datas = new Dictionary<Type, Type>();

            var classTypes = assembly.GetTypes();
            var returnType = typeof(T).Name;
            foreach (var item in classTypes)
            {
                var interfaces = item.GetInterfaces();
                if (interfaces.Count() > 0)
                {
                    var interfaceOfT = interfaces.FirstOrDefault(i => i.Name.Equals(returnType));
                    if (interfaceOfT != null)
                    {
                        datas.Add(item, interfaceOfT);
                    }
                }
            }
            return datas;
        }

        public static object InvokeMethod(this Type type, string methodname, object data)
        {
            return InvokeMethod<object>(type, methodname, data);
        }

        public static T InvokeMethod<T>(this Type type, string methodname, object data) where T : class
        {
            return InvokeMethod<T>(type, methodname, new List<object>() { data }, BindingFlags.Instance | BindingFlags.Public);
        }

        public static T InvokeMethod<T>(this Type type, string methodname, List<object> datas, BindingFlags bindingAttr) where T : class
        {
            var instance = type.Assembly.CreateInstance(type.FullName);
            if (instance != null)
            {
                var method = type.GetMethod(methodname, bindingAttr);
                if (method != null)
                {
                    return method.Invoke(instance, new List<object>(datas).ToArray()) as T;
                }
            }
            return null;
        }

        public static T CreatAndInvokeMethod<T>(this Type type, string methodname, object data) where T : class
        {
            return CreatAndInvokeMethod<T>(type, methodname, new List<object>() { data }, BindingFlags.Instance | BindingFlags.Public);
        }

        public static T CreatAndInvokeMethod<T>(this Type type, string methodname, List<object> datas, BindingFlags bindingAttr) where T : class
        {
            var instance = type.Assembly.CreateInstance(type.FullName) as T;
            if (instance != null)
            {
                var method = type.GetMethod(methodname, bindingAttr);
                if (method != null)
                {
                    method.Invoke(instance, new List<object>(datas).ToArray());
                    return instance;
                }
            }
            return null;
        }

        /// <summary> 复制源所有公开的属性值,因为会降低性能，因此不推荐大量使用 </summary>
        public static void DeepCopy<T>(this T oldvalue, T newvalue)
        {
            var type = typeof(T);
            var properties = new ReadOnlyCollection<string>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.Name != "Properties").Select(i => i.Name).ToList());
            foreach (var property in properties)
            {
                var propertyinfo = type.GetProperty(property);
                if (propertyinfo != null)
                {
                    if (propertyinfo.CanRead && propertyinfo.CanWrite)
                    {
                        propertyinfo.SetValue(oldvalue, propertyinfo.GetValue(newvalue, null), null);
                    }
                }
            }
        }

        public static void DeepCopyProperties<T, R>(this T newvalue, R oldvalue)
        {
            var oldtype = typeof(R);
            var oldproperties = new ReadOnlyCollection<string>(oldtype.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.Name != "Properties").Select(i => i.Name).ToList());

            var type = typeof(T);
            var properties = new ReadOnlyCollection<string>(type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(i => i.Name != "Properties").Select(i => i.Name).ToList());
            foreach (var property in properties)
            {
                var propertyinfo = type.GetProperty(property);
                if (propertyinfo != null)
                {
                    if (propertyinfo.CanRead && propertyinfo.CanWrite)
                    {
                        try
                        {
                            var oldpi = oldtype.GetProperty(property);
                            if (oldpi != null)
                            {
                                object value = null;
                                if (oldvalue != null)
                                {
                                    value = oldpi.GetValue(oldvalue, null);
                                }

                                var typestr = propertyinfo.ToString();
                                if (typestr.Contains("System.Int32"))
                                {
                                    int v = 0;
                                    if (value != null && int.TryParse(value.ToString(), out v))
                                    {
                                        propertyinfo.SetValue(newvalue, v, null);
                                    }
                                    else if (typestr.Contains("System.Nullable`1[System.Int32]"))
                                    {
                                        propertyinfo.SetValue(newvalue, null, null);
                                    }
                                }
                                else
                                {
                                    propertyinfo.SetValue(newvalue, value, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
                    }
                }
            }
        }

        /*  test

         * /// <summary> 获取继承接口而实现添加的快捷键子项集合 </summary>
        /// <param name="currentAssembly"></param>
        /// <returns></returns>
        public static List<RoutedCommandsHotKeys> GetInterfaceHotKeys(this Assembly currentAssembly)
        {
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies().ToList();
            referencedAssemblies.Add(currentAssembly.GetName());

            List<RoutedCommandsHotKeys> datas = new List<RoutedCommandsHotKeys>();
            Parallel.ForEach(referencedAssemblies, referencedAssembly =>
            {
                var assembly = Assembly.Load(referencedAssembly);
                var data = assembly.FindInterfacesInAssembly<IHotKeys>();
                if (data != null)
                {
                    foreach (var interfaceDictionary in data)
                    {
                        var methods = interfaceDictionary.Value.GetMethods();
                        var returnType = typeof(List<RoutedCommandsHotKeys>);
                        var methodOfIHotKeys = methods.FirstOrDefault(i => i.ReturnType.Equals(returnType));
                        if (methodOfIHotKeys != null)
                        {
                            var property = interfaceDictionary.Key.FindProperty("CurrenttRoutedCommandsTarget", BindingFlags.Instance | BindingFlags.Public);
                            if (property != null)
                            {
                                try
                                {
                                    //此时线程并不在UI线程，切要优化效率因此并不做移交到UI线程处理
                                    //因此，实现接口的函数不能是需要在UI线程创建的class，例如UIElement
                                    object obj = interfaceDictionary.Key.GetConstructor(Type.EmptyTypes).Invoke(null);
                                    var value = methodOfIHotKeys.Invoke(obj, null) as List<RoutedCommandsHotKeys>;
                                    if (value != null)
                                    {
                                        datas.AddRange(value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }
            });

            return datas;
        }

         * */
    }
}