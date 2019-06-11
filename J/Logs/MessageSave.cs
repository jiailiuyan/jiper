/* 迹I柳燕
 *
 * FileName:   MessageSave.cs
 * Version:    1.0
 * Date:       2018/11/28 11:36:31
 * Author:     迹
 *
 *========================================
 *
 * @namespace  Ji.CommonHelper.J.Logs
 * @class      MessageSave
 * @extends
 *
 *========================================
 * 
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ji.CommonHelper.J.Logs
{
    /// <summary>  </summary>
    public class MessageSave
    {
        public FileInfo SaveFileInfo { get; set; }

        protected Thread WriteThread { get; set; }

        public MessageSave()
        {
        }

        public void Init(string filepath)
        {
            SaveFileInfo = new FileInfo(filepath);

            if (!SaveFileInfo.Directory.Exists)
            {
                SaveFileInfo.Directory.Create();
            }

            if (WriteThread != null && WriteThread.ThreadState != ThreadState.Running && WriteThread.ThreadState != ThreadState.WaitSleepJoin)
            {
                try
                {
                    WriteThread.Abort();
                }
                catch { }
                finally
                {
                    WriteThread = null;
                }
            }

            if (WriteThread == null)
            {
                try
                {
                    WriteThread = new Thread(new ThreadStart(WriteMessage));
                    WriteThread.IsBackground = true;
                    WriteThread.Start();
                }
                catch { }
            }
        }

        public void Close()
        {
            try
            {
                OnWorking = false;
                if (WriteThread != null)
                {
                    WriteThread.Abort();
                    WriteThread = null;
                }
            }
            catch { }
        }

        public void Save(string msg)
        {
            QueueMessages.Enqueue(msg);
        }

        public bool OnWorking { get; set; } = true;

        protected ConcurrentQueue<string> QueueMessages = new ConcurrentQueue<string>();

        private void WriteMessage()
        {
            try
            {
                while (true)
                {
                    using (var sw = new StreamWriter(SaveFileInfo.FullName, true, Encoding.UTF8))
                    {
                        while (!QueueMessages.IsEmpty)
                        {
                            QueueMessages.TryDequeue(out var msg);
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(msg))
                                {
                                    sw.WriteLine(msg);
                                }
                            }
                            catch (Exception)
                            {
                                QueueMessages.Enqueue(msg);
                            }
                        }
                        sw.Close();
                        sw.Dispose();
                    }

                    Thread.Sleep(10 * 1000);
                }
            }
            catch { }
        }
    }
}