/* 迹I柳燕
 *
 * FileName:   ILogSave.cs
 * Version:    1.0
 * Date:       2018/11/27 12:43:47
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ksy.Core.Model.ILog
 * @class      ILogSave
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
    public interface ILogSave
    {
        void Start(ILogSetting setting);

        void Save(string msg);

        void Save(Exception ex);

        void Save(Exception ex, string msg);

        void Debug(string msg);

        void Stop();
    }
}
