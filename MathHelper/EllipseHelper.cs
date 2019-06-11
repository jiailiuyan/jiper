using System;
using System.Windows;

namespace Ji.MathHelper
{
    /// <summary> 椭圆焦点类型  </summary>
    public class EllipseArea
    {
        /// <summary> 左焦点 </summary>
        public Point Center1 { get; set; }

        /// <summary> 右焦点 </summary>
        public Point Center2 { get; set; }

        /// <summary> 椭圆圆周点到两焦点定长 </summary>
        public double Length { get; set; }

        /// <summary> 判断点是否在椭圆内 </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInclude(Point point)
        {
            var length = Math.Sqrt(Math.Pow(point.X - Center1.X, 2) + Math.Pow(point.Y - Center1.Y, 2)) + Math.Sqrt(Math.Pow(Center2.X - point.X, 2) + Math.Pow(Center2.Y - point.Y, 2));
            var maxlength = Math.Pow(Length, 1);
            return length <= maxlength;
        }
    }

    /// <summary> 椭圆帮助类 </summary>
    public static class EllipseHelper
    {
        /// <summary> 计算椭圆焦点 </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static EllipseArea GetEllipseArea(Point location, double width, double height, double angle = 0)
        {
            if (width != 0 && height != 0)
            {
                if (height > width)
                {
                    angle += 90;
                    var tw = width;
                    var th = height;

                    width = th;
                    height = tw;
                }

                //以左上角为原点计算
                var centerx = width / 2;
                var centery = height / 2;

                var currentlocationx = -width / 2;
                var currentlocationy = -height / 2;
                var currentlocationangle = Math.Atan2(currentlocationy, currentlocationx);
                var currentlocationrealangle = currentlocationangle + angle * Math.PI / 180d;
                var currentlocationr = Math.Sqrt(Math.Pow(currentlocationx, 2) + Math.Pow(currentlocationy, 2));
                var rx = Math.Cos(currentlocationrealangle) * currentlocationr;
                var currentreallocationx = location.X + centerx + rx;

                var ry = Math.Sin(currentlocationrealangle) * currentlocationr;
                var currentreallocationy = location.Y + centery + ry;

                location = new Point(currentreallocationx, currentreallocationy);

                var lengthcenter = Math.Sqrt(Math.Pow(centerx, 2) - Math.Pow(centery, 2));

                // 计算中心点1
                var x1 = centerx - lengthcenter;
                var y1 = centery;

                // 计算中心点2
                var x2 = centerx + lengthcenter;
                var y2 = centery;

                // 中心点1旋转半径
                var r1 = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
                var angle1 = Math.Atan2(y1, x1) * 180 / Math.PI;
                var realangle1 = angle1 + angle;
                var center1x = Math.Cos(realangle1 * Math.PI / 180d) * r1;
                var center1y = Math.Sin(realangle1 * Math.PI / 180d) * r1;
                // 相对 Location 和 Angle 坐标系的点坐标
                var realcenter1x = location.X + center1x;
                var realcenter1y = location.Y + center1y;

                // 中心点2旋转半径
                var r2 = Math.Sqrt(Math.Pow(x2, 2) + Math.Pow(y2, 2));
                var angle2 = Math.Atan2(y2, x2) * 180 / Math.PI;
                var realangle2 = angle2 + angle;
                var center2x = Math.Cos(realangle2 * Math.PI / 180d) * r2;
                var center2y = Math.Sin(realangle2 * Math.PI / 180d) * r2;
                // 相对 Location 和 Angle 坐标系的点坐标
                var realcenter2x = location.X + center2x;
                var realcenter2y = location.Y + center2y;

                return new EllipseArea() { Length = width, Center1 = new Point(realcenter1x, realcenter1y), Center2 = new Point(realcenter2x, realcenter2y) };
            }
            return null;
        }
    }
}