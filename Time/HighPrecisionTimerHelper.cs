/* 迹I柳燕
*
* FileName:   HighPrecisionTimerHelper.cs
* Version:    1.0
* Date:       2014.03.18
* Author:     Ji
*
*========================================
* @namespace  Ji.TimeHelper
* @class      HighPrecisionTimerHelper
* @extends
*
*             对于高精度计时器的使用
*             此处代码包括注释 Copy 于 wmesci 在此鸣谢
*             wmesci:http://www.cnblogs.com/wmesci
*
*========================================
* Hi,小喵喵...
* 
*
* 
*
*/

using System;
using System.Runtime.InteropServices;

namespace Ji.TimeHelper
{
    public static class HighPrecisionTimerHelper
    {
        #region 毫秒级精度

        /// <summary> 毫秒级精度 获取系统启动后经过的毫秒数，包装了GetTickCount
        /// 否决的，经测试，此项极其不准</summary>
        public static int PrecisionTimerOfMillisecond
        {
            get
            {
                return Environment.TickCount;
            }
        }

        /// <summary> 毫秒级精度 从操作系统启动到现在所经过的毫秒数，精度为1毫秒，经简单测试发现其实误差在大约在15ms左右 </summary>
        /// <returns> 从操作系统启动到现在所经过的毫秒数 </returns>
        [DllImport("kernel32")]
        public static extern uint GetTickCount();

        #endregion 毫秒级精度

        #region 微秒级精度

        /// <summary> 微秒级精度 用于得到高精度计时器（如果存在这样的计时器）的值。 </summary>
        /// <param name="tick"></param>
        /// <returns> 如果安装的硬件不支持高精度计时器,函数将返回 false </returns>
        [DllImport("kernel32.dll ")]
        private static extern bool QueryPerformanceCounter(ref long tick);

        /// <summary> 微秒级精度 返回硬件支持的高精度计数器的频率
        /// 但是据说该API在节能模式的时候结果偏慢，超频模式的时候又偏快，而且用电池和接电源的时候效果还不一样（笔记本）。没有测试过。</summary>
        /// <param name="tick"></param>
        /// <returns> 如果安装的硬件不支持高精度计时器,函数将返回 false </returns>
        [DllImport("kernel32.dll ")]
        private static extern bool QueryPerformanceFrequency(ref long tick);

        /// <summary> 微秒级精度 用于得到高精度计时器（如果存在这样的计时器）的值 ，如果安装的硬件不支持高精度计时器 此处将返回 -1 </summary>
        public static long PrecisionTimerOfMicrosecondUseCounter
        {
            get
            {
                long time = -1;
                QueryPerformanceCounter(ref time);
                return time;
            }
        }

        /// <summary> 微秒级精度 返回硬件支持的高精度计数器的频率 如果安装的硬件不支持高精度计时器 此处将返回 -1
        /// 但是据说该API在节能模式的时候结果偏慢，超频模式的时候又偏快，而且用电池和接电源的时候效果还不一样（笔记本）。没有测试过 </summary>
        public static long PrecisionTimerOfMicrosecondUseFrequency
        {
            get
            {
                long time = -1;
                QueryPerformanceFrequency(ref time);
                return time;
            }
        }

        #endregion 微秒级精度

        #region 纳秒级精度

        [DllImport("kernel32.dll")]
        private static extern int VirtualProtect(IntPtr lpAddress, int dwSize, int flNewProtect, ref int lpflOldProtect);

        private delegate ulong GetTickDelegate();

        private static readonly IntPtr Addr;
        private static readonly GetTickDelegate getTick;

        /// <summary> rdtsc, ret的机器码 </summary>
        private static readonly byte[] asm = { 0x0F, 0x31, 0xC3 };

        static HighPrecisionTimerHelper()
        {
            Addr = Marshal.AllocHGlobal(3);
            int old = 0;
            VirtualProtect(Addr, 3, 0x40, ref old);
            Marshal.Copy(asm, 0, Addr, 3);
            getTick = (GetTickDelegate)Marshal.GetDelegateForFunctionPointer(Addr, typeof(GetTickDelegate));
        }

        /// <summary> 纳秒级精度 获取纳秒级精度
        /// 调用注意事项
        /// 1. 需要屏蔽 [assembly: AllowPartiallyTrustedCallers] （仅可由完全受信任的调用方调用具有强名称的程序集）
        /// 2. (此项只适用在 Win32 平台上 )
        /// 以下是百度看到的：
        /// 这个指令在超线程和多核CPU上用来计算时间不是很准确
        /// 1 根据intel的介绍，由于在现代的处理器中都具有指令乱序执行的功能，因此在有些情况下rdtsc指令并不能很好的反映真实情况。解决方法是，在rdtsc之前加一些cpuid指令，使得rdtsc后面的指令顺序执行。
        /// 2 另外，rdtsc是一条慢启动的指令，第一次执行需要比较长的启动时间，而第二次之后时间就比较短了，也就是说，这条指令在第一次工作时需要比较长的时钟周期，之后就会比较短了。所以可以多运行几次，避过第一次的消耗。
        /// 3 大家在测试某一个函数的cpu周期的时候，如果精度要求很高，需要减去rdtsc的周期消耗。我在至强2.6G上测试的结果是大约500多个时钟周期，我想这是应该考虑在内的，很多小的函数也就是几K个时钟周期。
        /// 4 一定要注意cache的影响。如果你在对同一组数据进行操作，第一次操作往往要比后面几次时间开销大，原因就在于cache的缓存功能，而这一部分是不可见的。
        /// </summary>
        public static ulong PrecisionTimerOfNanosecond
        {
            get
            {
                return getTick();
            }
        }

        #endregion 纳秒级精度
    }
}