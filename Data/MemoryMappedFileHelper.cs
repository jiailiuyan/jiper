using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ji.DataHelper
{
    /// <summary> 编辑内存映射文件 </summary>
    [Serializable]
    public class EditMemoryFile
    {
        /// <summary> 写入的说明 </summary>
        public string MemoryFileInfo { get; set; }

        public List<EditInfo> Projects { get; set; }

        public EditMemoryFile()
        {
            this.Projects = new List<EditInfo>();
        }
    }

    [Serializable]
    public class EditInfo
    {
        public string EditName { get; set; }

        public List<EditFile> Projects { get; set; }

        public EditInfo(string editName)
        {
            this.EditName = editName;
            this.Projects = new List<EditFile>();
        }
    }

    [Serializable]
    public class EditFile
    {
        public IntPtr Handle { get; set; }

        public string ProjectPath { get; set; }
    }

    public class MemoryMappedFileHelper<T>
    {
        private MemoryMappedFile memoryFile = null;
        private MemoryMappedViewStream stream = null;

        /// <summary> 最大容量 </summary>
        public static int FileSize = 1024 * 1024 * 10;

        public MemoryMappedFileHelper(string memoryName)
        {
            InitMemoryMappedFile(memoryName);
        }

        ~MemoryMappedFileHelper()
        {
            DisposeMemoryMappedFile();
        }

        private bool InitMemoryMappedFile(string memoryName)
        {
            try
            {
                memoryFile = MemoryMappedFile.CreateOrOpen(memoryName, FileSize);
                stream = memoryFile.CreateViewStream();
                return true;
            }
            catch { }
            return false;
        }

        public bool SaveMemoryFile(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                formatter.Serialize(stream, obj);
                return true;
            }
            catch { }
            return false;
        }

        public T LoadMemoryFile()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);

                //流中的序列化类型在其前18字节,如果读取不了,则判断流序列化异常
                byte[] data = new byte[18];
                stream.Read(data, 0, 18);
                var isexsit = false;
                foreach (byte item in data)
                {
                    if (item != 0)
                    {
                        isexsit = true;
                    }
                }

                //此处在没有任何映射文件存在的时候,会导致序列化失败,因此try
                if (isexsit)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(stream);
                }
                else
                {
                    return default(T);
                }
            }
            catch { }

            return default(T);
        }

        public void DisposeMemoryMappedFile()
        {
            if (stream != null)
            {
                stream.Close();
            }

            if (memoryFile != null)
            {
                memoryFile.Dispose();
            }
        }
    }
}