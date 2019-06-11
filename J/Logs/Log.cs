/* 迹I柳燕
 *
 * FileName:   Log.cs
 * Version:    1.0
 * Date:       2018/11/27 11:46:54
 * Author:     迹
 *
 *========================================
 *
 * @namespace  System
 * @class      Log
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary> 迹。新日志及统计系统 </summary>
    public class LogX : IDisposable
    {
        public static event Action<TipType, string> OnTip = delegate { };

        protected static ILogSave LogSave { get; set; }

        public static ILogReport LogReport { get; private set; }

        protected static ILogSetting LogSetting { get; set; }

        protected static ILogViewer LogViewer { get; set; }

        protected static IDataStatistic DataStatistic { get; set; }

        public static string LogFloder { get; }

        static LogX()
        {
            LogSetting = Locator.GetInstance<ILogSetting>();
            if (LogSetting != null)
            {
                LogSave = Locator.GetInstance<ILogSave>();
                if (LogSave != null)
                {
                    LogSave.Start(LogSetting);
                }
            }

            LogReport = Locator.GetInstance<ILogReport>();

            LogViewer = Locator.GetInstance<ILogViewer>();

            DataStatistic = Locator.GetInstance<IDataStatistic>();
            if (DataStatistic != null)
            {
                DataStatistic.Start();
            }
        }

        /// <summary>
        /// 数据统计
        /// </summary>
        public static void Statistics(DataStatisticType type, object msg = null)
        {
            SetConsole(msg + "");
            DataStatistic?.Send(new DataStatisticItem() { StatisticType = type, Data = msg });
        }

        public static void Statistics(DataStatisticItem item)
        {
            SetConsole(item?.ToString() + "");
            DataStatistic?.Send(item);
        }

        /// <summary>
        /// 记录 崩溃
        /// 崩溃数据会被上传
        /// </summary>
        public static void Crash(Exception ex)
        {
            SetConsole(ex.StackTrace);

            DataStatistic?.Send(new DataStatisticItem() { StatisticType = DataStatisticType.Crash, Data = ex });
            LogSave?.Save(ex);
        }

        public static void Error(Exception ex)
        {
            SetConsole(ex.StackTrace);

            LogSave?.Save(ex);
        }

        public static void Error(Exception ex, string msg)
        {
            SetConsole(msg);
            SetConsole(ex.StackTrace);

            LogSave?.Save(ex);
        }

        /// <summary>
        /// 记录 警告
        /// </summary>
        public static void Warn(string msg)
        {
            SetConsole(msg);

            LogSave?.Debug(msg);
        }

        /// <summary>
        /// 记录 调试信息
        /// </summary>
        public static void Debug(string msg)
        {
            SetConsole(msg);

            LogSave?.Debug(msg);
        }

        public static event Action<string> OnTiping = delegate { };
        public static void OnRaiseTiping(string msg)
        {
            OnTiping?.Invoke(msg);
        }

        public static void Tip(string tip, params string[] args)
        {
            var tps = string.Format(tip, args);
            Tip(tps);
        }

        /// <summary>
        /// 只进行提示
        /// </summary>
        public static void Tip(string tip, TipType tt = TipType.Normal)
        {
            SetConsole(tip);

            OnTip?.Invoke(tt, tip);

            LogViewer?.ShowTip(tip);
        }

        private static void SetConsole(string msg)
        {
            if (LogSetting?.IsDebug == true && !string.IsNullOrWhiteSpace(msg))
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        #region IDisposable

        protected bool IsDispose { get; set; }

        /// <summary> 析构函数 </summary>
        ~LogX()
        {
            Dispose(false);
        }

        /// <summary> Dispose 重载 </summary>
        /// <param name="disposing"> true 时释放所有资源，false 释放非托管资源 </param>
        private void Dispose(bool disposing)
        {
            if (!this.IsDispose)
            {
                this.IsDispose = true;
                if (disposing)
                {
                    ReleaseUnTrust();
                }

                ReleaseTrust();
            }
        }

        /// <summary> 手动释放资源 </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary> 释放托管数据 </summary>
        protected virtual void ReleaseTrust()
        {
            if (LogSave != null)
            {
                LogSave.Stop();
                LogSave = null;
            }

            if (DataStatistic != null)
            {
                DataStatistic.Stop();
                DataStatistic = null;
            }
        }

        /// <summary> 释放非托管数据 </summary>
        protected virtual void ReleaseUnTrust()
        {

        }

        #endregion IDisposable

    }


    public enum TipType
    {
        Normal = 0,

        Error,

        Warn,
    }
}
