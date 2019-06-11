/* 迹I柳燕
 *
 * FileName:   INotifyAll.cs
 * Version:    1.0
 * Date:       2017/11/28 迹 16:17:04
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Extensions
 * @interface      INotifyAll
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ji.CommonHelper.WPF.Extensions
{
    /// <summary></summary>
    public interface INotifyAll : INotifyPropertyChanged
    {

    }

    public static class INotifyAllHelper
    {
        #region Methods

        /// <summary> 触发所有具有 Get 的属性通知 </summary>
        /// <param name="data"></param>
        /// <param name="pce"> </param>
        public static void RaiseAll(this INotifyAll data)
        {
            var methods = data.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            if (methods != null)
            {
                var raisemethod = methods.FirstOrDefault(i => i.ToString() == "Void RaisePropertyChanged(System.String)");
                if (raisemethod != null)
                {
                    var properties = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    if (properties != null)
                    {
                        foreach (var item in properties)
                        {
                            if (item.CanRead)
                            {
                                raisemethod.Invoke(data, new object[1] { item.Name });
                            }
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}