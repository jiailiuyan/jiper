/* 迹I柳燕
 *
 * FileName:   TaskHelper.cs
 * Version:    1.0
 * Date:       2017/11/22 14:59:08
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Actions
 * @class      TaskHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Actions
{
    /// <summary></summary>
    public class TaskHelper
    {  /// <summary>
       /// 取消后台任务 </summary> <param name="worker"></param>
        public static void CancelWorker(BackgroundWorker worker)
        {
            try
            {
                if (worker != null)
                    worker.CancelAsync();
            }
            catch { }
        }
    }
}