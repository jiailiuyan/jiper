using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Convert
{
    public class HttpTaskTextJsonCovert<T> : HttpTaskTextBaseConvert<T>
    {
        public HttpTaskTextJsonCovert(Action<T> convertResult) : base(convertResult)
        {
        }

        public override object OnHttpTaskResponse(string text)
        {
            return null;
        }
    }
}