using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Convert
{
    public class HttpTaskTextCovert : HttpTaskTextBaseConvert<string>
    {
        public HttpTaskTextCovert(Action<string> convertResult) : base(convertResult)
        {
        }

        public override object OnHttpTaskResponse(string text)
        {
            return text;
        }
    }
}