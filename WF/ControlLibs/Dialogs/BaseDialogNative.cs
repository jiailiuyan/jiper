using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public class BaseDialogNative : NativeWindow, IDisposable
    {
        public delegate void FileNameChangedHandler(BaseDialogNative sender, string filePath);

        public delegate void ComfirmChangedHandler(BaseDialogNative sender);

        public delegate void FilesChangedHandler(BaseDialogNative sender, IntPtr handle);

        //public event ComfirmChangedHandler ComfirmChanged = delegate { };

        public event FileNameChangedHandler FileNameChanged;

        public event FileNameChangedHandler FolderNameChanged;

        public event FilesChangedHandler FilesSelected;

        private IntPtr mhandle;

        public BaseDialogNative(IntPtr handle)
        {
            mhandle = handle;
            AssignHandle(handle);
        }

        public void Dispose()
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)Msg.WM_NOTIFY:
                    {
                        OFNOTIFY ofNotify = (OFNOTIFY)Marshal.PtrToStructure(m.LParam, typeof(OFNOTIFY));
                        if (ofNotify.hdr.code == (uint)DialogChangeStatus.CDN_SELCHANGE)
                        {
                            StringBuilder filePath = new StringBuilder(102400);
                            NativeMethods.SendMessage(NativeMethods.GetParent(mhandle), (int)DialogChangeProperties.CDM_GETFILEPATH, (int)102400, filePath);

                            if (FileNameChanged != null)
                            {
                                FileNameChanged(this, filePath.ToString());
                            }
                        }
                        else if (ofNotify.hdr.code == (uint)DialogChangeStatus.CDN_FOLDERCHANGE)
                        {
                            StringBuilder folderPath = new StringBuilder(256);
                            NativeMethods.SendMessage(NativeMethods.GetParent(mhandle), (int)DialogChangeProperties.CDM_GETFOLDERPATH, (int)256, folderPath);

                            if (FolderNameChanged != null)
                            {
                                FolderNameChanged(this, folderPath.ToString());
                            }
                        }

                        if (FilesSelected != null)
                        {
                            FilesSelected(this, mhandle);
                        }

                        break;
                    }
            }
            base.WndProc(ref m);
        }
    }
}