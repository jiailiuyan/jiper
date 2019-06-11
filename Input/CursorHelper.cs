/* 迹I柳燕
 *
 * FileName:   CursorHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.InputHelper
 * @class      CursorHelper
 * @extends
 *
 *             对于Cursor的处理
 *
 *========================================

 * 
 *
 * 
 *
 */

using System.IO;
using System.Windows.Input;

namespace Ji.InputHelper
{
    public static class CursorHelper
    {
        /// <summary> 把Stream转换为Cursor </summary>
        /// <param name="ms"> 传入的Stream </param>
        /// <returns> 中内存流中读取的Cursor </returns>
        public static Cursor ConvertToCursor(this Stream ms)
        {
            return new Cursor(ms);
        }
    }
}