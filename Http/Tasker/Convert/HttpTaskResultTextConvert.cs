using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Convert
{
    public interface HttpTaskResultTextConvert : HttpTaskResultConvert
    {
        object OnHttpTaskResponse(string text);
    }
}