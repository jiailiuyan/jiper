using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ji.DataHelper
{
    public static class TypeHelper
    {
        /// <summary> 获取指定类型的指定属性 </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static R GetAttribute<T, R>(this T data, string name) where R : Attribute
        {
            return (from r in (from i in data.GetType().GetFields() where i.Name.Equals(name) select i) let ca = r.GetCustomAttributes(true).FirstOrDefault(c => (c as R) != null) where (ca != null) select ca).FirstOrDefault() as R;
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var info = GetPropertyInfo(propertyExpression);
            return info.Name;
        }
        public static string GetPropertyName<TObj, TProperty>(Expression<Func<TObj, TProperty>> propertyExpression)
        {
            var info = GetPropertyInfo(propertyExpression);
            return info.Name;
        }
        public static PropertyInfo GetPropertyInfo<TObj, TProperty>(Expression<Func<TObj,TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");
            return GetPropertyInfo(propertyExpression.Body);
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");
            return GetPropertyInfo(propertyExpression.Body);
        }
        public static PropertyInfo GetPropertyInfo(Expression expression)
        {
            MemberExpression body = expression as MemberExpression;

            if (body == null)
                throw new ArgumentException("Invalid argument", "propertyExpression");
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == (PropertyInfo)null)
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            return member;
        }
        public static T GetAttribute<T>(MemberInfo filedInfo) where T : Attribute
        {
            var attributes = filedInfo.GetCustomAttributes(true);
            return attributes.FirstOrDefault(x => (x as T) != null) as T;
        }

        public static List<T> GetAttributes<T>(MemberInfo filedInfo) where T : Attribute
        {
            var attributes = filedInfo.GetCustomAttributes(true);
            return attributes.Where(x => (x as T) != null).Cast<T>().ToList();
        }
    }

    public static class TypeHelperExt
    {
        public static string GetPropertyName<TObj, TProperty>(this TObj obj, Expression<Func<TObj, TProperty>> propertyExpression)
        {
            var info = TypeHelper.GetPropertyInfo(propertyExpression);
            return info.Name;
        }
    }
}