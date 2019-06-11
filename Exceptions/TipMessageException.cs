using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Exceptions
{
    /// <summary>
    /// 给用户的提示信息，用于中断程序运行，不是程序异常
    /// </summary>
    public class TipMessageException : Exception
    {
        public TipMessageException() : base()
        {

        }

        public TipMessageException(string message) : base(message)
        {

        }

        public TipMessageException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public TipMessageException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public TipMessageException(bool needLog, string message) : base(message)
        {
            this.NeedLog = needLog;
        }

        /// <summary>
        /// 是否需要写日志
        /// </summary>
        public bool NeedLog
        {
            get;
            set;
        }
    }
}
