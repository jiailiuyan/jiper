using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace Ji.SystemHelper
{
    public static class OpenHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        public static bool CanOpen(this Assembly assembly)
        {
            return CanOpen(assembly.ManifestModule.Name.Replace(".exe", ""));
        }

        public static bool CanOpen(string processname)
        {
            int nIndex;
            var procCurrent = Process.GetCurrentProcess();
            var procProgram = Process.GetProcessesByName(processname);

            if (procProgram.Length > 1)
            {
                for (nIndex = 0; nIndex < procProgram.Length; nIndex++)
                {
                    if (procProgram[nIndex].Id != procCurrent.Id)
                    {
                        SwitchToThisWindow(procProgram[nIndex].MainWindowHandle, true);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsRunAsAdmin()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool IsUpperVista()
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6);
        }

        public static bool CheckAdminRun(Assembly assembly)
        {
            if (!IsRunAsAdmin())
            {
                RunAsAdmin(assembly);
            }
            RunAsDefaultAdmin(assembly, false);
            return true;
        }

        public static Process RunAsAdmin(Assembly assembly)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = assembly.ManifestModule.FullyQualifiedName;
            psi.Verb = "runas";
            try
            {
                return Process.Start(psi);
            }
            catch { }
            return null;
        }

        public static Process Run(string url)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    return Process.Start(url);
                }
            }
            catch { }
            return null;
        }

        public static Process RunAsAdmin(string filepath)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = filepath;
            psi.Verb = "runas";

            // xp 解决启动请求弹窗
            //psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;

            try
            {
                return Process.Start(psi);
            }
            catch { }
            return null;
        }

        //'Software\Microsoft\Windows\CurrentVersion\Policies\System'

        /// <summary> 打开文件夹 </summary>
        /// <param name="dirPath"> 文件夹路径 </param>
        public static void OpenDirectory(string dirPath)
        {
            using (System.Diagnostics.Process.Start("explorer.exe", dirPath)) { }
        }

        /// <summary> 打开文件 </summary>
        /// <param name="filePath"> 文件路径 </param>
        public static void OpenFile(string filePath)
        {
            try
            {
                using (System.Diagnostics.Process.Start(filePath)) { }
            }
            catch { }
        }

        public static bool IsOpenUAC()
        {
            return true;
        }

        /// <summary> 取消默认赋值的管理员权限 </summary>
        /// <param name="assembly"> </param>
        /// <param name="isdefault"></param>
        /// <returns></returns>
        public static bool RunAsDefaultAdmin(Assembly assembly, bool? isdefault = null)
        {
            bool ismatch = false;
            var mechinetype = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            var rootkey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, mechinetype);
            var layers = rootkey.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            if (layers != null)
            {
                ismatch = layers.GetValue(assembly.ManifestModule.FullyQualifiedName, false, RegistryValueOptions.DoNotExpandEnvironmentNames) != null;
                if (isdefault == null) { }
                else if (isdefault.Value)
                {
                    if (!ismatch)
                    {
                        layers.SetValue(assembly.ManifestModule.FullyQualifiedName, "~ RUNASADMIN");
                        layers.Flush();
                        layers.Close();
                    }
                }
                else if (!isdefault.Value)
                {
                    if (ismatch)
                    {
                        layers.DeleteValue(assembly.ManifestModule.FullyQualifiedName, false);
                    }
                }
                layers.Close();
            }

            try
            {
                // 当没有系统管理员权限时，无法操作此注册表
                rootkey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, mechinetype);
                layers = rootkey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
                if (layers != null)
                {
                    var names = layers.GetValueNames();
                    if (names != null)
                    {
                        foreach (var item in names)
                        {
                            ismatch = assembly.ManifestModule.FullyQualifiedName == new FileInfo(item).FullName;
                            if (ismatch)
                            {
                                break;
                            }
                        }
                    }

                    if (isdefault == null) { }
                    else if (isdefault.Value)
                    {
                        if (!ismatch)
                        {
                            layers.SetValue(assembly.ManifestModule.FullyQualifiedName, "~ RUNASADMIN");
                            layers.Flush();
                            layers.Close();
                        }
                    }
                    else if (!isdefault.Value)
                    {
                        if (ismatch)
                        {
                            layers.DeleteValue(assembly.ManifestModule.FullyQualifiedName, false);
                        }
                    }
                    layers.Close();
                }
            }
            catch { ismatch = false; }

            return ismatch;
        }
    }
}