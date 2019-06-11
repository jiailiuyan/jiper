/* 迹I柳燕
 *
 * FileName:   AppBarWindow.cs
 * Version:    1.0
 * Date:       2016/11/25 11:21:42
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.WPF.ControlLibs.AppBarManager
 * @class      AppBarWindow
 * @extends
 *
 *========================================
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using Ji.SystemHelper;

namespace Ji.WPF.ControlLibs.AppBarManager
{
    /// <summary>  </summary>
    public abstract class AppBarWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        /// <summary> 获取或设置 AppBarLocation </summary>
        public AppBarLocation AppBarLocation
        {
            get { return (AppBarLocation)GetValue(AppBarLocationProperty); }
            set { SetValue(AppBarLocationProperty, value); }
        }

        /// <summary> 注册 AppBarLocation 依赖属性 </summary>
        public static readonly DependencyProperty AppBarLocationProperty = DependencyProperty.Register("AppBarLocation", typeof(AppBarLocation), typeof(AppBarWindow), new PropertyMetadata(AppBarLocation.Right, (s, e) => (s as AppBarWindow).ChangeLocation()));

        public virtual int ViewWidth { get; set; }

        public virtual int ViewHeight { get; set; }

        private readonly AppBarAction AppBar = AppBarAction.Creat();

        protected IntPtr Handle = IntPtr.Zero;

        protected bool IsReRegister = false;

        public AppBarWindow()
        {
            ViewWidth = 100;
            ViewHeight = 100;

            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
        }

        public AppBarWindow(AppBarLocation appbarlocation) : this()
        {
            this.AppBarLocation = appbarlocation;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.UnregisterAppBar();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(new HwndSourceHook(WndProc));
            Handle = source.Handle;

            RegisterBar();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (this.IsReRegister && this.AppBar.IsBarRegistered)
            {
                this.UnregisterAppBar();
            }
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == AppBar.WndProcCallBack && wParam.ToInt32() == AppBarAction.ABN_POSCHANGED)
            {
                SetAppBar();
                handled = true;
            }
            else if (msg == AppBarAction.WM_NCLBUTTONDOWN)
            {
                this.IsReRegister = true;
            }
            else if (msg == AppBarAction.WM_EXITSIZEMOVE)
            {
                this.IsReRegister = false;
                RegisterBar();
            }
            return IntPtr.Zero;
        }

        protected virtual void RegisterBar()
        {
            if (this.Handle != IntPtr.Zero)
            {
                this.Visibility = Visibility.Hidden;
                this.AppBar.RegisterBar(this.Handle, this.AppBarLocation, ScreenResolution.RealScreenWidth, ScreenResolution.RealScreenHeight, this.ViewWidth, this.ViewHeight);
            }
        }

        protected virtual void SetAppBar()
        {
            if (this.Handle != IntPtr.Zero)
            {
                this.Visibility = Visibility.Visible;
                this.AppBar.ABSetPos(this.Handle, this.AppBarLocation, ScreenResolution.RealScreenWidth, ScreenResolution.RealScreenHeight, this.ViewWidth, this.ViewHeight);
            }
        }

        protected virtual void UnregisterAppBar()
        {
            if (this.Handle != IntPtr.Zero)
            {
                this.AppBar.UnregisterBar(this.Handle);
            }
        }

        private void ChangeLocation()
        {
            SetAppBar();
        }
    }
}