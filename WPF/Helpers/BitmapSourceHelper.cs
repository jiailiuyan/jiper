using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Ji.WPFHelper.ImageHelper
{
    public static class BitmapSourceHelper
    {
        //new BitmapImage(new Uri("pack://application:,,,/NoteBookPlugin;component/Images/5.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage GetBitmapImage(string assname, string imagepath)
        {
            return new BitmapImage(new Uri("pack://application:,,,/" + assname + "; component/" + imagepath, UriKind.RelativeOrAbsolute));
        }

        /// <summary> 把 BitmapSource 转换成 Bitmap </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(this BitmapSource bs)
        {
            return ConvertToBitmap(bs, 0, 0, bs.PixelWidth, bs.PixelHeight);
        }

        /// <summary> 把 BitmapSource 转换成指定起始点和宽高的 Bitmap </summary>
        /// <param name="bs">    </param>
        /// <param name="x">     </param>
        /// <param name="y">     </param>
        /// <param name="width"> </param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(this BitmapSource bs, int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var bmpdata = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            bs.CopyPixels(new Int32Rect(x, y, width, height), bmpdata.Scan0, bmpdata.Height * bmpdata.Stride, bmpdata.Stride);
            bmp.UnlockBits(bmpdata);
            return bmp;
        }

        /// <summary> 把 BitmapSource 转换成 byte[] </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(this BitmapSource bs)
        {
            return ConvertToBytes(bs, 0, 0, (int)bs.Width, (int)bs.Height);
        }

        /// <summary> 把 BitmapSource 转换成指定起始点和宽高的 byte[] </summary>
        /// <param name="bs">    </param>
        /// <param name="x">     </param>
        /// <param name="y">     </param>
        /// <param name="width"> </param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(this BitmapSource bs, int x, int y, int width, int height)
        {
            var rect = new Int32Rect(x, y, width, height);
            var stride = bs.Format.BitsPerPixel * rect.Width / 8;
            byte[] data = new byte[rect.Height * stride];
            bs.CopyPixels(rect, data, stride, 0);
            return data;
        }

        public static BitmapSource ClipBitmapSource(this BitmapSource bs, int x, int y, int width, int height)
        {
            var rect = new Int32Rect(x, y, width, height);
            var stride = bs.Format.BitsPerPixel * rect.Width / 8;
            byte[] data = new byte[rect.Height * stride];
            bs.CopyPixels(rect, data, stride, 0);
            return BitmapSource.Create(width, height, 0, 0, System.Windows.Media.PixelFormats.Bgra32, null, data, stride);
        }

        /// <summary> 性能超差，不建议使用 </summary>
        /// <param name="data">  </param>
        /// <param name="width"> </param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(this byte[] data, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    int index = h * width * 4 + w * 4;

                    if (index + 3 < data.Length)
                    {
                        int B = data[index];
                        int G = data[index + 1];
                        int R = data[index + 2];
                        int A = data[index + 3];

                        bmp.SetPixel(w, h, System.Drawing.Color.FromArgb(A, R, G, B));
                    }
                }
            }
            return bmp;
        }

        /// <summary> 性能超差，不建议使用 </summary>
        /// <param name="data">  </param>
        /// <param name="width"> </param>
        /// <param name="height"></param>
        /// <param name="isrgb"> </param>
        /// <returns></returns>
        public static Bitmap ConvertToBitmap(this byte[] data, int width, int height, bool isrgb)
        {
            var bmp = new Bitmap(width, height);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    int index = h * width * 3 + w * 3;

                    if (index + 3 < data.Length)
                    {
                        int R = data[index];
                        int G = data[index + 1];
                        int B = data[index + 2];

                        bmp.SetPixel(w, h, System.Drawing.Color.FromArgb(255, R, G, B));
                    }
                }
            }
            return bmp;
        }

        public static BitmapSource ConvertToBitmapSource(this Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}