using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ji.LinqHelper
{
    public static class ExpressionHelper
    {
        /// <summary> 获取当前的委托名称 </summary>
        /// <typeparam name="T">当前发送线程通知的类型</typeparam>
        /// <param name="propertyExpression">反射获取类型名称的委托</param>
        /// <returns> 当前的委托名称 </returns>
        public static string ExtractPropertyName<T>(this Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression != null)
            {
                var memberExpression = propertyExpression.Body as MemberExpression;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                var getMethod = propertyInfo.GetGetMethod(true);
                return memberExpression.Member.Name;
            }
            return string.Empty;
        }
    }
}