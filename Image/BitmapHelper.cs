/* 迹I柳燕
 *
 * FileName:   BitmapHelper.cs
 * Version:    1.0
 * Date:       2014.03.18
 * Author:     Ji
 *
 *========================================
 * @namespace  Ji.ImageHelper
 * @class      BitmapHelper
 * @extends
 *
 *             对于 Bitmap 的转换函数
 *
 *========================================

 *
 *
 *
 *
 */

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ji.ImageHelper
{
    public static class BitmapHelper
    {
        /// <summary> 从 ImageSource 转换到 Bitmap </summary>
        /// <param name="imageSource"> 将要转换的 ImageSource </param>
        /// <returns> 返回转换后的 Bitmap </returns>
        public static Bitmap ConvertToBitmap(this ImageSource imageSource)
        {
            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource != null)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                using (MemoryStream stm = new MemoryStream())
                {
                    encoder.Save(stm);
                    return new Bitmap(stm);
                }
            }
            return null;
        }

        /// <summary> 从文件中读取 ImageSource </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>读取到的 ImageSource </returns>
        public static Bitmap GetBitmap(this string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var fs = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var binReader = new BinaryReader(fs);
                        byte[] bytes = binReader.ReadBytes((int)fs.Length);
                        return new Bitmap(new MemoryStream(bytes));
                    }
                }
            }
            catch { }
            return null;
        }

        public const string ColorLock = "Colors";
        private static System.Drawing.Color[] colors = null;

        private static System.Drawing.Color[] Colors
        {
            get
            {
                if (colors == null)
                {
                    lock (ColorLock)
                    {
                        if (colors == null)
                        {
                            colors = new System.Drawing.Color[256];
                            for (int i = 0; i < 256; i++)
                            {
                                System.Drawing.Color tmp = System.Drawing.Color.FromArgb(255, i, i, i);
                                colors[i] = System.Drawing.Color.FromArgb(255, i, i, i);
                            }
                        }
                    }
                }
                return colors;
            }
        }

        private static ColorPalette ColorPalette = null;

        public static void SetFormat8ColorsPalette(this Bitmap bitmap)
        {
            if (ColorPalette == null)
            {
                ColorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    ColorPalette.Entries[i] = Colors[i];
                }
            }

            bitmap.Palette = ColorPalette;
        }

        public static Bitmap ConvertToFormat8(this Bitmap bitmap)
        {
            if (bitmap != null)
            {
                int bitskip = 1;
                switch (bitmap.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format8bppIndexed: return bitmap;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb: bitskip = 3; break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb: bitskip = 4; break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb: bitskip = 4; break;
                    default: return null;
                }

                int w = bitmap.Width;
                int h = bitmap.Height;

                if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                {
                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                    var f8Bitmap = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    f8Bitmap.SetFormat8ColorsPalette();

                    BitmapData f8BitmapData = f8Bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, f8Bitmap.PixelFormat);

                    unsafe
                    {
                        //byte* olddata = (byte*)(bitmapData.Scan0).ToPointer();
                        //byte* newdata = (byte*)(f8BitmapData.Scan0).ToPointer();

                        byte[] data = new byte[w * h];

                        for (int y = 0; y < bitmapData.Height; y++)
                        {
                            int viewcount = 0;
                            for (int x = 0; viewcount < bitmapData.Width; x += bitskip, viewcount++)
                            {
                                *((byte*)(f8BitmapData.Scan0) + viewcount + y * f8BitmapData.Stride) = *((byte*)(bitmapData.Scan0 + (x) + y * bitmapData.Stride));
                            }
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    f8Bitmap.UnlockBits(f8BitmapData);

                    bitmap.Dispose();

                    return f8Bitmap;
                }
            }
            return null;
        }
    }
}