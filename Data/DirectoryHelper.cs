using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ji.DataHelper
{
    [Serializable]
    public class FileData
    {
        public bool IsReceive { get; set; }

        public bool IsSend { get; set; }

        public string Name { get; set; }

        public string Root { get; set; }

        public int Length { get; set; }

        public string FullName { get; set; }

        public string ParentName { get; set; }

        /// <summary>
        /// 摒弃的，现不用MD5
        /// </summary>
        public string MD5 { get; set; }

        public string NameOnly { get; set; }

        public FileData()
        {
            this.IsSend = this.IsReceive = false;
        }
    }

    [Serializable]
    public class FloderData
    {
        public List<FileData> FileDataList { get; set; }

        public string FullPath { get; set; }

        public int Length { get; set; }

        public FloderData()
        {
            this.FileDataList = new List<FileData>();
        }
    }

    public static class DirectoryHelper
    {
        public static bool MakeExsit(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch { }
            return false;
        }

        public static List<FileData> GetAllFiles(string path)
        {
            var DirectoryInfo = new System.IO.DirectoryInfo(path);
            return DirectoryHelper.GetAllFiles("", DirectoryInfo.Name, DirectoryInfo.Parent.FullName).ToList();
        }

        public static List<string> GetAllDirectories(string path)
        {
            var DirectoryInfo = new System.IO.DirectoryInfo(path);
            return DirectoryHelper.GetAllDirectories("", DirectoryInfo.Name, DirectoryInfo.Parent.FullName).ToList();
        }

        public static IEnumerable<string> GetAllDirectories(string path, string parentname = "", string rootname = "")
        {
            var realpath = Path.Combine(rootname, parentname, path);
            var di = new DirectoryInfo(realpath);
            if (di.Exists)
            {
                //if (di..Length > 0)
                {
                    yield return di.FullName;
                }

                foreach (var item in di.GetDirectories())
                {
                    var pn = string.IsNullOrWhiteSpace(path) ? item.Name : path + "\\" + item.Name;
                    var fl = GetAllDirectories(pn, parentname, rootname).ToList();
                    foreach (var filedatainfo in fl)
                    {
                        if (filedatainfo != null)
                        {
                            yield return filedatainfo;
                        }
                    }
                }
            }
        }

        public static FileInfo[] GetAllFileInfo(string path, string searchPattern = "")
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                if (string.IsNullOrWhiteSpace(searchPattern)) { return di.GetFiles(); }
                else { return di.GetFiles(searchPattern); }
            }
            return null;
        }

        public static IEnumerable<FileData> GetAllFiles(string path, string parentname = "", string rootname = "")
        {
            var realpath = Path.Combine(rootname, parentname, path);
            var di = new DirectoryInfo(realpath);
            if (di.Exists)
            {
                foreach (var item in di.GetFiles())
                {
                    var p = Path.Combine(path, parentname);
                    var f = GetFileDataInfo(item.FullName, path, parentname, rootname);
                    if (f != null)
                    {
                        yield return f;
                    }
                }
                foreach (var item in di.GetDirectories())
                {
                    var pn = string.IsNullOrWhiteSpace(path) ? item.Name : path + "\\" + item.Name;
                    var fl = GetAllFiles(pn, parentname, rootname).ToList();
                    foreach (var filedatainfo in fl)
                    {
                        if (filedatainfo != null)
                        {
                            yield return filedatainfo;
                        }
                    }
                }
            }
        }

        public static FileData GetFileDataInfo(string filepath, string path, string parentname, string root)
        {
            var fileinfo = new FileInfo(filepath);
            if (fileinfo.Exists)
            {
                var fd = new FileData();
                //fd.MD5 = MD5Helper.GetMD5(fileinfo.FullName);
                fd.Length = (int)fileinfo.Length;
                fd.FullName = fileinfo.FullName;
                fd.NameOnly = fileinfo.Name.ReplaceLast(fileinfo.Extension, "");
                fd.ParentName = parentname;
                fd.Root = root;
                fd.Name = string.IsNullOrWhiteSpace(path) ? fileinfo.Name : path + "\\" + fileinfo.Name;
                return fd;
            }
            return null;
        }

        public static string PathCombin(this DirectoryInfo directory, string path)
        {
            var realdir = new DirectoryInfo(directory.FullName);
            var directroysplit = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            int dircount = directroysplit.Count() - 1;
            if (dircount >= 0)
            {
                var name = directroysplit[dircount];
                for (int i = dircount; i > 0; i--)
                {
                    if (directroysplit[i] == "..")
                    {
                        realdir = realdir.Parent;
                    }
                    else if (i != dircount && directroysplit[i] != ".")
                    {
                        name = directroysplit[i] + "\\" + name;
                    }
                }
                return realdir.FullName + "\\" + name;
            }
            return null;
        }

        public static string PathCombinSimple(this DirectoryInfo directory, string path)
        {
            var fileinfo = new FileInfo(path);
            var firstdirs = new List<DirectoryInfo>();

            while (true)
            {
                firstdirs.Add(directory);
                if (directory.FullName.Equals(directory.Root.FullName))
                {
                    break;
                }
                directory = directory.Parent;
            }

            var filedir = fileinfo.Directory;
            int count = firstdirs.Count;
            string filenamedir = "";
            while (true)
            {
                for (int i = 0; i < count; i++)
                {
                    if (filedir.FullName.Equals(firstdirs[i].FullName))
                    {
                        var realpath = "";
                        for (int j = i; j > 0; j--)
                        {
                            realpath += "..\\";
                        }
                        return realpath + filenamedir + "\\" + fileinfo.Name;
                    }
                }

                if (filedir.Parent == null)
                {
                    return null;
                }
                filenamedir = !string.IsNullOrWhiteSpace(filenamedir) ? (filedir.Name + "\\" + filenamedir) : filedir.Name;
                filedir = filedir.Parent;
            }
        }

        /// <summary> 将目录及其内容拷贝到新位置。 </summary>
        /// <param name="dir"></param>
        /// <param name="targetDirName">目标路径</param>
        public static void Copy(this DirectoryInfo dirInfo, string targetDirName, FileAttributes attributes = FileAttributes.Normal)
        {
            try
            {
                if (!Directory.Exists(targetDirName))
                {
                    Directory.CreateDirectory(targetDirName);
                }

                var tagInfo = new DirectoryInfo(targetDirName);
                tagInfo.Attributes = attributes;
                //获取子目录
                var dirs = Directory.GetDirectories(dirInfo.FullName);
                //获取子文件文件
                var files = dirInfo.GetFiles();
                if (dirs.Length > 0)
                {
                    foreach (string dirPath in dirs)
                    {
                        var dir = new DirectoryInfo(dirPath);
                        var attri = dir.Attributes;
                        dir.Copy(System.IO.Path.Combine(targetDirName, dir.Name), attri);
                    }
                }
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        file.CopyTo(System.IO.Path.Combine(targetDirName, file.Name), true);
                    }
                }
            }
            catch { }
        }

        public static bool IsDirectory(this FileSystemInfo info)
        {
            return info.Attributes.HasFlag(FileAttributes.Directory);
        }
    }
}