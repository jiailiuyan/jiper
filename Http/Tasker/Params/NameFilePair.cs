using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ji.CommonHelper.Http.Tasker.Params
{
    /// <summary> 文件键值对 </summary>
    public class NameFilePair
    {
        public string Name { set; get; }
        public string FileName { set; get; }
        public string FilePath { set; get; }
        public string ContentType { set; get; }

        public NameFilePair(string name, string filePath) : this(name, string.Empty, filePath, string.Empty)
        {
        }

        public NameFilePair(string name, string filePath, string contentType) : this(name, string.Empty, filePath, contentType)
        {
        }

        public NameFilePair(string name, string fileName, string filePath, string contentType)
        {
            Name = name;
            FileName = fileName;
            FilePath = filePath;
            ContentType = contentType;
        }
    }
}