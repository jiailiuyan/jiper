/* 迹I柳燕
 *
 * FileName:   IDataStatistic.cs
 * Version:    1.0
 * Date:       2018/11/27 18:56:13
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J.Logs
 * @interface  IDataStatistic
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
    public interface IDataStatistic
    {
        void Start();

        void Send(DataStatisticItem data);

        void Stop();
    }

    public class DataStatisticItem
    {
        public DataStatisticType StatisticType { get; set; }

        public object Data { get; set; }

        public override string ToString()
        {
            return $"[{Enum.GetName(typeof(DataStatisticType), StatisticType)}] {Data + ""}";
        }
    }


    public enum DataStatisticType
    {
        None,

        Start,

        Crash,

        Action,

        MenuOne,

        MenuTwo,
    }

}
