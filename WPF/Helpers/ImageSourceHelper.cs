/* 迹I柳燕
 *
 * FileName:   ImageSourceHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.WPFHelper.ImageHelper
 * @class      ImageSourceHelper
 * @extends
 *
 *             WPF 扩展
 *             对于JilyHelperImageSource的处理
 *
 *========================================

 *
 *
 *
 *
 */

using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ji.WPFHelper.ImageHelper
{
    public static class ImageSourceHelper
    {
        /// <summary> 转换 Icon 到 ImageSource </summary>
        /// <param name="icon"> 将要转换的 Icon </param>
        /// <returns> 转换后的 ImageSource </returns>
        public static ImageSource ConvertToImageSource(this Icon icon)
        {
            return icon != null ? Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) : null;
        }

        public static ImageSource ConvertImageSource(Stream stream)
        {
            var imageBrush = new ImageBrush();
            var imageSourceConverter = new ImageSourceConverter();
            if (stream != null)
            {
                stream.Position = 0;
                return (ImageSource)imageSourceConverter.ConvertFrom(stream);
            }
            return null;
        }

        /// <summary> 转换 Bitmap 到 ImageSource Imaging.CreateBitmapSourceFromHBitmap 具有相同的功能，此项是通过数据流进行转换 </summary>
        /// <param name="bitmap"> 将要转换的 Bitmap </param>
        /// <returns> 转换后的 ImageSource </returns>
        public static ImageSource ConvertImageSource(this Bitmap bitmap)
        {
            var stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            var imageBrush = new ImageBrush();
            var imageSourceConverter = new ImageSourceConverter();
            stream.Position = 0;
            return (ImageSource)imageSourceConverter.ConvertFrom(stream);
        }

        /// <summary> 从 FrameworkElement 中截取 ImageSource 当前 FrameworkElement 需已经渲染 </summary>
        /// <param name="frameworkElement"> 将要截取图像的 FrameworkElement </param>
        /// <returns> 截取后的 ImageSource </returns>
        public static ImageSource GetImageSource(this FrameworkElement frameworkElement)
        {
            int Height = (int)frameworkElement.ActualHeight;
            int Width = (int)frameworkElement.ActualWidth;

            if (Height == 0 || Width == 0)
            {
                return null;
            }

            var renderBMP = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
            renderBMP.Render(frameworkElement);
            return renderBMP as ImageSource;
        }

        public static ImageSource GetImageSource(this byte[] bytes)
        {
            var bitmap = new BitmapImage();
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        /// <summary> 从文件中读取 ImageSource </summary>
        /// <param name="filePath"> 文件路径 </param>
        /// <returns> 读取到的 ImageSource </returns>
        public static ImageSource GetImageSource(this string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var fs = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var binReader = new BinaryReader(fs);
                        byte[] bytes = binReader.ReadBytes((int)fs.Length);
                        var bitmap = new BitmapImage();
                        bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(bytes);
                        bitmap.EndInit();

                        return bitmap;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}