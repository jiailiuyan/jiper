using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public static class NativeMethods
    {
        public const int HWND_TOP = 0x0;
        public const int SW_SHOW = 5;
        public const int SW_HIDE = 0;

        ///<summary>
        ///��С������
        ///      hwnd:Integer��,����С�����Ǹ����ڵľ��
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int CloseWindow(IntPtr hwnd);

        ///<summary>
        ///�豸����ʹ��״̬
        ///      hwnd:Integer��,���ھ��
        ///      fEnable:Integer��,���������ڣ����ֹ
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int EnableWindow(IntPtr hwnd, int fEnable);

        ///<summary>
        ///��Ŀ¼�������ƴ��������ô����ı�
        ///      hwnd:Integer��,Ҫ�������ֵĴ��ڵľ��
        ///      lpString:String�ͣ�Ҫ�赽hwnd�����е�����
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hwnd, string lpString);

        ///<summary>
        ///��ʾ����
        ///      hwnd:Integer��,���ھ����Ҫ���������Ӧ����nCmdShowָ��������
        ///      nCmdShow:Integer��,Ϊ����ָ�������Է����һ��������������κ�һ������
        ///      SW_HIDE
        ///      ���ش��ڣ��״̬����һ������
        ///      SW_MINIMIZE
        ///      ��С�����ڣ��״̬����һ������
        ///      SW_RESTORE
        ///      ��ԭ���Ĵ�С��λ����ʾһ�����ڣ�ͬʱ�������״̬
        ///      SW_SHOW
        ///      �õ�ǰ�Ĵ�С��λ����ʾһ�����ڣ�ͬʱ�������״̬
        ///      SW_SHOWMAXIMIZED
        ///      ��󻯴��ڣ������伤��
        ///      SW_SHOWMINIMIZED
        ///      ��С�����ڣ������伤��
        ///      SW_SHOWMINNOACTIVE
        ///      ��С��һ�����ڣ�ͬʱ���ı�����
        ///      SW_SHOWNA
        ///      �õ�ǰ�Ĵ�С��λ����ʾһ�����ڣ����ı�����
        ///      SW_SHOWNOACTIVATE
        ///      ������Ĵ�С��λ����ʾһ�����ڣ�ͬʱ���ı�����
        ///      SW_SHOWNORMAL
        ///      ��SW_RESTORE��ͬ
        ///</summary>
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        ///<summary>���ش�������������ID</summary>
        ///<param name="hwnd">ָ�����ھ��</param>
        ///<param name="lpdwProcessId">ָ��һ������������װ��ӵ���Ǹ����ڵ�һ�����̵ı�ʶ��</param>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

        ///<summary>��������ظ����̶���</summary>
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary> Ϊָ���Ľ��̷����ڴ��ַ:�ɹ��򷵻ط����ڴ���׵�ַ </summary>
        [DllImport("kernel32.dll")]
        public static extern int VirtualAllocEx(int hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        /// <summary> ������д���ڴ��� </summary>
        /// <param name="hProcess"> ��OpenProcess���صĽ��̾�� </param>
        /// <param name="lpBaseAddress"> Ҫд���ڴ��׵�ַ,��д��֮ǰ,�˺������ȼ��Ŀ���ַ�Ƿ����,�������ɴ�д������� </param>
        /// <param name="lpBuffer"> ָ��Ҫд�����ݵ�ָ�� </param>
        /// <param name="nSize"> Ҫд����ֽ��� </param>
        /// <param name="vNumberOfBytesRead"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        /// <summary> ��ָ���ڴ��ж�ȡ�ֽڼ����� </summary>
        /// <param name="hProcess"> ����ȡ�ߵĽ��̾�� </param>
        /// <param name="lpBaseAddress"> ��ʼ��ȡ���ڴ��ַ </param>
        /// <param name="lpBuffer"> ���ݴ洢���� </param>
        /// <param name="nSize"> Ҫд������ֽ� </param>
        /// <param name="vNumberOfBytesRead"> ��ȡ���� </param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        #region Delegates

        public delegate bool EnumWindowsCallBack(IntPtr hWnd, int lParam);

        #endregion Delegates

        #region USER32

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.Dll")]
        public static extern int GetDlgCtrlID(IntPtr hWndCtl);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int MapWindowPoints(IntPtr hWnd, IntPtr hWndTo, ref POINT pt, int cPoints);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, out WINDOWINFO pwi);

        [DllImport("User32.Dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder param, int length);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hWnd, StringBuilder param, int length);

        [DllImport("user32.Dll")]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsCallBack lpEnumFunc, int lParam);

        [DllImport("user32.Dll")]
        public static extern bool EnumWindows(EnumWindowsCallBack lpEnumFunc, int lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr hParent, POINT pt, ChildFromPointFlags flags);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, StringBuilder param);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, char[] chars);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);

        #endregion USER32
    }
}