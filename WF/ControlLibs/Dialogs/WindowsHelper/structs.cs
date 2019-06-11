using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    #region WINDOWINFO

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public UInt32 cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public UInt32 dwStyle;
        public UInt32 dwExStyle;
        public UInt32 dwWindowStatus;
        public UInt32 cxWindowBorders;
        public UInt32 cyWindowBorders;
        public UInt16 atomWindowType;
        public UInt16 wCreatorVersion;
    }

    #endregion WINDOWINFO

    #region POINT

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        #region Constructors

        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public POINT(Point point)
        {
            x = point.X;
            y = point.Y;
        }

        #endregion Constructors
    }

    #endregion POINT

    #region RECT

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public uint left;
        public uint top;
        public uint right;
        public uint bottom;

        #region Properties

        public POINT Location
        {
            get { return new POINT((int)left, (int)top); }
            set
            {
                right -= (left - (uint)value.x);
                bottom -= (bottom - (uint)value.y);
                left = (uint)value.x;
                top = (uint)value.y;
            }
        }

        public uint Width
        {
            get { return right - left; }
            set { right = left + value; }
        }

        public uint Height
        {
            get { return bottom - top; }
            set { bottom = top + value; }
        }

        #endregion Properties

        #region Overrides

        public override string ToString()
        {
            return left + ":" + top + ":" + right + ":" + bottom;
        }

        #endregion Overrides
    }

    #endregion RECT

    #region WINDOWPOS

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;

        #region Overrides

        public override string ToString()
        {
            return x + ":" + y + ":" + cx + ":" + cy + ":" + ((SWP_Flags)flags).ToString();
        }

        #endregion Overrides
    }

    #endregion WINDOWPOS

    #region NCCALCSIZE_PARAMS

    public struct NCCALCSIZE_PARAMS
    {
        public RECT rgrc1;
        public RECT rgrc2;
        public RECT rgrc3;
        public IntPtr lppos;
    }

    #endregion NCCALCSIZE_PARAMS

    #region

    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public uint idFrom;
        public uint code;
    }

    #endregion

    #region OFNOTIFY

    [StructLayout(LayoutKind.Sequential)]
    public struct OFNOTIFY
    {
        public NMHDR hdr;
        public IntPtr OPENFILENAME;
        public IntPtr fileNameShareViolation;
    }

    #endregion
}