using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Params
{
    /// <summary> 键值对 对象 </summary>
    public class NameValuePair
    {
        public string Name { set; get; }
        public string Value { set; get; }

        public object Tag { set; get; }

        public NameValuePair(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}