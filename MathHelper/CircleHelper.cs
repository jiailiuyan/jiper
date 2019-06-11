using System;

namespace Ji.MathHelper
{
    public static class CircleHelper
    {
        /// <summary> 获取圆面积 </summary>
        /// <param name="r"> 半径 </param>
        /// <returns></returns>
        public static double GetCircleArea(double r)
        {
            return Math.PI * Math.Pow(r, 2);
        }

        /// <summary> 获取圆周长 </summary>
        /// <param name="r"> 半径 </param>
        /// <returns></returns>
        public static double GetCirclePerimete(double r)
        {
            return Math.PI * 2 * r;
        }
    }
}