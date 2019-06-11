using System;

namespace Ji.MathHelper
{
    public static class SectorHelper
    {
        /// <summary> 获取扇形面积 </summary>
        public static double GetCircleAreaByAngle(double r, double angle)
        {
            return Math.PI * Math.Pow(r, 2) * (angle / 360d);
        }

        /// <summary> 获取扇形面积 </summary>
        public static double GetCircleAreaByArc(double r, double arc)
        {
            return arc * r / 2;
        }

        /// <summary> 获取扇形弧长 </summary>
        public static double GetArcLength(double r, double angle)
        {
            return (angle / 360d) * 2 * Math.PI * r;
        }

        /// <summary> 获取扇形周长 </summary>
        public static double GetArcPerimete(double r)
        {
            return Math.PI * 2 * r;
        }
    }
}