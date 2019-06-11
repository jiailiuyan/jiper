using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ji.DataHelper;

namespace Ji.SystemHelper
{
    public static class StartupHelper
    {
        /// <summary> 增加开机启动项 </summary>
        public static void SetStartup(bool isstartup = true)
        {
            SetStartup(Assembly.LoadFile(Application.ExecutablePath), isstartup);
        }

        /// <summary> 增加开机启动项 </summary>
        /// <param name="assembly"> 执行自启的程序集 </param>
        /// <param name="isstartup"> 是否自启 </param>
        public static void SetStartup(this Assembly assembly, bool isstartup = true)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var fileinfo = new FileInfo(assembly.Location);
            var lnkfilename = fileinfo.Name + ".lnk";
            var lnkfulepath = Path.Combine(path, lnkfilename);
            if (isstartup)
            {
                assembly.WriteLnk(path);
            }
            else
            {
                File.Delete(lnkfulepath);
            }
        }

        /// <summary> 检查启动项是否存在 </summary>
        public static bool CheckStartUp()
        {
            return CheckStartUp(Assembly.LoadFile(Application.ExecutablePath));
        }

        /// <summary> 检查启动项是否存在 </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static bool CheckStartUp(this Assembly assembly)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var fileinfo = new FileInfo(assembly.Location);
            var lnkfilename = fileinfo.Name + ".lnk";
            var lnkfulepath = Path.Combine(path, lnkfilename);
            return File.Exists(lnkfulepath);
        }
    }
}