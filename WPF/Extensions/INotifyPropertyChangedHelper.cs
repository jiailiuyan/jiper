/* 迹I柳燕
 *
 * FileName:   INotifyPropertyChangedHelper.cs
 * Version:    1.0
 * Date:       2017/11/23 18:05:26
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Extensions
 * @class      INotifyPropertyChangedHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ji.LinqHelper;

namespace Ji.CommonHelper.WPF.Extensions
{
    /// <summary></summary>
    public static class INotifyPropertyChangedHelper
    {
        public static void Raise<T>(this PropertyChangedEventHandler pce, object source, Expression<Func<T>> propertyExpression)
        {
            if (pce != null)
            {
                var propertyName = ExpressionHelper.ExtractPropertyName(propertyExpression);
                if (!string.IsNullOrWhiteSpace(propertyName))
                {
                    pce(source, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}