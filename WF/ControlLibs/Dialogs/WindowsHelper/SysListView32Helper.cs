using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    /// <summary> 使用时需要注意
    /// 1，目标系统位数必须和当前位数匹配
    /// </summary>
    public static class SysListView32Helper
    {
        #region 结构体定义

        private const int LVIF_TEXT = 0x0001;

        #region PROCESS_VM

        private enum PROCESS
        {
            /// <summary> 允许函数VirtualProtectEx使用此句柄修改进程的虚拟内存 </summary>
            PROCESS_VM_OPERATION = 0x0008,

            /// <summary> 允许函数访问权限 </summary>
            PROCESS_VM_READ = 0x0010,

            /// <summary> 允许函数写入权限 </summary>
            PROCESS_VM_WRITE = 0x0020
        }

        #endregion PROCESS_VM

        #region LVM_

        private enum SysListView32
        {
            LVM_FIRST = 0x1000,
            HDM_GETITEMCOUNT = 0x1200,
            LVM_GETHEADER = LVM_FIRST + 31,
            LVM_GETITEMSTATE = LVM_FIRST + 44,
            LVIS_SELECTED = 0x2,
            LVM_GETITEMCOUNT = LVM_FIRST + 4,
            LVM_GETITEMW = LVM_FIRST + 75
        }

        #endregion LVM_

        #region MEM

        private enum MEM
        {
            /// <summary> 为特定的页面区域分配内存中或磁盘的页面文件中的物理存储 </summary>
            MEM_COMMIT = 0x1000,

            MEM_RELEASE = 0x8000,

            /// <summary> 保留进程的虚拟地址空间,而不分配任何物理存储 </summary>
            MEM_RESERVE = 0x2000
        }

        #endregion MEM

        #region

        private enum PAGE
        {
            PAGE_READWRITE = 4
        }

        #endregion 结构体定义

        /// <summary>
        /// LVITEM结构体,是列表视图控件的一个重要的数据结构
        /// 占空间：4(int)x7=28个byte
        /// </summary>
        private struct LVITEM
        {
            /// <summary> 说明此结构中哪些成员是有效的 </summary>
            public int mask;

            /// <summary> 项目的索引值(可以视为行号)从0开始 </summary>
            public int iItem;

            /// <summary> 子项的索引值(可以视为列号)从0开始 </summary>
            public int iSubItem;

            /// <summary> 子项的状态 </summary>
            public int state;

            /// <summary> 状态有效的屏蔽位 </summary>
            public int stateMask;

            /// <summary> 主项或子项的名称 </summary>
            public IntPtr pszText;

            /// <summary> pszText所指向的缓冲区大小 </summary>
            public int cchTextMax;
        }

        #endregion

        public static bool IsRowSelected(this IntPtr handle, int rowindex)
        {
            return NativeMethods.SendMessage(handle, (int)SysListView32.LVM_GETITEMSTATE, rowindex, (int)SysListView32.LVIS_SELECTED) == 2;
        }

        public static List<int> GetSelectedRowsIndex(this IntPtr handle)
        {
            List<int> selectesindex = new List<int>();

            int rows = NativeMethods.SendMessage(handle, (int)SysListView32.LVM_GETITEMCOUNT, 0, 0);
            if (rows > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    if (handle.IsRowSelected(i))
                    {
                        selectesindex.Add(i);
                    }
                }
            }

            return selectesindex;
        }

        public static int GetItemCount(this IntPtr handle)
        {
            return NativeMethods.SendMessage(handle, (int)SysListView32.LVM_GETITEMCOUNT, 0, 0);
        }

        public static List<string> GetItemsText(this IntPtr handle, List<int> rowIndex, int column)
        {
            List<string> texts = new List<string>();
            if (rowIndex != null)
            {
                int processid = 0;
                NativeMethods.GetWindowThreadProcessId(handle, ref processid);
                if (processid != 0)
                {
                    int process = NativeMethods.OpenProcess((int)(PROCESS.PROCESS_VM_OPERATION | PROCESS.PROCESS_VM_READ | PROCESS.PROCESS_VM_WRITE), false, processid);
                    var pointer = NativeMethods.VirtualAllocEx(process, IntPtr.Zero, (uint)4096, (uint)(MEM.MEM_RESERVE | MEM.MEM_COMMIT), (uint)PAGE.PAGE_READWRITE);

                    int count = handle.GetItemCount();
                    if (count > 0 && rowIndex.Count <= count)
                    {
                        foreach (int itemindex in rowIndex)
                        {
                            //定义一个临时缓冲区
                            byte[] vBuffer = new byte[256];
                            LVITEM[] vItem = new LVITEM[1];
                            vItem[0].mask = LVIF_TEXT;
                            vItem[0].iItem = itemindex;
                            vItem[0].iSubItem = column;

                            vItem[0].state = 0;
                            vItem[0].stateMask = 0;

                            //所能存储的最大的文本为256字节
                            vItem[0].cchTextMax = vBuffer.Length;
                            vItem[0].pszText = (IntPtr)((int)pointer + Marshal.SizeOf(typeof(LVITEM)));
                            uint vNumberOfBytesRead = 0;

                            //把数据写到vItem中
                            //pointer为申请到的内存的首地址
                            //UnsafeAddrOfPinnedArrayElement:获取指定数组中指定索引处的元素的地址
                            NativeMethods.WriteProcessMemory(process, pointer, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

                            //发送LVM_GETITEMW消息给hwnd,将返回的结果写入pointer指向的内存空间
                            NativeMethods.SendMessage(handle, (int)SysListView32.LVM_GETITEMW, itemindex, pointer);
                            //从pointer指向的内存地址开始读取数据,写入缓冲区vBuffer中
                            NativeMethods.ReadProcessMemory(process, ((int)pointer + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0), vBuffer.Length, ref vNumberOfBytesRead);

                            texts.Add(Encoding.Unicode.GetString(vBuffer, 0, (int)vNumberOfBytesRead));
                        }
                    }
                }
            }
            return texts;
        }

        public static List<string> GetSlectedItemsText(this IntPtr handle, int column = 0)
        {
            List<string> texts = new List<string>();

            if (handle != IntPtr.Zero)
            {
                List<int> rowIndex = handle.GetSelectedRowsIndex();
                if (rowIndex.Count > 0)
                {
                    texts.AddRange(handle.GetItemsText(rowIndex, column));
                }
            }

            return texts;
        }

        public static IntPtr GetSysListView32Handle(this IntPtr mainWindowHandle)
        {
            var defaultviewhandle = NativeMethods.FindWindowEx(mainWindowHandle, IntPtr.Zero, "SHELLDLL_DefView", "");
            return NativeMethods.FindWindowEx((IntPtr)defaultviewhandle, (IntPtr)null, "SysListView32", null);
        }
    }
}