using System;
using System.Collections.Generic;
using System.Linq;
using Ji.CommonHelper.Attributes;

namespace Ji.DataHelper
{
    public static class EnumHelper
    {
        /// <summary> 获取枚举的声明名称 </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetName(this Enum data)
        {
            return Enum.GetName(data.GetType(), data);
        }

        public static Dictionary<object, string> GetOrderDescriptions(this object data)
        {
            Dictionary<object, OrderDescriptionAttribute> dirs = new Dictionary<object, OrderDescriptionAttribute>();
            var type = data.GetType();
            foreach (var i in type.GetFields())
            {
                if (i.FieldType.Equals(type))
                {
                    var enumdata = Enum.Parse(type, i.Name);
                    var attribute = enumdata.GetAttribute<object, OrderDescriptionAttribute>(i.Name);
                    if (attribute != null)
                    {
                        if (attribute.CanView && !dirs.ContainsKey(enumdata))
                        {
                            dirs.Add(enumdata, attribute);
                        }
                    }
                }
            }
            return dirs.OrderBy(x => x.Value.Order).ToDictionary(x => x.Key, x => x.Value.Description);
        }
        public static Dictionary<object, string> GetOrderDescriptions(Type type)
        {
            Dictionary<object, OrderDescriptionAttribute> dirs = new Dictionary<object, OrderDescriptionAttribute>();
            
            foreach (var i in type.GetFields())
            {
                if (i.FieldType.Equals(type))
                {
                    var enumdata = Enum.Parse(type, i.Name);
                    var attribute = enumdata.GetAttribute<object, OrderDescriptionAttribute>(i.Name);
                    if (attribute != null)
                    {
                        if (attribute.CanView && !dirs.ContainsKey(enumdata))
                        {
                            dirs.Add(enumdata, attribute);
                        }
                    }
                }
            }
            return dirs.OrderBy(x => x.Value.Order).ToDictionary(x => x.Key, x => x.Value.Description);
        }
        //public static Dictionary<T, string> GetAllEnumDescriptions<T>(this T data) where T : struct
        //{
        //    return GetAllEnumDescriptions<T, DescriptionAttribute>(data);
        //}

        ///// <summary> 获取枚举的描述属性 </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public static Dictionary<T, string> GetAllEnumDescriptions<T, R>(this T data)
        // where R : DescriptionAttribute
        //  where T : struct
        //{
        //    Dictionary<T, string> dirs = new Dictionary<T, string>();
        //    var type = data.GetType();
        //    foreach (var i in type.GetFields())
        //    {
        //        if (i.FieldType.Equals(type))
        //        {
        //            var enumdata = (T)Enum.Parse(type, i.Name);
        //            var attribute = enumdata.GetAttribute<T, R>(i.Name);
        //            if (attribute != null)
        //            {
        //                if (!dirs.ContainsKey(enumdata))
        //                {
        //                    dirs.Add(enumdata, attribute.Description);
        //                }
        //            }
        //        }
        //    }
        //    return dirs;
        //}
    }
}