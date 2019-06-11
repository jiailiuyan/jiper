using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary> FileEncoding 的摘要说明 </summary>
namespace Ji.DataHelper
{
    /// <summary> 获取文件的编码格式 </summary>
    public static class FileHelper
    {

        //System.IO.Path.GetFileNameWithoutExtension(str);

        /// <summary> 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 </summary> <param name=“FILE_NAME“>文件路径</param> <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetEncodingType(this FileInfo fileinfo)
        {
            using (FileStream fs = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read))
            {
                return GetType(fs);
            }
        }

        /// <summary> 通过给定的文件流，判断文件的编码类型 </summary> <param name=“fs“>文件流</param> <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(FileStream fs)
        {
            var Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            var UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            //带BOM
            var UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };
            var reVal = Encoding.Default;

            var r = new BinaryReader(fs, System.Text.Encoding.Default);
            var ss = r.ReadBytes((int)fs.Length);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// <summary> 判断是否是不带 BOM 的 UTF8 格式 </summary> <param name=“data“></param> <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            //计算当前正分析的字符应还有的字节数
            int charByteCounter = 1;
            //当前分析的字节.
            byte curByte;
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        /// <summary> 根据文件编码格式，自动读取文件的内容 </summary>
        /// <param name="fileinfo"></param>
        /// <returns></returns>
        public static string ReadText(this FileInfo fileinfo)
        {
            return File.ReadAllText(fileinfo.FullName, GetEncodingType(fileinfo));
        }

        /// <summary> 获取文件名称（不含后缀名） </summary>
        /// <param name="fileinfo"></param>
        /// <returns></returns>
        public static string GetName(this FileInfo fileinfo)
        {
            return fileinfo.Name.ReplaceLast(fileinfo.Extension, "");
        }

        /// <summary> 确保文件存在，如果不存在则新建 此项非绝对，比如 1，在没有权限的文件夹下创建 2，此文件无后缀名，且已经存在与文件同名的文件夹时 </summary>
        /// <param name="fullpath"></param>
        public static void CreatFile(string fullpath)
        {
            CreatFile(new FileInfo(fullpath));
        }

        /// <summary> 确保文件存在，如果不存在则新建 此项非绝对，比如 1，在没有权限的文件夹下创建 2，此文件无后缀名，且已经存在与文件同名的文件夹时 </summary>
        /// <param name="fileinfo"></param>
        public static void CreatFile(this FileInfo fileinfo)
        {
            if (!fileinfo.Exists)
            {
                if (!fileinfo.Directory.Exists)
                {
                    fileinfo.Directory.Create();
                }
                using (var fs = File.Create(fileinfo.FullName)) fs.Close();
            }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        public static System.Drawing.Image GetFileImage(string fileName)
        {
            System.Drawing.Image image = null;
            var info = new FileInfo(fileName);
            if (info.Exists)
            {
                if (info.Extension.Equals(".png") || info.Extension.Equals(".jpg") || info.Extension.Equals(".bmp"))
                {
                    image = (new System.Drawing.Bitmap(info.FullName)).GetThumbnailImage(40, 40, ThumbnailCallback, IntPtr.Zero);
                }
            }
            return image ?? System.Drawing.Icon.ExtractAssociatedIcon(fileName).ToBitmap();
        }

        public static bool IsFileOrDirectory(this string path)
        {
            return IsDirectory(path) != null;
        }

        public static bool? IsDirectory(this string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        return true;
                    }

                    if (File.Exists(path))
                    {
                        var fileinfo = new FileInfo(path);
                        if ((fileinfo != null && fileinfo.Exists))
                        {
                            return false;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch { }
            }
            return null;
        }

        public static string GetLnkRealtivePath(this string path)
        {
            var vShellLink = (LnkData.IShellLink)new LnkData.ShellLink();
            var vPersistFile = vShellLink as System.Runtime.InteropServices.ComTypes.IPersistFile;
            vPersistFile.Load(path, 0);
            var vStringBuilder = new StringBuilder(260);
            LnkData.WIN32_FIND_DATA vWIN32_FIND_DATA;
            vShellLink.GetPath(vStringBuilder, vStringBuilder.Capacity, out vWIN32_FIND_DATA, LnkData.SLGP_FLAGS.SLGP_RAWPATH);
            return vStringBuilder.ToString();
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly static IntPtr HFILE_ERROR = new IntPtr(-1);

        public static bool CanUseFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return false;
            }
            IntPtr vHandle = _lopen(filepath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return false;
            }
            CloseHandle(vHandle);

            return true;
        }
    }
}