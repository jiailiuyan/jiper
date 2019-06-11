/* 迹I柳燕
 *
 * FileName:   IListHelper.cs
 * Version:    1.0
 * Date:       2017/8/31 17:27:21
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Data
 * @class      IListHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Data
{
    /// <summary></summary>
    public static class IListHelper
    {
        public static void SyncList<T>(this IList<T> olddatas, IList<T> newdatas)
        {
            List<string> a = new List<string>();

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

                // 迹 修改发送状态的优惠券不会被清除
                var sendcoupon = this.CollectModelList.FirstOrDefault(i => i.SendStatus >= GroupSendType.InSending && i.SendStatus <= GroupSendType.WaitSendEndBoundary);
                if (sendcoupon != null)
                {
                    int ci = this.CollectModelList.IndexOf(sendcoupon);
                    if (ci >= MaxViewCount)
                    {
                        currentcount++;
                    }
                    this.CollectModelList.Move(ci, 0);
                }

                int allcount = this.CollectModelList.Count;
                for (int r = currentcount; r < allcount; r++)
                {
                    this.CollectModelList.RemoveAt(currentcount);
                }
            }
            else
            {
                CollectModelList.Clear();
            }
        }
    }
}