using System;
using System.ComponentModel;
using System.Net;
using Ji.CommonHelper.Actions;
using Ji.CommonHelper.Http.Tasker.Convert;
using Ji.CommonHelper.Http.Tasker.Params;
using Ji.CommonHelper.Http.Tasker.Util;

namespace Ji.CommonHelper.Http.Tasker
{
    /// <summary> 网络任务 </summary>
    public class HttpTask
    {
        private static CookieContainer commonCookieContainer = new CookieContainer();
        public static IHttpTaskListener HttpTaskListener;

        private object mLockObj = new object();
        private HttpTaskParams mHttpTaskParams;
        private HttpWebRequest mHttpWebRequest;
        private CookieContainer mCustomCookieContainer;

        private BackgroundWorker mWorker;
        private Exception mWorkException;

        public Action OnPreAction;
        public Action<Exception> OnFailedAction;
        private HttpTaskResultConvert mResultConvert;

        public HttpTask()
        {
        }

        public HttpTask(HttpTaskParams httpTaskParams)
        {
            SetHttpTaskParams(httpTaskParams);
        }

        public void SetHttpTaskParams(HttpTaskParams htp)
        {
            mHttpTaskParams = htp;
        }

        public void SetCustomCookieContainer(CookieContainer cookieContainer)
        {
            mCustomCookieContainer = cookieContainer;
        }

        public void SetHttpTaskResultConvert(HttpTaskResultConvert convert)
        {
            mResultConvert = convert;
        }

        /// <summary> 获取当前请求对象 </summary>
        /// <returns></returns>
        public HttpWebRequest getRequest()
        {
            return mHttpWebRequest;
        }

        /// <summary> 取消网络请求 </summary>
        public void Cancel()
        {
            lock (mLockObj)
            {
                HttpTaskUtil.Abort(mHttpWebRequest);
                TaskHelper.CancelWorker(mWorker);
            }
        }

        /// <summary> 网络任务是否在运行 如果worker已取消或线程已运行完毕 返回true </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return mWorker == null ? false : IsCancelled() || mWorker.IsBusy;
        }

        /// <summary> 网络任务是否已取消 </summary>
        /// <returns></returns>
        public bool IsCancelled()
        {
            return mWorker.CancellationPending;
        }

        public void ExecuteAsync()
        {
            if (mWorker == null)
            {
                mWorker = new BackgroundWorker();
                //mWorker.WorkerReportsProgress = true;
                mWorker.WorkerSupportsCancellation = true;
                mWorker.DoWork += new DoWorkEventHandler(onHttpTaskDoWork);
                //mWorker.ProgressChanged += new ProgressChangedEventHandler(OnWorkerProgressChanged);
                mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(onHttpTaskWorkCompleted);
                mWorker.RunWorkerAsync();

                if (OnPreAction != null)
                    OnPreAction();
            }
        }

        public string ExecuteString()
        {
            try
            {
                return executeStringResp();
            }
            catch (Exception e)
            {
                callbackOnHttpTaskExecuteException(mHttpTaskParams, e);
            }

            return string.Empty;
        }

        public T ExecuteJson<T>()
        {
            try
            {
                var p = executeStringResp();
                //executeStringResp 中初始化了httptaskwebrequest
                return default(T);
            }
            catch (Exception e)
            {
                callbackOnHttpTaskExecuteException(mHttpTaskParams, e);
            }

            return default(T);
        }

        public byte[] ExecuteBytes()
        {
            try
            {
                callbackOnHttpTaskFillParamsPre(mHttpTaskParams);
                mHttpWebRequest = HttpTaskUtil.CreateHttpWebRequest(mHttpTaskParams);
                setCookieContainer();
                return HttpTaskUtil.ExecuteBytesResp(mHttpWebRequest, mHttpTaskParams);
            }
            catch (Exception e)
            {
                callbackOnHttpTaskExecuteException(mHttpTaskParams, e);
            }

            return null;
        }

        /// <summary> 获取cookie </summary>
        /// <param name="uri"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCookie(string uri, string key)
        {
            try
            {
                CookieContainer container = mCustomCookieContainer;
                if (container == null)
                    container = commonCookieContainer;

                CookieCollection cc = container.GetCookies(new Uri(uri));
                if (cc != null && cc.Count > 0)
                {
                    for (int i = 0; i < cc.Count; i++)
                    {
                        if (cc[i].Name.Equals(key))
                            return cc[i].Value;
                    }
                }
            }
            catch { }

            return string.Empty;
        }

        /// <summary> 子线程执行网络任务 </summary>
        /// <param name="sender"></param>
        /// <param name="args">  </param>
        private void onHttpTaskDoWork(object sender, DoWorkEventArgs args)
        {
            try
            {
                args.Result = executeHttpWebRequestAndCallback();
            }
            catch (Exception e)
            {
                mWorkException = e;
                callbackOnHttpTaskExecuteException(mHttpTaskParams, e);
            }
        }

        /// <summary> 网络任务执行完成 </summary>
        /// <param name="sender"></param>
        /// <param name="args">  </param>
        private void onHttpTaskWorkCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            if (IsCancelled())
                return;

            if (mWorkException == null)
            {
                if (mResultConvert != null)
                {
                    mResultConvert.OnHttpTaskResult(args.Result);
                }
            }
            else
            {
                if (OnFailedAction != null)
                {
                    OnFailedAction.Invoke(mWorkException);
                }
            }
        }

        private object executeHttpWebRequestAndCallback()
        {
            if (mResultConvert is HttpTaskResultTextConvert)
            {
                HttpTaskResultTextConvert lisn = (HttpTaskResultTextConvert)mResultConvert;
                string text = executeStringResp();
                return lisn.OnHttpTaskResponse(text);
            }
            else
            {
                //默认执行一次
                return executeStringResp();
            }
        }

        private string executeStringResp()
        {
            callbackOnHttpTaskFillParamsPre(mHttpTaskParams);
            lock (mLockObj)
            {
                mHttpWebRequest = HttpTaskUtil.CreateHttpWebRequest(mHttpTaskParams);
            }
            setCookieContainer();
            return HttpTaskUtil.ExecuteStringResp(mHttpWebRequest, mHttpTaskParams);
        }

        private void callbackOnHttpTaskFillParamsPre(HttpTaskParams param)
        {
            if (HttpTaskListener != null)
            {
                HttpTaskListener.OnHttpTaskFillParamsPre(mHttpTaskParams);
            }
        }

        private void callbackOnHttpTaskExecuteException(HttpTaskParams param, Exception e)
        {
            if (HttpTaskListener != null)
            {
                HttpTaskListener.OnHttpTaskExecuteException(param, e);
            }
        }

        private void setCookieContainer()
        {
            if (mCustomCookieContainer == null)
                mHttpWebRequest.CookieContainer = commonCookieContainer;
            else
                mHttpWebRequest.CookieContainer = mCustomCookieContainer;
        }
    }
}