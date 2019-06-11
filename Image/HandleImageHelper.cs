using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Ji.WPFHelper.ImageHelper;

namespace Ji.ImageHelper
{
    public static class HandleImageHelper
    {
        public static BitmapSource GetView(this IntPtr handle)
        {
            var rect = new NativeHelper.NativeHelper.RECT();
            var result = NativeHelper.NativeHelper.GetWindowRect(handle, ref rect);
            if (result != IntPtr.Zero)
            {
                Rectangle r = new Rectangle(new System.Drawing.Point(rect.Left, rect.Top), new Size(rect.Right - rect.Left, rect.Bottom - rect.Top));
                using (var img = new Bitmap(r.Width, r.Height))
                {
                    using (var g = Graphics.FromImage(img))
                    {
                        try
                        {
                            g.CopyFromScreen(r.Location, System.Drawing.Point.Empty, r.Size, CopyPixelOperation.SourceCopy);
                            g.DrawImage(img, 0, 0, img.Width, img.Height);
                            return img.ConvertToBitmapSource();
                        }
                        catch { }
                    }
                }
            }

            return null;
        }

        public static Bitmap GetBitmapView(this IntPtr handle)
        {
            try
            {
                var rect = new NativeHelper.NativeHelper.RECT();
                var result = NativeHelper.NativeHelper.GetWindowRect(handle, ref rect);
                if (result != IntPtr.Zero)
                {
                    Rectangle r = new Rectangle(new System.Drawing.Point(rect.Left, rect.Top), new Size(rect.Right - rect.Left, rect.Bottom - rect.Top));
                    var img = new Bitmap(r.Width, r.Height);
                    {
                        using (var g = Graphics.FromImage(img))
                        {
                            try
                            {
                                g.CopyFromScreen(r.Location, System.Drawing.Point.Empty, r.Size, CopyPixelOperation.SourceCopy);
                                g.DrawImage(img, 0, 0, img.Width, img.Height);
                                return img;
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }
}