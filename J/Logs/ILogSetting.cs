/* 迹I柳燕
 *
 * FileName:   ILogSetting.cs
 * Version:    1.0
 * Date:       2018/11/27 13:02:07
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J.Logs
 * @interface      ILogSetting
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>  </summary>
    public interface ILogSetting
    {
        string LogFloder { get; }

        bool IsDebug { get; }
    }
}
