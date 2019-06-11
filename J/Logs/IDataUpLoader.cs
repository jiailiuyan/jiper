/* 迹I柳燕
 *
 * FileName:   IDataUpLoader.cs
 * Version:    1.0
 * Date:       2018/11/28 17:57:48
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J.Logs
 * @class      IDataUpLoader
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.J.Logs
{
    /// <summary>  </summary>
    public interface IDataUpLoader
    {
        string ServerUrl { get; }

        void Init();

        void ChangeUploadFloder(string uploadfloder);

        string UploadFile(string name, Stream stream, bool usegzip = true);
    }
}
