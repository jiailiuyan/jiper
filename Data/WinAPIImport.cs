using System.Runtime.InteropServices;

namespace Helper.JilyData
{
    /// <summary>
    /// 引用WindowsApi调用接口
    /// </summary>
    public static class WinAPIImport
    {
        /// <summary>
        /// 调用winAPI,使用自然排序,对字符串进行排序
        /// </summary>
        /// <param name="psz1"></param>
        /// <param name="psz2"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
}
