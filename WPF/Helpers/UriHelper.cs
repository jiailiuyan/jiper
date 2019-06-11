using System;
using System.Reflection;

namespace Ji.WPFHelper.DataHelper
{
    public static class UriHelper
    {
        public static Uri GetUri(this Assembly assembly, string relativepath)
        {
            return new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + relativepath, UriKind.RelativeOrAbsolute);
        }
    }
}