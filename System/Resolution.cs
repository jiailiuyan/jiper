using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ji.SystemHelper
{
    public class ScreenResolution
    {
        private static Resolution.DEVMODE realDevmode;

        public static Resolution.DEVMODE RealDevmode
        {
            get
            {
                if (realDevmode.dmLogPixels == 0)
                {
                    realDevmode = new Resolution().getResolution();
                }
                return realDevmode;
            }
        }

        private static double? realScale = null;

        public static double RealScale
        {
            get
            {
                if (realScale == null)
                {
                    realScale = RealDevmode.dmLogPixels / 96d;
                }
                return realScale.Value;
            }
        }

        private static int? realScreenWidth = null;

        public static int RealScreenWidth
        {
            get
            {
                if (realScreenWidth == null)
                {
                    realScreenWidth = RealDevmode.dmPelsWidth;
                }
                return realScreenWidth.Value;
            }
        }

        private static int? realScreenHeight = null;

        public static int RealScreenHeight
        {
            get
            {
                if (realScreenHeight == null)
                {
                    realScreenHeight = RealDevmode.dmPelsHeight;
                }
                return realScreenHeight.Value;
            }
        }
    }

    public class Resolution
    {
        public enum DMDO
        {
            DEFAULT = 0,
            D90 = 1,
            D180 = 2,
            D270 = 3
        }

        public const int CDS_UPDATEREGISTRY = 0x01;
        public const int CDS_TEST = 0x02;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DEVMODE
        {
            public const int DM_DISPLAYFREQUENCY = 0x400000;
            public const int DM_PELSWIDTH = 0x80000;
            public const int DM_PELSHEIGHT = 0x100000;
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;

            public int dmPositionX;
            public int dmPositionY;
            public DMDO dmDisplayOrientation;
            public int dmDisplayFixedOutput;

            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;

            public short dmLogPixels;
            public int dmBitsPerPel;//颜色质量,如32位,24位
            public int dmPelsWidth;//分辨率宽度，如1024
            public int dmPelsHeight;//分辨率高度,如768
            public int dmDisplayFlags;
            public int dmDisplayFrequency;//刷新频率,如75Hz
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int ChangeDisplaySettings([In]   ref DEVMODE lpDevMode, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnumDisplaySettings(string lpszDeviceName, Int32 iModeNum, ref DEVMODE lpDevMode);

        //设置分辨率,width宽,height高,displayFrequency刷新频率,设置成功返回true,否则false
        //调用方式： setResolution(1024, 768, 75);
        public bool setResolution(int width, int height, int displayFrequency)
        {
            bool ret = false;
            long RetVal = 0;
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;
            dm.dmDisplayFrequency = displayFrequency;
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;
            RetVal = ChangeDisplaySettings(ref dm, CDS_TEST);
            if (RetVal == 0)
            {
                RetVal = ChangeDisplaySettings(ref dm, 0);
                ret = true;
            }
            return ret;
        }

        //设置分辨率,width宽,height高,displayFrequency刷新频率,bitsPerPel颜色位数，设置成功返回true,否则false
        //调用方式： setResolution(1024, 768, 75, 32);
        public bool setResolution(int width, int height, int displayFrequency, int bitsPerPel)
        {
            bool ret = false;
            long RetVal = 0;
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;
            dm.dmDisplayFrequency = displayFrequency;
            dm.dmBitsPerPel = bitsPerPel;
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;
            RetVal = ChangeDisplaySettings(ref dm, CDS_TEST);
            if (RetVal == 0)
            {
                RetVal = ChangeDisplaySettings(ref dm, 0);
                ret = true;
            }
            return ret;
        }

        //返回当前图形模式信息
        public DEVMODE getResolution()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            bool mybool;
            mybool = EnumDisplaySettings(null, -1, ref dm);
            return dm;
        }

        //返回所有支持图形模式
        public List<DEVMODE> getAllResolution()
        {
            List<DEVMODE> allMode = new List<DEVMODE>();
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            int index = 0;
            while (EnumDisplaySettings(null, index, ref dm))
            {
                allMode.Add(dm);
                index++;
            }
            return allMode;
        }
    }
}