/* 迹I柳燕
 *
 * FileName:   DragEventArgsHelper.cs
 * Version:    1.0
 * Date:       2014.03.20
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.InputHelper
 * @class      DragEventArgsHelper
 * @extends
 *
 *             对于 DragEventArgs 数据的扩展处理
 *
 *========================================

 * 
 *
 * 
 *
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ji.InputHelper
{
    public static class DragEventArgsHelper
    {
        /// <summary> 从 DragEventArgs 中获取 拖拽 IEnumerable 的数据 </summary>
        /// <param name="args"> 拖拽的参数 </param>
        /// <returns> 查询到的数据 </returns>
        public static IList<object> GetDatas(this DragEventArgs args)
        {
            List<object> datas = new List<object>();

            var data = (DataObject)args.Data;
            var dataFormateList = data.GetFormats();
            if (dataFormateList.Count() > 0)
            {
                var dataList = data.GetData(dataFormateList[0]) as IEnumerable;
                if (dataList != null)
                {
                    IEnumerator enumerator = dataList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current != null)
                        {
                            datas.Add(enumerator.Current);
                        }
                    }
                }
            }

            return datas;
        }

        ///// <summary> 从 DragEventArgs 中获取指定类型的拖拽数据 </summary>
        ///// <typeparam name="T"> 获取的数据类型 </typeparam>
        ///// <param name="args"> 拖拽的参数 </param>
        ///// <returns> 查询到的数据 </returns>
        //public static IList<T> GetDatas<T>(this DragEventArgs args) where T : class
        //{
        //    List<T> datas = new List<T>();

        //    var data = (DataObject)args.Data;
        //    var dataFormateList = data.GetFormats();
        //    if (dataFormateList.Count() > 0)
        //    {
        //        dataFormateList.ForEach(i =>
        //        {
        //            var itemData = data.GetData(i) as T;
        //            if (itemData != null)
        //            {
        //                datas.Add(itemData);
        //            }
        //        });
        //    }

        //    return datas;
        //}
    }
}