using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Ji.ImageHelper.Gif
{
    public static class GifHelper
    {
        public static bool ConvertToGif(this List<Bitmap> bmps, string filepath, int delay = 60)
        {
            try
            {
                var ae = new AnimatedGifEncoder();
                ae.Start(filepath);
                ae.SetDelay(delay);
                ae.SetRepeat(0);
                bmps.ForEach(i =>
                {
                    ae.AddFrame(i);
                    i.Dispose();
                });

                ae.Finish();
                return true;
            }
            catch { }
            return false;
        }

        public static bool ConvertToGif(this List<BitmapSource> bmps, string filepath)
        {
            FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate);
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            bmps.ForEach(i =>
            {
                encoder.Frames.Add(BitmapFrame.Create(i));
            });
            encoder.Save(stream);

            return true;
        }
    }
}