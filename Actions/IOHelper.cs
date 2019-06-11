/* 迹I柳燕
 *
 * FileName:   IOHelper.cs
 * Version:    1.0
 * Date:       2017/11/22 15:02:26
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.Actions
 * @class      IOHelper
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

namespace Ji.CommonHelper.Actions
{
    /// <summary></summary>
    public class IOHelper
    {   /// <summary>
        /// 关闭流 </summary> <param name="stream"></param>
        public static void Close(Stream stream)
        {
            try
            {
                if (stream != null)
                    stream.Close();
            }
            finally
            {
            }
        }

        /// <summary> 关闭字符流 </summary>
        /// <param name="reader"></param>
        public static void Close(StreamReader reader)
        {
            try
            {
                if (reader != null)
                    reader.Close();
            }
            finally
            {
            }
        }
    }
}