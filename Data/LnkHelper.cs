using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.DataHelper
{
    public static class LnkHelper
    {
        public static string GetLnkRealtivePath(string path)
        {
            var vShellLink = (LnkData.IShellLink)new LnkData.ShellLink();
            var vPersistFile = vShellLink as System.Runtime.InteropServices.ComTypes.IPersistFile;
            vPersistFile.Load(path, 0);
            var vStringBuilder = new StringBuilder(260);
            LnkData.WIN32_FIND_DATA vWIN32_FIND_DATA;
            vShellLink.GetPath(vStringBuilder, vStringBuilder.Capacity, out vWIN32_FIND_DATA, LnkData.SLGP_FLAGS.SLGP_RAWPATH);
            return vStringBuilder.ToString();
        }

        /// <summary>
        /// var startuppath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        /// typeof(MainWindow).Assembly.WriteLnk(startuppath);
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="savepath"></param>
        public static void WriteLnk(this Assembly assembly, string savepath = "")
        {
            var fileinfo = new FileInfo(assembly.Location);

            var vShellLink = (LnkData.IShellLink)new LnkData.ShellLink();
            var vPersistFile = vShellLink as System.Runtime.InteropServices.ComTypes.IPersistFile;
            vShellLink.SetPath(fileinfo.FullName);
            vShellLink.SetWorkingDirectory(fileinfo.Directory.FullName);
            vShellLink.SetDescription(fileinfo.Name + "动快捷方式");
            vPersistFile.Save(Path.Combine(savepath, fileinfo.Name + ".lnk"), false);
        }
    }

    public class LnkData
    {
        [Flags()]
        public enum SLR_FLAGS
        {
            SLR_NO_UI = 0x1,
            SLR_ANY_MATCH = 0x2,
            SLR_UPDATE = 0x4,
            SLR_NOUPDATE = 0x8,
            SLR_NOSEARCH = 0x10,
            SLR_NOTRACK = 0x20,
            SLR_NOLINKINFO = 0x40,
            SLR_INVOKE_MSI = 0x80
        }

        [Flags()]
        public enum SLGP_FLAGS
        {
            SLGP_SHORTPATH = 0x1,
            SLGP_UNCPRIORITY = 0x2,
            SLGP_RAWPATH = 0x4
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WIN32_FIND_DATA
        {
            public int dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;

            private const int MAX_PATH = 260;
        }

        [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
        public interface IShellLink
        {
            void GetPath([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATA pfd, SLGP_FLAGS fFlags);

            void GetIDList(out IntPtr ppidl);

            void SetIDList(IntPtr pidl);

            void GetDescription([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);

            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

            void GetWorkingDirectory([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);

            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

            void GetArguments([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);

            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

            void GetHotkey(out short pwHotkey);

            void SetHotkey(short wHotkey);

            void GetShowCmd(out int piShowCmd);

            void SetShowCmd(int iShowCmd);

            void GetIconLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

            void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);

            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        [ComImport(), Guid("00021401-0000-0000-C000-000000000046")]
        public class ShellLink
        { }
    }
}