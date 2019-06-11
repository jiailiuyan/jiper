/* 迹I柳燕
 *
 * FileName:   SwitchHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  JilyHelper
 * @class      SwitchHelper
 * @extends
 *
 *             对于 Switch 的扩展，主要用于不写那么多的case
 *             在于对于具有参数的函数直接使用 Delegate 进行调用
 *             需要注意的是 Delegate 的参数顺序和个数
 *========================================

 * (http://www.jiailiuyan.com)
 *
 * 
 *
 */

using System;

namespace Ji.DataHelper
{
    public static class SwitchHelper
    {
        /// <summary> 创建 switch 中的单项 case 判断方法
        /// Ps，强烈不建议使用闭包进行数据的处理，正在研究闭包时参数传递不正确的问题</summary>
        /// <typeparam name="T">  </typeparam>
        /// <param name="enumData">  </param>
        /// <param name="action">  </param>
        /// <param name="paramsdatas"> 需要注意的是 Delegate 的参数顺序和个数 </param>
        /// <returns>  </returns>
        public static SwitchItem<T> CreatSwitchItem<T>(this T enumData, Delegate action, params object[] paramsdatas)
        {
            return new SwitchItem<T>(enumData, action, paramsdatas);
        }

        /// <summary> 执行 switch 中 case 的判断</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="judge"></param>
        /// <param name="items"></param>
        /// <returns> 返回是否执行其中项 </returns>
        public static bool DoSwitchAction<T>(this T judge, params SwitchItem<T>[] items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.Data != null && item.Data.Equals(judge))
                    {
                        item.Delegate.Method.Invoke(item, item.ActionParamsDatas);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary> switch Item 模版 </summary>
        /// <typeparam name="T"></typeparam>
        public class SwitchItem<T>
        {
            private SwitchItem(T enumData)
            {
                this.Data = enumData;
            }

            public SwitchItem(T enumData, Delegate delegateData, params object[] paramsdatas)
                : this(enumData)
            {
                this.Delegate = delegateData;
                this.ActionParamsDatas = paramsdatas;
            }

            public T Data { get; set; }

            public object[] ActionParamsDatas { get; set; }

            public Delegate Delegate { get; set; }
        }
    }
}