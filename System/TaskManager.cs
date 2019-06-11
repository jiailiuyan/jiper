using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace System
{
    public interface IResult
    {
        #region Public 属性

        bool State { get; set; }

        #endregion Public 属性
    }

    public class TaskManager
    {
        #region Public 方法

        /// <summary>
        /// 开始新的任务
        /// </summary>
        /// <param name="completeAction">任务完成时执行</param>
        /// <param name="actions">任务集合</param>
        public static void StartNews<T>(Action<IEnumerable<T>> completeAction, params Func<T>[] actions) where T : IResult
        {
            var completedCount = 0;
            var res = new List<T>();
            var b = true;
            var CancellationTokenSourceList = new List<CancellationTokenSource>();
            foreach (var item in actions)
            {
                var cancel = new CancellationTokenSource();
                CancellationTokenSourceList.Add(cancel);
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var t = item();
                        res.Add(t);
                        b &= t.State;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //LogX.Error(ex, "StartNews has exception, method:{0}", item.Method.Name);
                        b &= false;
                        throw;
                    }

                }, cancel.Token).ContinueWith((t) =>
                {
                    completedCount++;
                    var flag = true;
                    if (!b)
                    {
                        foreach (var c in CancellationTokenSourceList)
                        {
                            c.Cancel();
                        }
                    }
                    else
                    {
                        flag = completedCount == actions.Length;
                    }
                    if (flag)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            completeAction(res);
                        }));
                    }
                });
            }
        }

        #endregion Public 方法
    }
}