/* 迹I柳燕
 *
 * FileName:   ObservableCollectionHelper.cs
 * Version:    1.0
 * Date:       2017/8/31 17:31:43
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.WPF.Helpers
 * @class      ObservableCollectionHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.WPF.Helpers
{
    /// <summary></summary>
    public static class ObservableCollectionHelper
    {
        public static void SyncDatas<T>(this ObservableCollection<T> olddatas, IList<T> newdatas)
        {
            if (newdatas != null && newdatas.Count > 0)
            {
                var currentcount = newdatas.Count;
                for (int i = 0; i < currentcount; i++)
                {
                    var newitem = newdatas[i];
                    if (newitem != null)
                    {
                        var oldindex = olddatas.IndexOf(newitem);
                        if (oldindex != -1)
                        {
                            if (oldindex != i)
                            {
                                olddatas.Move(oldindex, i);
                            }
                        }
                        else
                        {
                            olddatas.Insert(i, newitem);
                        }
                    }
                }

                int allcount = olddatas.Count;
                for (int r = currentcount; r < allcount; r++)
                {
                    olddatas.RemoveAt(currentcount);
                }
            }
            else
            {
                olddatas.Clear();
            }
        }

        public static void StartSyncDatas<T, R>(this ObservableCollection<T> olddatas, IList<R> newdatas)
            where T : R
        {
            foreach (var item in olddatas)
            {
                newdatas.Add(item);
            }

            olddatas.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    newdatas.Clear();
                }
            };
        }
    }
}