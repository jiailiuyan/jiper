/* 迹I柳燕
 *
 * FileName:   ThreadPoolHelper.cs
 * Version:    1.0
 * Date:       2017/11/21 13:18:43
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Actions
 * @class      ThreadPoolHelper
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ji.CommonHelper.Actions
{
    /// <summary></summary>
    public static class ThreadPoolHelper
    {
        static ThreadPoolHelper()
        {
            ThreadPool.SetMaxThreads(100, 100);
        }

        public static void Sync(Action action)
        {
            if (action != null)
            {
                ThreadPool.QueueUserWorkItem((o) => action());
            }
        }
    }
}