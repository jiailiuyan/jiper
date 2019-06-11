using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public class DummyForm : Form
    {
        /// <summary> 文件夹: </summary>
        public string Lable_OpenFloder = "文件夹:";

        /// <summary> 选择文件夹 </summary>
        public string Lable_SelectTexg = "选择";

        /// <summary> 取消 </summary>
        public string Lable_CancelText = "取消";

        #region Variables Declaration

        private OpenDialogNative mNativeDialog = null;
        private OpenFileDialogEx mFileDialogEx = null;
        private bool mWatchForActivate = false;
        private IntPtr mOpenDialogHandle = IntPtr.Zero;

        #endregion Variables Declaration

        #region Constructors

        public DummyForm(OpenFileDialogEx fileDialogEx, string lable_OpenFloder, string lable_SelectTexg, string lable_CancelText)
        {
            this.Lable_OpenFloder = lable_OpenFloder;
            this.Lable_SelectTexg = lable_SelectTexg;
            this.Lable_CancelText = lable_CancelText;

            mFileDialogEx = fileDialogEx;
            this.Text = "";
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(-32000, -32000);
            this.ShowInTaskbar = false;
        }

        #endregion Constructors

        #region Properties

        public bool WatchForActivate
        {
            get { return mWatchForActivate; }
            set { mWatchForActivate = value; }
        }

        #endregion Properties

        #region Overrides

        protected override void OnClosing(CancelEventArgs e)
        {
            if (mNativeDialog != null)
                mNativeDialog.Dispose();
            base.OnClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (mWatchForActivate && m.Msg == (int)Msg.WM_ACTIVATE)
            {
                mWatchForActivate = false;
                mOpenDialogHandle = m.LParam;
                mNativeDialog = new OpenDialogNative(m.LParam, mFileDialogEx, this.Handle, Lable_OpenFloder, Lable_SelectTexg, Lable_CancelText);
            }
            base.WndProc(ref m);
        }

        #endregion Overrides
    }
}