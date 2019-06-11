using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public partial class OpenFileDialogEx : UserControl
    {
        private string lable_OpenFloder = "文件夹:";

        /// <summary> 文件夹: </summary>
        public virtual string Lable_OpenFloder { get { return lable_OpenFloder; } }

        private string lable_SelectTexg = "选择文件夹";

        /// <summary> 选择文件夹 </summary>
        public virtual string Lable_SelectTexg { get { return lable_SelectTexg; } }

        private string lable_CancelText = "取消";

        /// <summary> 取消 </summary>
        public virtual string Lable_CancelText { get { return lable_CancelText; } }

        public delegate void FileNameChangedHandler(OpenFileDialogEx sender, string filePath);

        public virtual event EventHandler SelectedNameChanged;

        public event FileNameChangedHandler FileNameChanged;

        public event FileNameChangedHandler FolderNameChanged;

        public event EventHandler ClosingDialog;

        protected delegate void FilesChangedHandler(OpenFileDialogEx sender, IntPtr handle);

        protected event FilesChangedHandler FilesSelected;

        private SetWindowPosFlags UFLAGSHIDE = SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_HIDEWINDOW;

        private FolderViewMode mDefaultViewMode = FolderViewMode.Default;

        public OpenFileDialogEx(string dir = "")
        {
            InitializeComponent();

            this.DefaultViewMode = FolderViewMode.Default;
            this.OpenDialog.AddExtension = false;
            this.OpenDialog.InitialDirectory = dir;
            this.OpenDialog.CheckFileExists = false;
            this.OpenDialog.ValidateNames = true;
            this.OpenDialog.ShowHelp = true;

            this.SizeChanged += OpenFileDialogEx_SizeChanged;

            this.OpenDialog.HelpRequest += OpenDialog_HelpRequest;

            this.Disposed += OpenFileDialogEx_Disposed;

            this.OpenDialog.DereferenceLinks = true;
        }

        private void OpenFileDialogEx_Disposed(object sender, EventArgs e)
        {
            //由于没有指定所属容器，会导致选择后主窗口最小化，因此在此 Show 一次主窗口
            WindowHelper.ShowCurrentWindowHandle();
        }

        private void OpenDialog_HelpRequest(object sender, EventArgs e)
        {
            this.Close();

            OnSelectedNameChanged();
        }

        private void OpenFileDialogEx_SizeChanged(object sender, EventArgs e)
        {
            this.textBox1.Width = this.Width + 1;
        }

        public OpenFileDialog OpenDialog
        {
            get { return dlgOpen; }
        }

        [DefaultValue(FolderViewMode.Default)]
        public FolderViewMode DefaultViewMode
        {
            get { return mDefaultViewMode; }
            set { mDefaultViewMode = value; }
        }

        public virtual void OnFileNameChanged(string fileName)
        {
            if (FileNameChanged != null)
            {
                FileNameChanged(this, fileName);
            }
        }

        public virtual void OnFolderNameChanged(string folderName)
        {
            if (FolderNameChanged != null)
            {
                FolderNameChanged(this, folderName);
            }
        }

        public virtual void OnFilesSelectedHandle(IntPtr handle)
        {
            if (FilesSelected != null)
            {
                FilesSelected(this, handle);
            }
        }

        public virtual void OnClosingDialog()
        {
            if (ClosingDialog != null)
            {
                ClosingDialog(this, new EventArgs());
            }
            //OnSelectedNameChanged();
            // WindowHelper.ShowCurrentWindowHandle();
        }

        public virtual void OnSelectedNameChanged()
        {
            if (this.SelectedNameChanged != null)
            {
                this.SelectedNameChanged(this, EventArgs.Empty);
            }

            ////由于没有指定所属容器，会导致选择后主窗口最小化，因此在此 Show 一次主窗口
            //WindowHelper.ShowCurrentWindowHandle();
        }

        public virtual void SetShowText(string directorypath)
        {
            var directoryInfo = new DirectoryInfo(directorypath);
            if (directoryInfo.Exists)
            {
                this.OpenDialog.InitialDirectory = directorypath;
            }
        }

        public void ShowDialog()
        {
            ShowDialog(null);
        }

        private DummyForm form;

        public void ShowDialog(IWin32Window owner)
        {
            form = new DummyForm(this, Lable_OpenFloder, Lable_SelectTexg, Lable_CancelText);

            form.Show(owner);
            NativeMethods.SetWindowPos(form.Handle, IntPtr.Zero, 0, 0, 0, 0, UFLAGSHIDE);

            // 此处应该设置弹出窗口的句柄，不能设置主窗口句柄，否则会导致弹出窗口会覆盖在 Dialog 之上，不设置窗口也可
            //设置父窗体为主窗口 WindowHelper.GetCurrentWindowHandle()
            //NativeMethods.SetParent(form.Handle, (IntPtr)21498960);

            form.WatchForActivate = true;
            try
            {
                dlgOpen.ShowDialog(form);
            }
            catch { }
            form.Dispose();
            form.Close();

            this.Dispose();
        }

        public void Close()
        {
            if (form != null)
            {
                form.Dispose();
                form.Close();
                this.Dispose();
            }
        }
    }
}