// ***********************************************************************
// 程序集            : Ksy.Client.CommonHelper
// 作者              : Lyon
// 创建日期          : 05-10-2018
//
// 最后编辑者        : Lyon
// 最后编辑时间      : 05-10-2018
// ***********************************************************************
// <版权所有 文件="DispatcherHelper.cs" 组织="KingSoft">
//     Copyright © KingSoft 2017
// </版权所有>
// ***********************************************************************

using System.Text;
using System.Windows.Threading;

namespace System
{
    /// <summary>
    /// 调度帮助类.
    /// </summary>
    public static class DispatcherHelper
    {
        #region Private 属性

        /// <summary>
        /// 获取UI调度.
        /// </summary>
        /// <value>UI调度.</value>
        private static Dispatcher UIDispatcher { get; set; }

        #endregion Private 属性

        #region Public 方法

        /// <summary>
        /// as the synchronize UI.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="action">The action.</param>
        public static void ASyncUI(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher != null && action != null &&
                dispatcher.Thread != null && dispatcher.Thread.IsAlive)
            {
                dispatcher.BeginInvoke(action);
            }
        }

        /// <summary>
        /// as the synchronize UI.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void ASyncUI(this Action action)
        {
            CheckBeginInvokeOnUI(action);
        }

        /// <summary>
        /// 检查UI上的开始调用.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void CheckBeginInvokeOnUI(Action action)
        {
            if (action == null)
                return;
            CheckDispatcher();
            if (UIDispatcher.CheckAccess())
                action();
            else
                UIDispatcher.Invoke(action);
        }
        /// <summary>
        /// 同步调用
        /// </summary>
        /// <param name="action">The action.</param>
        public static void InvokeOnUI(Action action)
        {
            if (action == null)
                return;
            CheckDispatcher();
            if (UIDispatcher.CheckAccess())
                action();
            else
                UIDispatcher.Invoke(action);
        }
        /// <summary>
        /// 初始化实例.
        /// </summary>
        public static void Initialize()
        {
            if (UIDispatcher != null && UIDispatcher.Thread.IsAlive)
                return;
            UIDispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// 重置实例.
        /// </summary>
        public static void Reset()
        {
            UIDispatcher = null;
        }

        /// <summary>
        /// 异步运行.
        /// </summary>
        /// <param name="action">委托.</param>
        /// <returns>DispatcherOperation.</returns>
        public static DispatcherOperation RunAsync(Action action)
        {
            CheckDispatcher();
            return UIDispatcher.BeginInvoke(action);
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 检查调度.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        private static void CheckDispatcher()
        {
            if (UIDispatcher == null)
            {
                var error = new StringBuilder("The DispatcherHelper is not initialized.");
                error.AppendLine();
                error.Append("Call DispatcherHelper.Initialize() in the static App constructor.");
                throw new InvalidOperationException(error.ToString());
            }
        }

        #endregion Private 方法
    }
}