// ***********************************************************************
// 程序集         : Yuanbo.ChssClient.Common
// 作者           : 刘晓青
// 创建日期       : 12-26-2017
//
// 最后编辑人员   : 刘晓青
// 最后编辑日期   : 12-26-2017
// ***********************************************************************
// <copyright file="BitmapImageHelper.cs" company="KingSoft">
//     Copyright © KingSoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace System
{
    /// <summary>
    /// Class BitmapImageHelper.
    /// </summary>
    public static class BitmapImageHelper
    {
        #region Public 方法

        /// <summary>
        /// Bitmaps the image to bitmap.
        /// </summary>
        /// <param name="bitmapImage">The bitmap image.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap BitmapImageToBitmap(this BitmapImage bitmapImage)
        {
            Stream stream = bitmapImage.StreamSource;
            return new Bitmap(stream);
        }

        /// <summary>
        /// Bitmaps to bitmap image.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns>BitmapImage.</returns>
        public static BitmapImage BitmapToBitmapImage(this Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();
            return bitmapImage;
        }

        /// <summary>
        /// bitmap 转光栅图
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="left">The left.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] BitmapToMonochrome(Bitmap bitmap, int left)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            byte[] imgbuf = new byte[(width / 8 + left + 4) * height];
            byte[] bitbuf = new byte[width / 8];
            int[] p = new int[8];
            int s = 0;

            for (int y = 0; y < height; ++y)
            {
                int n;
                for (n = 0; n < width / 8; ++n)
                {
                    int value;
                    for (value = 0; value < 8; ++value)
                    {
                        Color c = bitmap.GetPixel(n * 8 + value, y);
                        if (c.A == 0 && c.B == 0 && c.G == 0 && c.R == 0)
                        {
                            p[value] = 0;
                        }
                        else
                        {
                            p[value] = 1;
                        }
                    }

                    value = p[0] * 128 + p[1] * 64 + p[2] * 32 + p[3] * 16 + p[4] * 8 + p[5] * 4 + p[6] * 2 + p[7];
                    bitbuf[n] = (byte)value;
                }

                if (y != 0)
                {
                    ++s;
                    imgbuf[s] = 22;
                }
                else
                {
                    imgbuf[s] = 22;
                }

                ++s;
                imgbuf[s] = (byte)(width / 8 + left);

                for (n = 0; n < left; ++n)
                {
                    ++s;
                    imgbuf[s] = 0;
                }

                for (n = 0; n < width / 8; ++n)
                {
                    ++s;
                    imgbuf[s] = bitbuf[n];
                }

                ++s;
                imgbuf[s] = 21;
                ++s;
                imgbuf[s] = 1;
            }
            return imgbuf;
        }

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap GetBitmap(this string url)
        {
            try
            {
                string userHeadPic = url;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(userHeadPic);
                WebResponse response = request.GetResponse();
                Image img = Image.FromStream(response.GetResponseStream());
                return new Bitmap(img);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Public 方法
    }
}