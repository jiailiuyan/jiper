using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace JZYD.TY.Update.Helpers
{
    public class Downloader
    {
        private static readonly Downloader instance = new Downloader();
        public static Downloader Instance { get { return instance; } }

        private string DownLoadUrl { get; set; }

        private string FileName { get; set; }

        private string saveDir;

        public string SaveDir
        {
            get { return saveDir; }
            set { saveDir = value; }
        }

        private int length;

        public int Length
        {
            get { return length; }
            private set
            {
                length = value;
                this.OnDownloadLengthChanged(length);
            }
        }

        public int AllLength { get; private set; }

        public delegate void DownloadLength(int currentlength);

        public event DownloadLength OnDownloadLengthChanged = delegate { };

        public delegate void DownloadComplated(string filepath);

        public event DownloadComplated OnDownloadComplated = delegate { };

        protected Downloader()
        {
        }

        private void GetAllFileLength()
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(this.DownLoadUrl);
            var response = request.GetResponse();
            AllLength = (int)response.ContentLength;
            response.Close();
            request.Abort();
            request = null;
        }

        private FileStream fileStream = null;

        /// <summary> 开始异步下载 </summary>
        /// <param name="url">    </param>
        /// <param name="filedir"></param>
        public void StartDownload(string url, string filedir = "")
        {
            Task.Factory.StartNew(() =>
            {
                this.SaveDir = !string.IsNullOrWhiteSpace(filedir) ? filedir : Path.Combine(Environment.CurrentDirectory, "UpdateFiles");

                if (!Directory.Exists(this.SaveDir))
                {
                    Directory.CreateDirectory(this.SaveDir);
                }

                this.DownLoadUrl = url;
                GetAllFileLength();

                this.FileName = this.DownLoadUrl.Substring(url.LastIndexOf('/') + 1);
                string fullName = Path.Combine(this.SaveDir, this.FileName);

                using (fileStream = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    this.Length = (int)fileStream.Length;
                    fileStream.Position = fileStream.Length;
                    var issave = this.Length < this.AllLength;
                    if (issave)
                    {
                        var request = (HttpWebRequest)HttpWebRequest.Create(url);
                        request.AddRange("bytes", fileStream.Length, this.AllLength);
                        var response = (HttpWebResponse)request.GetResponse();
                        if (response != null)
                        {
                            if (response.Headers["Content-Range"] == null)
                            {
                                fileStream.Position = 0;
                            }

                            var buffer = new byte[102400];
                            using (var stream = response.GetResponseStream())
                            {
                                int size = stream.Read(buffer, 0, buffer.Length);
                                while (size > 0)
                                {
                                    fileStream.Write(buffer, 0, size);

                                    this.Length += size;
                                    size = stream.Read(buffer, 0, buffer.Length);
                                }
                            }
                            fileStream.Flush(true);
                            response.Close();
                        }
                    }
                }
                this.OnDownloadComplated(fullName);
            });
        }
    }
}