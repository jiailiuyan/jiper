/* 迹I柳燕
 *
 * FileName:   ConsoleWindow.cs
 * Version:    1.0
 * Date:       2018/11/27 14:29:29
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J
 * @class      ConsoleWindow
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ji.CommonHelper.J
{
    /// <summary>  </summary>
    public class ConsoleWindow : Window
    {
        private readonly ObservableCollection<string> ViewStrs = new ObservableCollection<string>();

        public ConsoleWindow()
        {
            this.Loaded += this.ConsoleWindow_Loaded;
            var sc = new ScrollViewer();
            var IC = new ItemsControl();
            sc.Content = IC;
            this.Content = sc;
            IC.ItemsSource = ViewStrs;

            this.Title = "Debug Info Viewer";

            this.Width = 600;
            this.Height = 800;
        }

        private void Dtl_OnWriting(string obj)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                lock (ViewLock)
                {
                    if (ViewStrs.Count > 200)
                    {
                        ViewStrs.Clear();
                    }

                    if (!string.IsNullOrWhiteSpace(obj))
                    {
                        ViewStrs.Add(obj);
                    }
                }
            }));
        }

        private readonly object ViewLock = new object();

        private void ConsoleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dl = new DebugTraceListener();
            dl.OnWriting += this.Dtl_OnWriting;
            Debug.Listeners.Clear();
            Debug.Listeners.Add(dl);

            Debug.WriteLine("Debug.WriteLine Viewer");

            var tl = new DebugTraceListener();
            tl.OnWriting += this.Dtl_OnWriting;
            Trace.Listeners.Clear();
            Trace.Listeners.Add(tl);
            Trace.WriteLine("Trace.WriteLine Viewer");

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            Console.SetOut(sw);

            Console.WriteLine("Console.WriteLine Viewer");

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);

                    sw.Flush();

                    if (ms.Length <= 0)
                    {
                        continue;
                    }

                    ms.Position = 0;
                    var datas = new byte[ms.Length];
                    ms.Read(datas, 0, (int)ms.Length);
                    var sf = Encoding.UTF8.GetString(datas);

                    sw.Close();

                    ms = new MemoryStream();
                    sw = new StreamWriter(ms);
                    Console.SetOut(sw);

                    this.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lock (ViewLock)
                        {
                            if (ViewStrs.Count > 200)
                            {
                                ViewStrs.Clear();
                            }

                            if (!string.IsNullOrWhiteSpace(sf))
                            {
                                ViewStrs.Add(sf);
                            }
                        }
                    }));
                }
            });
        }
    }


    public class DebugTraceListener : TraceListener
    {
        public event Action<string> OnWriting = delegate { };

        public override void Write(string message)
        {
            OnWriting?.Invoke(message);
        }

        public override void WriteLine(string message)
        {
            OnWriting?.Invoke(message);
        }
    }
}
