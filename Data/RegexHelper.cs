using System.Text.RegularExpressions;

namespace Ji.DataHelper
{
    public static class RegexHelper
    {
        /// <summary> 是否为合法文件名称 </summary>
        public static bool IsLegalFileName(string name)
        {
            string pattern = "[\\*\\\\/:?<>|\"]";
            bool isLegal = !Regex.IsMatch(name, pattern);
            return isLegal;
        }
    }
}