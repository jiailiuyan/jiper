using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Windows;

public sealed class ThumbnailSafeHandle : SafeHandle
{
    [DllImport("dwmapi.dll", SetLastError = true)]
    static extern int DwmQueryThumbnailSourceSize(ThumbnailSafeHandle hThumbnail, ref PSIZE pSize);
    [DllImport("dwmapi.dll", SetLastError = true)]
    static extern int DwmRegisterThumbnail(IntPtr hwndDestination, IntPtr hwndSource, out ThumbnailSafeHandle hThumbnailId);
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("dwmapi.dll", SetLastError = true)]
    static extern int DwmUnregisterThumbnail(IntPtr hThumbmailId);
    [DllImport("dwmapi.dll", SetLastError = true)]
    static extern int DwmUpdateThumbnailProperties(ThumbnailSafeHandle hThumbmailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties);
    [DllImport("user32.dll")]
    static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    struct PSIZE
    {
        public int cx;
        public int cy;
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        internal RECT(int l, int t, int r, int b)
        {
            this.left = l;
            this.top = t;
            this.right = r;
            this.bottom = b;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DWM_THUMBNAIL_PROPERTIES
    {
        public int dwFlags;
        public RECT rcDestination;
        public RECT rcSource;
        public byte opacity;
        public bool fVisible;
        public bool fSourceClientAreaOnly;
    }

    private ThumbnailSafeHandle()
        : base(IntPtr.Zero, true)
    {
    }

    //在目标窗口(HWND = hwndDestination)内显示源窗口(HWND = hwndSource)的实时缩略图
    public static ThumbnailSafeHandle Register(IntPtr hwndDestination, IntPtr hwndSource)
    {
        ThumbnailSafeHandle handle;
        DwmRegisterThumbnail(hwndDestination, hwndSource, out handle);
        Size size = handle.Size;
        DWM_THUMBNAIL_PROPERTIES m_ThumbnailProperties = new DWM_THUMBNAIL_PROPERTIES();
        m_ThumbnailProperties.dwFlags = 29;
        m_ThumbnailProperties.opacity = 128; //透明度
        m_ThumbnailProperties.fVisible = true;
        m_ThumbnailProperties.fSourceClientAreaOnly = true; //只显示客户区
        m_ThumbnailProperties.rcDestination = new RECT(0, 0, (int)size.Width, (int)size.Height); //显示在目标窗口的哪个位置
        DwmUpdateThumbnailProperties(handle, ref m_ThumbnailProperties);

        return handle;
    }

    public Size Size
    {
        get
        {
            PSIZE size = new PSIZE();
            DwmQueryThumbnailSourceSize(this, ref size);
            return new Size(size.cx, size.cy);
        }
    }

    protected override bool ReleaseHandle()
    {
        if (this.IsInvalid)
        {
            return true;
        }
        int num = DwmUnregisterThumbnail(base.handle);
        base.SetHandle(IntPtr.Zero);
        return (num == 0);
    }

    public override bool IsInvalid
    {
        get
        {
            return (IntPtr.Zero == base.handle);
        }
    }
}
