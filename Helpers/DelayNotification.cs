// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-29-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 02-09-2018
// ***********************************************************************
// <copyright file="DelayNotification.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Threading;

namespace Ksy.Client.CommonHelper.Helpers
{
    /// <summary>
    /// 延迟推送.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelayNotification<T>
    {
        #region Public 字段

        /// <summary>
        /// 延迟计时器
        /// </summary>
        public DispatcherTimer DelayTimer;

        #endregion Public 字段

        #region Private 字段

        /// <summary>
        /// 值
        /// </summary>
        private T _value;

        /// <summary>
        /// 延迟推送通知委托.
        /// </summary>
        private Action<T> callBackAction;

        /// <summary>
        /// 延迟推送通知委托.
        /// </summary>
        private Action callBackActionNoArg;

        /// <summary>
        /// 延迟毫秒数
        /// </summary>
        private int delayMilliseconds;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayNotification{T}" /> class.
        /// </summary>
        /// <param name="callBackAction">The call back action.</param>
        /// <param name="delayMilliseconds">The delay milliseconds.</param>
        public DelayNotification(Action<T> callBackAction, int delayMilliseconds = 300)
        {
            DelayMilliseconds = delayMilliseconds;
            this.callBackAction = callBackAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayNotification{T}"/> class.
        /// </summary>
        /// <param name="callBackAction">The call back action.</param>
        /// <param name="delayMilliseconds">The delay milliseconds.</param>
        public DelayNotification(Action callBackAction, int delayMilliseconds = 300)
        {
            DelayMilliseconds = delayMilliseconds;
            this.callBackActionNoArg = callBackAction;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取或设置延迟推送毫秒数.
        /// </summary>
        /// <value>The delay milliseconds.</value>
        public int DelayMilliseconds
        {
            get { return delayMilliseconds; }
            set
            {
                delayMilliseconds = value;
                if (DelayTimer == null)
                {
                    DelayTimer = new DispatcherTimer();
                }
                DelayTimer.Interval = new TimeSpan(0, 0, 0, 0, value);
                DelayTimer.Tick -= DelayTimer_Tick;
                DelayTimer.Tick += DelayTimer_Tick;
            }
        }

        public bool IsValueChanged { get; private set; }

        /// <summary>
        /// 获取或设置值.
        /// </summary>
        /// <value>值.</value>
        public T Value
        {
            get { return _value; }
            set
            {
                /*
                 *    在 Ksy.Client.CommonHelper.Helpers.DelayNotification`1.set_Value(T value) 位置 D:\Work\GitProject\HaaS\Commons\CommonHelper\Helpers\DelayNotification.cs:行号 117
   在 Yuanbo.ChssClient.BusinessControls.YuJing.ViewModel.KCSettingViewModel.DoSearch() 位置 D:\Work\GitProject\HaaS\Yuanbo.ChssClient.BusinessControls\YuJing\ViewModel\KCSettingViewModel.cs:行号 287
   */
                if ((value == null && _value != null) || value!=null && !value.Equals(_value))
                {
                    IsValueChanged = true;
                    _value = value;
                    DelayTimer.Stop();
                    DelayTimer.Start();
                }
                else
                {
                    IsValueChanged = false;
                }
            }
        }

        #endregion Public 属性

        #region Private 方法

        /// <summary>
        /// Handles the Tick event of the DelayTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            if (callBackAction != null)
            {
                callBackAction(Value);
            }
            if (callBackActionNoArg != null)
            {
                callBackActionNoArg();
            }
            DelayTimer.Stop();
        }

        #endregion Private 方法
    }
}