using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Ji.DataHelper;
using JZYD.TY.Common.Http.Tasker;
using JZYD.TY.Common.Http.Tasker.Params;
using JZYD.TY.Common.Managers;

namespace JZYD.TY.Common.Util
{
    public enum ValidImageFormatEnum
    {
        NONE = 0,
        JPEG = 1,
        JPG = 2,
        PNG = 3
    }

    public class ImageUtil
    {
        #region Methods

        public static string CopyImage(string imgPath, string floder, string starttxt = "")
        {
            if (!string.IsNullOrWhiteSpace(imgPath))
            {
                try
                {
                    var format = Path.GetExtension(imgPath);
                    var md5Str = starttxt + MD5Helper.GetCustomMD5(imgPath);
                    lock (md5Str)
                    {
                        var fileName = md5Str + format;

                        var imagefloder = ApplicationManager.Instance.ApplicationData.CommonFloder + "\\Images\\" + floder;
                        if (!System.IO.Directory.Exists(imagefloder))
                        {
                            System.IO.Directory.CreateDirectory(imagefloder);
                        }
                        var tempfloder = ApplicationManager.Instance.ApplicationData.CommonFloder + "\\Temps\\" + floder;
                        if (!System.IO.Directory.Exists(tempfloder))
                        {
                            System.IO.Directory.CreateDirectory(tempfloder);
                        }

                        var imagefile = imagefloder + "\\" + fileName;
                        var tmbimagefile = tempfloder + "\\" + fileName;

                        if (ImageUtil.IsImageExistes(imagefile))
                        {
                            // 正常显示图片才需要处理缩略图
                            if (string.IsNullOrWhiteSpace(starttxt) && !ImageUtil.IsImageExistes(tmbimagefile))
                            {
                                var bmp = new Bitmap(imagefile);
                                SaveClipImage(bmp, tmbimagefile);
                                bmp.Dispose();
                            }

                            return fileName;
                        }
                        File.Copy(imgPath, imagefile);
                        if (string.IsNullOrWhiteSpace(starttxt) && !ImageUtil.IsImageExistes(tmbimagefile))
                        {
                            var bmp = new Bitmap(imagefile);
                            SaveClipImage(bmp, tmbimagefile);
                            bmp.Dispose();
                        }
                        return fileName;
                    }
                }
                catch { }
            }

            return string.Empty;
        }

        public static string DownloadImage(string url, string floder, string starttxt = "")
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    var format = ImageUtil.GetImageFormat(url);
                    if (format == ValidImageFormatEnum.NONE)
                    {
                        format = ValidImageFormatEnum.PNG;
                    }

                    var md5Str = starttxt + MD5Helper.GetCustomMD5(url);
                    lock (md5Str)
                    {
                        string fileName;
                        switch (format)
                        {
                            case ValidImageFormatEnum.JPEG:
                                fileName = md5Str + ".jpeg";

                                break;

                            case ValidImageFormatEnum.JPG:
                                fileName = md5Str + ".jpg";
                                break;

                            case ValidImageFormatEnum.PNG:
                                fileName = md5Str + ".png";
                                break;

                            default:
                                return string.Empty;
                        }

                        var imagefloder = ApplicationManager.Instance.ApplicationData.CommonFloder + "\\Images\\" + floder;
                        if (!System.IO.Directory.Exists(imagefloder))
                        {
                            System.IO.Directory.CreateDirectory(imagefloder);
                        }
                        var tempfloder = ApplicationManager.Instance.ApplicationData.CommonFloder + "\\Temps\\" + floder;
                        if (!System.IO.Directory.Exists(tempfloder))
                        {
                            System.IO.Directory.CreateDirectory(tempfloder);
                        }

                        var imagefile = imagefloder + "\\" + fileName;
                        var tmbimagefile = tempfloder + "\\" + fileName;

                        if (ImageUtil.IsImageExistes(imagefile))
                        {
                            // 正常显示图片才需要处理缩略图
                            if (string.IsNullOrWhiteSpace(starttxt) && !ImageUtil.IsImageExistes(tmbimagefile))
                            {
                                var bmp = new Bitmap(imagefile);
                                SaveClipImage(bmp, tmbimagefile);
                                bmp.Dispose();
                            }

                            return fileName;
                        }

                        // 本地路径时不进行Http下载
                        if (!(new string(url.Take(0, 4).ToCharArray()).StartsWith("http")))
                        {
                            return string.Empty;
                        }

                        var taskParams = HttpTaskParams.NewGet(url);
                        var httpTask = new HttpTask(taskParams);
                        var bytes = httpTask.ExecuteBytes();
                        if (bytes != null)
                        {
                            using (var ms = new MemoryStream(bytes))
                            {
                                bool issave = false;
                                var bmp = new Bitmap(ms);
                                try
                                {
                                    if (!File.Exists(imagefile))
                                    {
                                        bmp.Save(imagefile);
                                    }
                                    issave = true;
                                }
                                catch { }
                                if (!issave)
                                {
                                    SaveBytesToImage(bytes, imagefile);
                                }

                                SaveClipImage(bmp, tmbimagefile);
                            }
                            return fileName;
                        }
                    }
                }
                catch { }
            }

            return string.Empty;
        }

        // 确认文件格式 .jpg .jpeg or .png 拿到md5 Save 图片
        public static ValidImageFormatEnum GetImageFormat(string url)
        {
            var regJpg = new Regex(@"\.jpg", RegexOptions.IgnoreCase);
            if (regJpg.IsMatch(url))
                return ValidImageFormatEnum.JPG;

            var regPng = new Regex(@"\.png", RegexOptions.IgnoreCase);
            if (regPng.IsMatch(url))
                return ValidImageFormatEnum.PNG;

            var regJpeg = new Regex(@"\.jpeg", RegexOptions.IgnoreCase);
            if (regJpeg.IsMatch(url))
                return ValidImageFormatEnum.JPEG;

            return ValidImageFormatEnum.NONE;
        }

        public static bool IsImageExistes(string fileStr)
        {
            return File.Exists(fileStr);
        }

        public static bool SaveBytesToImage(byte[] bytes, string fileStr)
        {
            try
            {
                lock (fileStr)
                {
                    if (!File.Exists(fileStr))
                    {
                        File.WriteAllBytes(fileStr, bytes);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SaveClipImage(Bitmap bmp, string filename)
        {
            var sw = bmp.Width / 70d;
            var sh = bmp.Height / 70d;
            var sc = sw > sh ? sh : sw;
            var newW = (int)(bmp.Width / sc); var newH = (int)(bmp.Height / sc);
            var tmbimage = bmp.GetThumbnailImage(newW, newH, null, IntPtr.Zero);
            var filepath = filename;
            try
            {
                if (!File.Exists(filepath))
                {
                    //var b = new Bitmap(newW, newH);
                    //var g = Graphics.FromImage(b);
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                    //g.Dispose();
                    //b.Save(filepath);
                    //b.Dispose();
                    tmbimage.Save(filepath);
                }
            }
            catch { }

            tmbimage.Dispose();
        }

        #endregion Methods
    }
}