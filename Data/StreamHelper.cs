namespace Ji.DataHelper
{
    public static class StreamHelper
    {
        /// <summary> 转换字节长度为 M </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetRateText(this double size)
        {
            double doubleTemp;
            if (size < 1024.0)
            {
                return string.Format("{0} B", size.ToString("f1"));
            }
            if (size < 1048576.0)
            {
                doubleTemp = size / 1024.0;
                return string.Format("{0} KB", doubleTemp.ToString("f1"));
            }
            doubleTemp = size / 1048576.0;
            return string.Format("{0} MB", doubleTemp.ToString("f1"));
        }
    }
}