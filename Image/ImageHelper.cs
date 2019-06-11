using System.Drawing;
using System.IO;

namespace Ji.ImageHelper
{
    public static class ImageHelper
    {
        public static Size GetImageSize(this FileInfo fileinfo)
        {
            if (fileinfo.Exists)
            {
                using (var bmp = new Bitmap(fileinfo.FullName))
                {
                    if (bmp != null)
                    {
                        return new Size(bmp.Size.Width, bmp.Size.Height);
                        //ImageBrush brush = new ImageBrush(Texture);
                        //double width = 200;
                        //double height = 200;

                        //if (Texture.DpiX != 0 && Texture.DpiX != 0 && Texture.DpiX < 50 && Texture.DpiX < 50)
                        //{
                        //    width = (int)(Texture.Height * Texture.DpiX / 96);
                        //    height = (int)(Texture.Width * Texture.DpiY / 96);
                        //}

                        //else
                        //{
                        //    var image = brush.ImageSource as BitmapSource;
                        //    ImageRect = new Rect() { Width = image.PixelWidth, Height = image.PixelHeight };
                        //    width = image.PixelWidth;
                        //    height = image.PixelHeight;
                        //}
                    }
                }
            }
            return default(Size);
        }
    }
}