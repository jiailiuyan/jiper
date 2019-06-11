using System;
using System.Management;
using System.Runtime.InteropServices;

namespace Ji.SystemHelper
{
    public class SystemInfoHelper
    {
        public static void ShutDownCancle()
        {
            var cpuID = HardwareInfo.GetMacAddress();
        }

        public static string GetMAC()
        {
            var rd = "";
            try
            {
                rd = HardwareInfo.GetMacAddress();
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }
            return rd;
        }

        public static string GetDiskSerialNumber()
        {
            var rd = "";
            try
            {
                rd = HardwareInfo.GetDiskVolume();
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }
            return rd;
        }

        /// <summary>
        /// 获取系统内存大小
        /// </summary>
        /// <returns>内存大小（单位M）</returns>
        public static int GetPhisicalMemory()
        {
            try
            {
                var memInfo = new HardwareInfo.MEMORYSTATUSEX();
                memInfo.dwLength = (uint)Marshal.SizeOf(typeof(HardwareInfo.MEMORYSTATUSEX));
                HardwareInfo.GlobalMemoryStatusEx(ref memInfo);
                var tm = memInfo.ullTotalPhys / 1024 / 1024;
                return (int)tm;
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }

            try
            {
                var searcher = new ManagementObjectSearcher();
                searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[] { "Capacity" });
                var collection = searcher.Get();
                var em = collection.GetEnumerator();

                long capacity = 0;
                while (em.MoveNext())
                {
                    var baseObj = em.Current;
                    if (baseObj.Properties["Capacity"].Value != null)
                    {
                        try
                        {
                            capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
                return (int)(capacity / 1024 / 1024);
            }
            catch (Exception ex)
            {
                LogX.Error(ex);
            }

            return 0;
        }

        /// <summary> 
        /// HardwareInfo 的摘要说明。 
        /// </summary> 
        internal class HardwareInfo
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct MEMORYSTATUSEX
            {
                public uint dwLength;
                public uint dwMemoryLoad;
                public ulong ullTotalPhys;
                public ulong ullAvailPhys;      //可用物理内存
                public ulong ullTotalPageFile;
                public ulong ullAvailPageFile;
                public ulong ullTotalVirtual;
                public ulong ullAvailVirtual;
                public ulong ullAvailExtendedVirtual;
            }

            [DllImport("kernel32.dll")]
            public static extern void GlobalMemoryStatusEx(ref MEMORYSTATUSEX stat);




            public enum NCBCONST
            {
                NCBNAMSZ = 16,      /* absolute length of a net name         */
                MAX_LANA = 254,      /* lana's in range 0 to MAX_LANA inclusive   */
                NCBENUM = 0x37,      /* NCB ENUMERATE LANA NUMBERS            */
                NRC_GOODRET = 0x00,      /* good return                              */
                NCBRESET = 0x32,      /* NCB RESET                        */
                NCBASTAT = 0x33,      /* NCB ADAPTER STATUS                  */
                NUM_NAMEBUF = 30,      /* Number of NAME's BUFFER               */
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ADAPTER_STATUS
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
                public byte[] adapter_address;
                public byte rev_major;
                public byte reserved0;
                public byte adapter_type;
                public byte rev_minor;
                public ushort duration;
                public ushort frmr_recv;
                public ushort frmr_xmit;
                public ushort iframe_recv_err;
                public ushort xmit_aborts;
                public uint xmit_success;
                public uint recv_success;
                public ushort iframe_xmit_err;
                public ushort recv_buff_unavail;
                public ushort t1_timeouts;
                public ushort ti_timeouts;
                public uint reserved1;
                public ushort free_ncbs;
                public ushort max_cfg_ncbs;
                public ushort max_ncbs;
                public ushort xmit_buf_unavail;
                public ushort max_dgram_size;
                public ushort pending_sess;
                public ushort max_cfg_sess;
                public ushort max_sess;
                public ushort max_sess_pkt_size;
                public ushort name_count;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct NAME_BUFFER
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
                public byte[] name;
                public byte name_num;
                public byte name_flags;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct NCB
            {
                public byte ncb_command;
                public byte ncb_retcode;
                public byte ncb_lsn;
                public byte ncb_num;
                public IntPtr ncb_buffer;
                public ushort ncb_length;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
                public byte[] ncb_callname;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
                public byte[] ncb_name;
                public byte ncb_rto;
                public byte ncb_sto;
                public IntPtr ncb_post;
                public byte ncb_lana_num;
                public byte ncb_cmd_cplt;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
                public byte[] ncb_reserve;
                public IntPtr ncb_event;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct LANA_ENUM
            {
                public byte length;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.MAX_LANA)]
                public byte[] lana;
            }

            [StructLayout(LayoutKind.Auto)]
            public struct ASTAT
            {
                public ADAPTER_STATUS adapt;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NUM_NAMEBUF)]
                public NAME_BUFFER[] NameBuff;
            }
            public class Win32API
            {
                [DllImport("NETAPI32.DLL")]
                public static extern char Netbios(ref NCB ncb);
            }

            public static string GetMacAddress()
            {
                var addr = "";
                try
                {
                    int cb;
                    ASTAT adapter;
                    var Ncb = new NCB();
                    char uRetCode;
                    LANA_ENUM lenum;

                    Ncb.ncb_command = (byte)NCBCONST.NCBENUM;
                    cb = Marshal.SizeOf(typeof(LANA_ENUM));
                    Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                    Ncb.ncb_length = (ushort)cb;
                    uRetCode = Win32API.Netbios(ref Ncb);
                    lenum = (LANA_ENUM)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(LANA_ENUM));
                    Marshal.FreeHGlobal(Ncb.ncb_buffer);
                    if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                        return "";

                    for (var i = 0; i < lenum.length; i++)
                    {
                        Ncb.ncb_command = (byte)NCBCONST.NCBRESET;
                        Ncb.ncb_lana_num = lenum.lana[i];
                        uRetCode = Win32API.Netbios(ref Ncb);
                        if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                            return "";

                        Ncb.ncb_command = (byte)NCBCONST.NCBASTAT;
                        Ncb.ncb_lana_num = lenum.lana[i];
                        Ncb.ncb_callname[0] = (byte)'*';
                        cb = Marshal.SizeOf(typeof(ADAPTER_STATUS)) + Marshal.SizeOf(typeof(NAME_BUFFER)) * (int)NCBCONST.NUM_NAMEBUF;
                        Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                        Ncb.ncb_length = (ushort)cb;
                        uRetCode = Win32API.Netbios(ref Ncb);
                        adapter.adapt = (ADAPTER_STATUS)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(ADAPTER_STATUS));
                        Marshal.FreeHGlobal(Ncb.ncb_buffer);

                        if (uRetCode == (short)NCBCONST.NRC_GOODRET)
                        {
                            if (i > 0)
                                addr += ":";
                            addr = string.Format("{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}",
                             adapter.adapt.adapter_address[0],
                             adapter.adapt.adapter_address[1],
                             adapter.adapt.adapter_address[2],
                             adapter.adapt.adapter_address[3],
                             adapter.adapt.adapter_address[4],
                             adapter.adapt.adapter_address[5]);
                        }
                    }
                }
                catch
                { }
                return addr.Replace(' ', '0');
            }


            [DllImport("kernel32.dll")]
            private static extern int GetVolumeInformation(
            string lpRootPathName,
            string lpVolumeNameBuffer,
            int nVolumeNameSize,
            ref int lpVolumeSerialNumber,
            int lpMaximumComponentLength,
            int lpFileSystemFlags,
            string lpFileSystemNameBuffer,
            int nFileSystemNameSize
            );

            /// <summary>
            /// 获取硬盘序列号
            /// </summary>
            /// <param name="drvID">硬盘盘符[c|d|e|....]</param>
            /// <returns></returns>
            internal static string GetDiskVolume(string drvID = "C")
            {
                const int MAX_FILENAME_LEN = 256;
                var retVal = 0;
                var lpMaximumComponentLength = 0;
                var lpFileSystemFlags = 0;
                string lpVolumeNameBuffer = null;
                string lpFileSystemNameBuffer = null;
                var i = GetVolumeInformation(
                drvID + @":\",
                lpVolumeNameBuffer,
                MAX_FILENAME_LEN,
                ref retVal,
                lpMaximumComponentLength,
                lpFileSystemFlags,
                lpFileSystemNameBuffer,
                MAX_FILENAME_LEN
                );
                return retVal.ToString("x");
            }
        }

    }
}

