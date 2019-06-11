/* 迹I柳燕
 *
 * FileName:   IconHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.ImageHelper
 * @class      IconHelper
 * @extends
 *
 *             对于Icon的处理
 *
 *========================================

 * 
 *
 * 
 *
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ji.NativeHelper;

namespace Ji.ImageHelper
{
    public static class IconHelper
    {
        /// <summary> 通过进程的句柄获取此句柄的Icon </summary>
        /// <param name="handle"> 进程的句柄 </param>
        /// <returns> 获取到的 ImageSource </returns>
        public static ImageSource GetIconOfImageSource(this IntPtr handle)
        {
            var iconData = GetIconOfBytes(handle);
            if (iconData != null)
            {
                using (var ms = new MemoryStream(iconData))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
            }
            return null;
        }

        /// <summary> 通过进程的句柄获取此句柄的Icon </summary>
        /// <param name="handle"> 进程的句柄 </param>
        /// <returns> 获取到的 byte[] </returns>
        public static byte[] GetIconOfBytes(this IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                var process = Process.GetProcessById((int)handle);
                if (process.MainModule != null)
                {
                    IntPtr hIcon = NativeMethods.ExtractIcon(IntPtr.Zero, process.MainModule.FileName, 0);
                    if (hIcon != IntPtr.Zero)
                    {
                        var icon = Icon.FromHandle(hIcon);
                        var bitmap = icon.ToBitmap();
                        using (var ms = new MemoryStream())
                        {
                            bitmap.Save(ms, ImageFormat.Png);
                            icon.Save(ms);
                            return ms.ToArray();
                        }
                    }
                }
            }
            return null;
        }

        #region 获取Icon

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        internal static extern int DestroyIcon(IntPtr hIcon);

        #region API 参数的常量定义

        internal enum FileInfoFlags : uint
        {
            SHGFI_ICON = 0x000000100,  //  get icon
            SHGFI_DISPLAYNAME = 0x000000200,  //  get display name
            SHGFI_TYPENAME = 0x000000400,  //  get type name
            SHGFI_ATTRIBUTES = 0x000000800,  //  get attributes
            SHGFI_ICONLOCATION = 0x000001000,  //  get icon location
            SHGFI_EXETYPE = 0x000002000,  //  return exe type
            SHGFI_SYSICONINDEX = 0x000004000,  //  get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,  //  put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,  //  show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,  //  get only specified attributes
            SHGFI_LARGEICON = 0x000000000,  //  get large icon
            SHGFI_SMALLICON = 0x000000001,  //  get small icon
            SHGFI_OPENICON = 0x000000002,  //  get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,  //  get shell size icon
            SHGFI_PIDL = 0x000000008,  //  pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x000000010,  //  use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020,  //  apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040   //  Get the index of the overlay
        }

        internal enum FileAttributeFlags : uint
        {
            FILE_ATTRIBUTE_READONLY = 0x00000001,
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
            FILE_ATTRIBUTE_DEVICE = 0x00000040,
            FILE_ATTRIBUTE_NORMAL = 0x00000080,
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
        }

        #endregion API 参数的常量定义

        /// <summary>
        /// 获取文件类型的关联图标
        /// </summary>
        /// <param name="fileName">文件类型的扩展名或文件的绝对路径</param>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <returns>获取到的图标</returns>
        public static Icon GetFileIcon(string fileName, bool isLargeIcon = true)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI;
            if (isLargeIcon)
            {
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)FileInfoFlags.SHGFI_LARGEICON);
            }
            else
            {
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)FileInfoFlags.SHGFI_SMALLICON);
            }
            var icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
            DestroyIcon(shfi.hIcon);
            return icon;
        }

        /// <summary>
        /// 获取文件夹图标
        /// </summary>
        /// <returns>图标</returns>
        public static Icon GetDirectoryIcon(string floderpath, bool isLargeIcon = true)
        {
            SHFILEINFO _SHFILEINFO = new SHFILEINFO();
            IntPtr iconIntPtr;
            if (isLargeIcon)
            {
                iconIntPtr = SHGetFileInfo(floderpath, 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), ((uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_LARGEICON));
            }
            else
            {
                iconIntPtr = SHGetFileInfo(floderpath, 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), ((uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_SMALLICON));
            }
            if (iconIntPtr.Equals(IntPtr.Zero))
            {
                return null;
            }
            var icon = Icon.FromHandle(_SHFILEINFO.hIcon);
            return icon;
        }

        #endregion 获取Icon

        /// <summary>
        /// 实现bitmap到ico的转换
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <returns>转换后的指定大小的图标</returns>
        public static Icon ConvertBitmap2Ico(this Bitmap bitmap, Size size)
        {
            using (var icoBitmap = new Bitmap(bitmap, size))
            {
                var bmpicon = icoBitmap.GetHicon();
                var icon = System.Drawing.Icon.FromHandle(bmpicon);
                return icon;
            }
        }
    }
}