using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    /// <summary>
    /// 创建选择文件夹
    /// </summary>
    public class OpenDialogNative : NativeWindow, IDisposable
    {
        #region 多语言

        /// <summary> 文件夹: </summary>
        public string Lable_OpenFloder = "文件夹:";

        /// <summary> 选择文件夹 </summary>
        public string Lable_SelectTexg = "选择文件夹";

        /// <summary> 取消 </summary>
        public string Lable_CancelText = "取消";

        #endregion 多语言

        #region Constants Declaration

        private SetWindowPosFlags UFLAGSSIZE =
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOOWNERZORDER |
            SetWindowPosFlags.SWP_NOMOVE;

        private SetWindowPosFlags UFLAGSHIDE =
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOOWNERZORDER |
            SetWindowPosFlags.SWP_NOMOVE |
            SetWindowPosFlags.SWP_NOSIZE |
            SetWindowPosFlags.SWP_HIDEWINDOW;

        private SetWindowPosFlags UFLAGSZORDER =
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOMOVE |
            SetWindowPosFlags.SWP_NOSIZE;

        #endregion Constants Declaration

        public BaseDialogNative BaseDialogNative { get; private set; }

        #region Variables Declaration

        private Size mOriginalSize;
        private IntPtr mOpenDialogHandle;
        private IntPtr mListViewPtr;
        private WINDOWINFO mListViewInfo;

        private IntPtr mComboFolders;
        private WINDOWINFO mComboFoldersInfo;
        private IntPtr mGroupButtons;
        private WINDOWINFO mGroupButtonsInfo;
        private IntPtr mComboFileName;
        private WINDOWINFO mComboFileNameInfo;
        private IntPtr mComboExtensions;
        private WINDOWINFO mComboExtensionsInfo;
        private IntPtr mOpenButton;
        private WINDOWINFO mOpenButtonInfo;
        private IntPtr mCancelButton;
        private WINDOWINFO mCancelButtonInfo;
        private IntPtr mHelpButton;
        private WINDOWINFO mHelpButtonInfo;
        private OpenFileDialogEx mSourceControl;
        private IntPtr mToolBarFolders;
        private WINDOWINFO mToolBarFoldersInfo;
        private IntPtr mLabelFileName;
        private WINDOWINFO mLabelFileNameInfo;
        private IntPtr mLabelFileType;
        private WINDOWINFO mLabelFileTypeInfo;
        private IntPtr mChkReadOnly;
        private WINDOWINFO mChkReadOnlyInfo;
        private bool mIsClosing = false;
        private bool mInitializated = false;
        private RECT mOpenDialogWindowRect = new RECT();
        private RECT mOpenDialogClientRect = new RECT();

        #endregion Variables Declaration

        private IntPtr ParentHandle;

        public OpenDialogNative(IntPtr handle, OpenFileDialogEx sourceControl, IntPtr parentHandle,
            string lable_OpenFloder, string lable_SelectTexg, string lable_CancelText)
        {
            this.Lable_OpenFloder = lable_OpenFloder;
            this.Lable_SelectTexg = lable_SelectTexg;
            this.Lable_CancelText = lable_CancelText;

            ParentHandle = parentHandle;

            mOpenDialogHandle = handle;
            mSourceControl = sourceControl;
            AssignHandle(mOpenDialogHandle);
        }

        private void BaseDialogNative_FileNameChanged(BaseDialogNative sender, string filePath)
        {
            if (mSourceControl != null)
            {
                mSourceControl.OnFileNameChanged(filePath);
            }
        }

        private void BaseDialogNative_FolderNameChanged(BaseDialogNative sender, string folderName)
        {
            if (mSourceControl != null)
            {
                mSourceControl.OnFolderNameChanged(folderName);
            }
        }

        private void BaseDialogNative_FilesSelectedChanged(BaseDialogNative sender, IntPtr handle)
        {
            if (mSourceControl != null)
            {
                mSourceControl.OnFilesSelectedHandle(handle);
            }
        }

        public bool IsClosing
        {
            get { return mIsClosing; }
            set { mIsClosing = value; }
        }

        public void Dispose()
        {
            ReleaseHandle();
            if (BaseDialogNative != null)
            {
                BaseDialogNative.FileNameChanged -= BaseDialogNative_FileNameChanged;
                BaseDialogNative.FolderNameChanged -= BaseDialogNative_FolderNameChanged;
                BaseDialogNative.FilesSelected -= BaseDialogNative_FilesSelectedChanged;
                BaseDialogNative.Dispose();
            }
        }

        #region Private Methods

        private void PopulateWindowsHandlers()
        {
            NativeMethods.EnumChildWindows(mOpenDialogHandle, new NativeMethods.EnumWindowsCallBack(OpenFileDialogEnumWindowCallBack), 1);
        }

        private bool OpenFileDialogEnumWindowCallBack(IntPtr hwnd, int lParam)
        {
            StringBuilder className = new StringBuilder(256);
            NativeMethods.GetClassName(hwnd, className, className.Capacity);
            int controlID = NativeMethods.GetDlgCtrlID(hwnd);
            WINDOWINFO windowInfo;
            NativeMethods.GetWindowInfo(hwnd, out windowInfo);

            if (className.ToString().StartsWith("#32770"))
            {
                BaseDialogNative = new BaseDialogNative(hwnd);
                BaseDialogNative.FileNameChanged += BaseDialogNative_FileNameChanged;
                BaseDialogNative.FolderNameChanged += BaseDialogNative_FolderNameChanged;
                BaseDialogNative.FilesSelected += BaseDialogNative_FilesSelectedChanged;
                return true;
            }

            switch ((ControlsID)controlID)
            {
                #region 隐藏的窗口

                case ControlsID.CheckBoxReadOnly:
                    {
                        mChkReadOnly = hwnd;
                        mChkReadOnlyInfo = windowInfo;
                        NativeMethods.ShowWindow(hwnd, NativeMethods.SW_HIDE);
                        break;
                    }

                case ControlsID.LabelFileType:
                    {
                        mLabelFileType = hwnd;
                        mLabelFileTypeInfo = windowInfo;
                        NativeMethods.ShowWindow(hwnd, NativeMethods.SW_HIDE);
                        break;
                    }

                case ControlsID.ComboFileType:
                    {
                        mComboExtensions = hwnd;
                        mComboExtensionsInfo = windowInfo;
                        NativeMethods.ShowWindow(hwnd, NativeMethods.SW_HIDE);
                        break;
                    }

                case ControlsID.ComboFileName:
                    {
                        if (className.ToString().ToLower() == "comboboxex32")
                        {
                            mComboFileName = hwnd;
                            mComboFileNameInfo = windowInfo;
                            NativeMethods.ShowWindow(hwnd, NativeMethods.SW_HIDE);
                        }
                        break;
                    }

                case ControlsID.ButtonOpen:
                    {
                        mOpenButton = hwnd;
                        mOpenButtonInfo = windowInfo;
                        break;
                    }

                #endregion 隐藏的窗口

                case ControlsID.ButtonHelp:
                    {
                        mHelpButton = hwnd;
                        mHelpButtonInfo = windowInfo;

                        NativeMethods.SetWindowText(hwnd, Lable_SelectTexg);
                        break;
                    }

                case ControlsID.DefaultView:
                    mListViewPtr = hwnd;
                    NativeMethods.GetWindowInfo(hwnd, out mListViewInfo);
                    if (mSourceControl.DefaultViewMode != FolderViewMode.Default)
                    {
                        NativeMethods.SendMessage(mListViewPtr, (int)Msg.WM_COMMAND, (int)mSourceControl.DefaultViewMode, 0);
                    }

                    break;

                case ControlsID.ComboFolder:
                    mComboFolders = hwnd;
                    mComboFoldersInfo = windowInfo;
                    break;

                case ControlsID.GroupFolder:
                    mGroupButtons = hwnd;
                    mGroupButtonsInfo = windowInfo;
                    break;

                case ControlsID.LeftToolBar:
                    mToolBarFolders = hwnd;
                    mToolBarFoldersInfo = windowInfo;
                    break;

                case ControlsID.ButtonCancel:
                    {
                        mCancelButton = hwnd;
                        mCancelButtonInfo = windowInfo;
                        NativeMethods.SetWindowText(hwnd, Lable_CancelText);
                        break;
                    }

                case ControlsID.LabelFileName:
                    {
                        mLabelFileName = hwnd;
                        mLabelFileNameInfo = windowInfo;
                        NativeMethods.SetWindowText(hwnd, Lable_OpenFloder);
                        break;
                    }
            }

            return true;
        }

        private void InitControls()
        {
            mInitializated = true;

            NativeMethods.GetClientRect(mOpenDialogHandle, ref mOpenDialogClientRect);
            NativeMethods.GetWindowRect(mOpenDialogHandle, ref mOpenDialogWindowRect);
            PopulateWindowsHandlers();

            SetCustomPosition(true);

            NativeMethods.SetParent(mSourceControl.Handle, mOpenDialogHandle);
            NativeMethods.SetWindowPos(mSourceControl.Handle, (IntPtr)ZOrderPos.HWND_BOTTOM, 0, 0, 0, 0, UFLAGSZORDER);
        }

        #endregion Private Methods

        private void SetCustomPosition(bool islocation = false)
        {
            var rectmComboFileName = new RECT();
            NativeMethods.GetWindowRect(mComboFileName, ref rectmComboFileName);
            var rectmOpenDialogHandle = new RECT();
            NativeMethods.GetWindowRect(mOpenDialogHandle, ref rectmOpenDialogHandle);

            //设置新的textbox挡住假的
            var x = rectmComboFileName.left - rectmOpenDialogHandle.left - 8;
            var y = rectmComboFileName.top - rectmOpenDialogHandle.top - 30;
            if (islocation)
            {
                //设置文本位置
                mSourceControl.Location = new Point((int)x, (int)y);

                //设置按钮位置
                var rectmOpenButton = new RECT();
                NativeMethods.GetWindowRect(mOpenButton, ref rectmOpenButton);

                var hbx = rectmOpenButton.left - rectmOpenDialogHandle.left - 8;
                var hby = y;

                NativeMethods.SetWindowPos(mHelpButton,
                    (IntPtr)NativeMethods.HWND_TOP,
                    (int)hbx, (int)hby,
                    (int)rectmOpenButton.Width, (int)rectmOpenButton.Height,
                    SetWindowPosFlags.SWP_SHOWWINDOW);

                // 只有在显示出界面之后才能隐藏 Open 按钮，否则会导致默认的路径下不会显示任何文件
                NativeMethods.ShowWindow(mOpenButton, NativeMethods.SW_HIDE);
            }
            else
            {
                mSourceControl.Width = (int)rectmComboFileName.Width;
            }
        }

        protected override void WndProc(ref Message m)
        {
            bool isresize = false;
            switch (m.Msg)
            {
                case (int)Msg.WM_SHOWWINDOW:
                    {
                        mInitializated = true;
                        InitControls();
                        break;
                    }

                case (int)Msg.WM_SIZE:
                    {
                        isresize = true;
                        break;
                    }
                case (int)Msg.WM_WINDOWPOSCHANGING:
                    {
                        if (!mIsClosing)
                        {
                            if (!mInitializated)
                            {
                                //减去因为隐藏了帮助按钮而造成的距下边框过长
                                WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
                                pos.cy = 400;
                                Marshal.StructureToPtr(pos, m.LParam, true);
                            }

                            if (!mComboFileName.Equals(IntPtr.Zero))
                            {
                                SetCustomPosition(false);
                            }
                        }
                        break;
                    }

                case (int)Msg.WM_IME_NOTIFY:
                    if (m.WParam == (IntPtr)ImeNotify.IMN_CLOSESTATUSWINDOW && !mIsClosing)
                    {
                        mIsClosing = true;
                        mSourceControl.OnClosingDialog();

                        NativeMethods.SetWindowPos(mOpenDialogHandle, IntPtr.Zero, 0, 0, 0, 0, UFLAGSHIDE);
                        NativeMethods.GetWindowRect(mOpenDialogHandle, ref mOpenDialogWindowRect);
                        NativeMethods.SetWindowPos(mOpenDialogHandle, IntPtr.Zero,
                            (int)(mOpenDialogWindowRect.left),
                            (int)(mOpenDialogWindowRect.top),
                            (int)(mOriginalSize.Width),
                            (int)(mOriginalSize.Height),
                            UFLAGSSIZE);
                    }
                    break;

                case (int)Msg.WM_DESTROY:
                    {
                        if (!mIsClosing)
                        {
                            mIsClosing = true;

                            NativeMethods.SetWindowPos(mOpenDialogHandle, IntPtr.Zero, 0, 0, 0, 0, UFLAGSHIDE);
                            NativeMethods.GetWindowRect(mOpenDialogHandle, ref mOpenDialogWindowRect);
                            NativeMethods.SetWindowPos(mOpenDialogHandle, IntPtr.Zero,
                                (int)(mOpenDialogWindowRect.left),
                                (int)(mOpenDialogWindowRect.top),
                                (int)(mOriginalSize.Width),
                                (int)(mOriginalSize.Height),
                                UFLAGSSIZE);
                        }
                        break;
                    }
            }

            base.WndProc(ref m);

            //更改窗口大小后同步一次控件位置，主要用于最大化最小化
            if (isresize)
            {
                if (!mComboFileName.Equals(IntPtr.Zero))
                {
                    SetCustomPosition(false);
                }
            }
        }
    }
}