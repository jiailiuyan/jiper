using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Ji.DataHelper;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public partial class OpenFilesDialog : OpenFileDialogEx
    {
        /// <summary> 选择文件夹 </summary>
        public override string Lable_SelectTexg { get { return "选择文件"; } }

        public string FolderParent = string.Empty;
        public List<string> FilesPath = new List<string>();

        public OpenFilesDialog()
        {
            InitializeComponent();

            this.OpenDialog.Multiselect = true;
            this.OpenDialog.Filter = "All Files|*.*";

            this.FilesSelected += OpenFilesDialog_FilesSelected;

            this.FileNameChanged += OpenFilesDialog_FileNameChanged;
            this.FolderNameChanged += OpenFilesDialog_FolderNameChanged;

            this.OpenDialog.FileOk += OpenDialog_FileOk;
        }

        private void OpenDialog_FileOk(object sender, CancelEventArgs e)
        {
            //在此处理由于双击文件触发的关闭，此时需要通知选中的文件
            OnClosingDialog();
            OnSelectedNameChanged();
        }

        private void OpenFilesDialog_FolderNameChanged(OpenFileDialogEx sender, string filePath)
        {
            cando = true;
            fileConvert.Clear();
        }

        private List<string> fileConvert = new List<string>();
        private bool cando = false;

        private void OpenFilesDialog_FileNameChanged(OpenFileDialogEx sender, string filePath)
        {
            cando = true;
            fileConvert.Clear();
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                if (filePath.IsFileOrDirectory())
                {
                    FolderParent = string.Empty;
                    fileConvert.Add(filePath);
                }
                else
                {
                    var lastindex = filePath.LastIndexOf("\\");
                    if (lastindex != -1 && lastindex <= filePath.Length)
                    {
                        var filestr = new string(filePath.Skip(lastindex).ToArray());
                        var filelist = filestr.Split(new string[] { "\\", "/", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (filelist != null && filelist.Count() > 0)
                        {
                            var first = filelist.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i));
                            int firstindex = filePath.IndexOf(first);
                            FolderParent = firstindex != -1 ? new string(filePath.Take(firstindex - 1).ToArray()) : string.Empty;

                            filelist.ToList().ForEach(i =>
                            {
                                if (!string.IsNullOrWhiteSpace(i))
                                {
                                    fileConvert.Add(i);
                                }
                            });
                        }
                    }
                }
            }
        }

        private void OpenFilesDialog_FilesSelected(OpenFileDialogEx sender, IntPtr handle)
        {
            if (!cando)
            {
                return;
            }

            StringBuilder floderparent = new StringBuilder(256);
            NativeMethods.SendMessage(NativeMethods.GetParent(handle), (int)DialogChangeProperties.CDM_GETFOLDERPATH, (int)256, floderparent);

            if (!string.IsNullOrWhiteSpace(floderparent.ToString()))
            {
                FolderParent = floderparent.ToString();
            }

            FilesPath.Clear();

            List<string> names = new List<string>();
            this.textBox1.Text = string.Empty;

            var parenthandle = NativeMethods.GetParent(handle);

            var syslistview32handle = parenthandle.GetSysListView32Handle();
            if (syslistview32handle != IntPtr.Zero)
            {
                var selectstext = syslistview32handle.GetSlectedItemsText();
                if (selectstext != null && selectstext.Count > 0)
                {
                    if (selectstext.Count > 1)
                    {
                        selectstext = selectstext.OrderBy<string, string>((Func<string, string>)((i) => i)).ToList();
                        if (fileConvert.Count > 0)
                        {
                            fileConvert = fileConvert.OrderBy<string, string>((Func<string, string>)((i) => i)).ToList();
                        }

                        int filecount = 0;
                        int count = selectstext.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var name = selectstext[i].Split(new string[] { "\0" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                            var path = Path.Combine(FolderParent, name);
                            if (!Directory.Exists(path))
                            {
                                var floderparentTemp = FolderParent;
                                string text = string.Empty;
                                if (!string.IsNullOrWhiteSpace(FolderParent))
                                {
                                    var filepath = Path.Combine(FolderParent, name);
                                    try
                                    {
                                        var f = (new FileInfo(filepath));
                                        if (f != null && string.IsNullOrWhiteSpace(f.Extension))
                                        {
                                            filepath = filepath + ".lnk";
                                        }
                                        if (f != null && !f.Exists)
                                        {
                                            if (fileConvert.Count > filecount)
                                            {
                                                if (!Directory.Exists(fileConvert[filecount]))
                                                {
                                                    filepath = fileConvert[filecount];
                                                }
                                            }
                                        }

                                        text = CheckFile(name, filepath, ref floderparentTemp);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    if (fileConvert.Count > filecount)
                                    {
                                        text = CheckFile(name, fileConvert[filecount], ref floderparentTemp);
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(text))
                                {
                                    names.Add("\"" + text + "\"");
                                    filecount++;
                                }
                            }
                            else
                            {
                                var showpath = Path.Combine(FolderParent, name);
                                if (!FilesPath.Contains(showpath))
                                {
                                    FilesPath.Add(showpath);
                                    names.Add("\"" + name + "\"");
                                }
                            }
                        }
                        var files = string.Join(" ", names.ToArray());
                        if (names.Count == 1)
                        {
                            files = files.Replace("\"", "");
                        }
                        this.textBox1.Text = files;
                    }
                    else
                    {
                        if (fileConvert.Count == 1)
                        {
                            this.textBox1.Text = CheckFile(selectstext[0], fileConvert[0], ref FolderParent);
                        }
                    }
                }
            }
        }

        /// <summary> 此处因为 Windows 中文版的奇葩处理而生
        ///   // 我的文档中 显示中文，实际返回应该是英文
        ///   // 默认示例中，显示中文，真实文件同样是英文
        ///   // 选择文件窗口，默认选择桌面，但是会自动选择到计算机
        ///   // 当前用户的桌面可以显示所有用户桌面的lnk，但是真实地址是所有窗口
        /// </summary>
        private string CheckFile(string selettext, string converttext, ref string floderparent)
        {
            #region 单选处理

            var filename = selettext.Split(new string[] { "\0" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            bool islnk = false;
            var path = converttext;
            var fileinfo = new FileInfo(path);
            if (fileinfo.Extension.ToLower() == ".lnk")
            {
                if (!fileinfo.Exists)
                {
                    var allpublic = WindowHelper.GetAllUsersDesktopFolderPath();
                    var realfilepath = Path.Combine(allpublic, filename + ".lnk");
                    fileinfo = new FileInfo(realfilepath);
                }
                if (fileinfo.Exists)
                {
                    islnk = true;
                    path = fileinfo.FullName.GetLnkRealtivePath();
                }
            }

            if (string.IsNullOrWhiteSpace(floderparent))
            {
                if (path.IsFileOrDirectory())
                {
                    var directory = (new FileInfo(path)).Directory;
                    if (directory == null)
                    {
                        directory = new DirectoryInfo(path);
                    }
                    if (directory != null && directory.Exists)
                    {
                        floderparent = directory.FullName;
                        FilesPath.Add(Path.Combine(floderparent, path));
                        return filename;
                    }
                }
            }
            else
            {
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (floderparent.Equals(desktop))
                {
                    if (islnk)
                    {
                        var lnkinfo = new FileInfo(path);
                        if (lnkinfo != null && lnkinfo.Exists)
                        {
                            FilesPath.Add(path);
                            return lnkinfo.Name;
                        }
                    }
                    else
                    {
                        var pathsingle = System.IO.Path.Combine(floderparent, filename);
                        if (pathsingle.IsFileOrDirectory() && path.IsFileOrDirectory())
                        {
                            FilesPath.Add(Path.Combine(floderparent, path));
                            return filename;
                        }
                    }
                }
                else
                {
                    if (path.IsFileOrDirectory())
                    {
                        FilesPath.Add(Path.Combine(floderparent, path));
                        return filename;
                    }
                }
            }
            return string.Empty;

            #endregion 单选处理
        }

        public override void OnSelectedNameChanged()
        {
            if (FilesPath.Count > 0)
            {
                List<string> filesPath = new List<string>();
                FilesPath.ForEach(i => filesPath.Add(i.Replace("\"", "")));
                FilesPath = filesPath;
            }

            base.OnSelectedNameChanged();
        }
    }
}