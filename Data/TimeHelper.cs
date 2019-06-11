using System;

namespace Ji.DataHelper
{
    public static class TimeHelper
    {
        public static int ToIntByDay(this DateTime dateTime)
        {
            try
            {
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                return Convert.ToInt32(dateTime.ToOADate());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary> 转换标准时间为时间戳 </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary> 转换标准时间为时间戳，包含毫秒部分，占用三位 </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStampWithMs(this DateTime dt)
        {
            return long.Parse(dt.GetTimeStamp() + "" + DateTime.Now.Millisecond);
        }
    }
}