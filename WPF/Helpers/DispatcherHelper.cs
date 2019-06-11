using System;
using System.Windows;
using System.Windows.Threading;

namespace Ji.CommonHelper.WPF.Helpers
{
    public static class DispatcherHelper
    {
        public static void DoEvents(this Dispatcher dispatcher)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Action<object> cb = obj =>
            {
                var f = obj as DispatcherFrame;
                if (f != null)
                {
                    f.Continue = false;
                }
            };
            dispatcher.BeginInvoke(cb, DispatcherPriority.Background, frame);
            Dispatcher.PushFrame(frame);
        }

        public static void AsyncUI(this Dispatcher dispatcher, Action action)
        {
            AsyncUIAction(dispatcher, action);
            dispatcher.DoEvents();
        }

        public static void AsyncUI(this Action action)
        {
            if (Application.Current != null && Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.AsyncUI(action);
            }
        }

        public static void AsyncUIAction(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher != null)
            {
                dispatcher.BeginInvoke(action);
            }
        }

        public static void AsyncAction(this Action action, AsyncCallback callback = null, object data = null)
        {
            if (action != null)
            {
                action.BeginInvoke(callback, data);
            }
        }
    }
}