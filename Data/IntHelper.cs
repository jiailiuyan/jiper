/* 迹I柳燕
 *
 * FileName:   IntHelper.cs
 * Version:    1.0
 * Date:       2017/9/21 17:23:24
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Data
 * @class      IntHelper
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
    public static class IntHelper
    {
        public static int GetEven2(this int obj)
        {
            return (obj + 1) & ~1;
        }

        public static int GetEven4(this int obj)
        {
            return (obj + 3) & ~3;
        }
    }
}