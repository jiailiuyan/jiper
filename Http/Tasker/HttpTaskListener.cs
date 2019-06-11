using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ji.CommonHelper.Http.Tasker.Params;

namespace Ji.CommonHelper.Http.Tasker
{
    public interface IHttpTaskListener
    {
        void OnHttpTaskFillParamsPre(HttpTaskParams param);

        void OnHttpTaskExecuteException(HttpTaskParams param, Exception e);
    }
}