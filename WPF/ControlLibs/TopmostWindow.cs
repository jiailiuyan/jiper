/* 迹I柳燕
 *
 * FileName:   TopmostWindow.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.WPFHelper.ControlLibs
 * @class      TopmostWindow
 * @extends    Window
 *
 *             处理需要置顶显示的窗口
 *
 *========================================

 * 
 *
 * 
 *
 */

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Ji.WPFHelper.ControlLibs
{
    /// <summary> 创建自动置顶的窗口 </summary>
    public class TopmostWindow : Window
    {
        /// <summary> 当前窗口的单独快捷键，暂时实现单键 </summary>
        private Key keyNode = Key.None;

        /// <summary>
        /// 对于被MessageBox类覆盖时的置顶处理
        /// </summary>
        private bool enable = false;

        /// <summary>
        /// 对于焦点被切换时的指定处理
        /// </summary>
        private bool active = false;

        /// <summary> 对于内部没有处理的键盘消息的向外发送 </summary>
        public event EventHandler<KeyEventArgs> OnTopmosePreviewKeyDown;

        /// <summary> 对于内部没有处理的键盘消息的向外发送 </summary>
        /// <param name="args"></param>
        protected void OnTopmosePreviewKeyDownHandle(KeyEventArgs args)
        {
            if (OnTopmosePreviewKeyDown != null)
                OnTopmosePreviewKeyDown(this, args);
        }

        /// <summary> 在当前窗口关闭前调用，默认是隐藏当前窗口 </summary>
        public event EventHandler OnBeforeClosing;

        /// <summary> 在当前窗口关闭前调用，默认是隐藏当前窗口 </summary>
        protected void OnBeforeClosingHandle()
        {
            if (OnBeforeClosing != null)
                OnBeforeClosing(this, EventArgs.Empty);
        }

        /// <summary> 构造函数 </summary>
        public TopmostWindow()
        {
            this.SourceInitialized += WindowMsg_SourceInitialized;
            this.IsVisibleChanged += WindowTopmost_IsVisibleChanged;
        }

        /// <summary> 用以能够在子窗口自己处理事件 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void WindowMsg_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(new HwndSourceHook(WndProc));
        }

        /// <summary> 在此处理置顶逻辑 </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //窗口置顶事件
            switch (msg)
            {
                //是否可用
                case 0xA:
                    {
                        if ((int)wParam == 0)
                        {
                            if (!enable)
                            {
                                this.Topmost = false;

                                enable = true;
                            }
                        }
                        else if ((int)wParam == 1)
                        {
                            if (enable)
                            {
                                this.Topmost = true;
                                enable = false;
                            }
                        }
                        break;
                    }

                //是否激活
                case 0x1C:
                    {
                        if ((int)wParam == 0)
                        {
                            if (!active)
                            {
                                this.Topmost = false;
                                active = true;
                            }
                        }
                        else if ((int)wParam == 1)
                        {
                            if (active)
                            {
                                if (!enable)
                                {
                                    this.Topmost = true;
                                }
                                active = false;
                            }
                        }
                        break;
                    }

                default: break;
            }

            return IntPtr.Zero;
        }

        /// <summary> 显示窗口 </summary>
        public virtual void ShowWindow(bool focusShow = false)
        {
            if (this.Visibility == Visibility.Visible && !focusShow)
            {
                this.Hide();
            }
            else
            {
                this.Show();
                this.Topmost = true;
                this.Owner = Application.Current.MainWindow;
            }
        }

        /// <summary> 隐藏窗口 </summary>
        public virtual void HideWindow()
        {
            this.Hide();
        }

        /// <summary> 窗口的关闭事件重载 </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            OnBeforeClosingHandle();
            e.Cancel = true;
            this.Hide();
            return;
        }

        /// <summary> 发送当前窗口隐藏消息 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowTopmost_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                OnBeforeClosingHandle();
            }
        }

        /// <summary> 增加快捷键 </summary>
        /// <param name="key"></param>
        public void AddKeyNode(Key key)
        {
            keyNode = key;
        }

        /// <summary>
        /// 用于处理单个快捷键的调用显示
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (!(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && !(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (keyNode != Key.None && Keyboard.IsKeyDown(keyNode))
                {
                    ShowWindow();

                    e.Handled = true;
                }
            }

            if (!e.Handled)
            {
                OnTopmosePreviewKeyDownHandle(e);
                OnWindowTopmostPreviewKeyDown(e);
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary> 向外转发的键盘事件 </summary>
        /// <param name="e"></param>
        protected virtual void OnWindowTopmostPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
        }
    }
}