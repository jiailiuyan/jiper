namespace Ji.MathHelper
{
    public static class SquareHelper
    {
        /// <summary> 获取方形面积 </summary>
        public static double GetSquareArea(double w, double h)
        {
            return w * h;
        }

        /// <summary> 获取方形周长 </summary>
        public static double GetSquarePerimete(double w, double h)
        {
            return 2 * (w + h);
        }
    }
}