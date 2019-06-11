using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Convert
{
    public abstract class HttpTaskTextBaseConvert<T> : HttpTaskResultTextConvert
    {
        private Action<T> mConvertResultAction;

        public HttpTaskTextBaseConvert(Action<T> convertResult)
        {
            mConvertResultAction = convertResult;
        }

        public abstract object OnHttpTaskResponse(string text);

        public void OnHttpTaskResult(object obj)
        {
            if (obj is T)
                mConvertResultAction((T)obj);
            else
                mConvertResultAction(default(T));
        }
    }
}