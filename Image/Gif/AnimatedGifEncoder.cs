using System;
using System.Drawing;
using System.IO;

namespace Ji.ImageHelper.Gif
{
    public class AnimatedGifEncoder
    {
        protected int width;
        protected int height;
        protected Color transparent = Color.Empty;
        protected int transIndex;
        protected int repeat = -1;
        protected int delay;
        protected bool started;
        protected FileStream fs;
        protected Image image;
        protected byte[] pixels;
        protected byte[] indexedPixels;
        protected int colorDepth;
        protected byte[] colorTab;
        protected bool[] usedEntry = new bool[256];
        protected int palSize = 7;
        protected int dispose = -1;
        protected bool closeStream;
        protected bool firstFrame = true;
        protected bool sizeSet;
        protected int sample = 10;

        public void SetDelay(int ms)
        {
            this.delay = (int)Math.Round((double)((float)ms / 10f));
        }

        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                this.dispose = code;
            }
        }

        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                this.repeat = iter;
            }
        }

        public void SetTransparent(Color c)
        {
            this.transparent = c;
        }

        public bool AddFrame(Image im)
        {
            if (im == null || !this.started)
            {
                return false;
            }
            bool result = true;
            try
            {
                if (!this.sizeSet)
                {
                    this.SetSize(im.Width, im.Height);
                }
                this.image = im;
                this.GetImagePixels();
                this.AnalyzePixels();
                if (this.firstFrame)
                {
                    this.WriteLSD();
                    this.WritePalette();
                    if (this.repeat >= 0)
                    {
                        this.WriteNetscapeExt();
                    }
                }
                this.WriteGraphicCtrlExt();
                this.WriteImageDesc();
                if (!this.firstFrame)
                {
                    this.WritePalette();
                }
                this.WritePixels();
                this.firstFrame = false;
            }
            catch (IOException)
            {
                result = false;
            }
            return result;
        }

        public bool Finish()
        {
            if (!this.started)
            {
                return false;
            }
            bool result = true;
            this.started = false;
            try
            {
                this.fs.WriteByte(59);
                this.fs.Flush();
                if (this.closeStream)
                {
                    this.fs.Close();
                }
            }
            catch (IOException)
            {
                result = false;
            }
            this.transIndex = 0;
            this.fs = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.closeStream = false;
            this.firstFrame = true;
            return result;
        }

        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                this.delay = (int)Math.Round((double)(100f / fps));
            }
        }

        public void SetQuality(int quality)
        {
            if (quality < 1)
            {
                quality = 1;
            }
            this.sample = quality;
        }

        public void SetSize(int w, int h)
        {
            if (this.started && !this.firstFrame)
            {
                return;
            }
            this.width = w;
            this.height = h;
            if (this.width < 1)
            {
                this.width = 320;
            }
            if (this.height < 1)
            {
                this.height = 240;
            }
            this.sizeSet = true;
        }

        public bool Start(FileStream os)
        {
            if (os == null)
            {
                return false;
            }
            bool flag = true;
            this.closeStream = false;
            this.fs = os;
            try
            {
                this.WriteString("GIF89a");
            }
            catch (IOException)
            {
                flag = false;
            }
            return this.started = flag;
        }

        public bool Start(string file)
        {
            bool flag = true;
            try
            {
                this.fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                flag = this.Start(this.fs);
                this.closeStream = true;
            }
            catch (IOException)
            {
                flag = false;
            }
            return this.started = flag;
        }

        protected void AnalyzePixels()
        {
            int num = this.pixels.Length;
            int num2 = num / 3;
            this.indexedPixels = new byte[num2];
            NeuQuant neuQuant = new NeuQuant(this.pixels, num, this.sample);
            this.colorTab = neuQuant.Process();
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int num4 = neuQuant.Map((int)(this.pixels[num3++] & 255), (int)(this.pixels[num3++] & 255), (int)(this.pixels[num3++] & 255));
                this.usedEntry[num4] = true;
                this.indexedPixels[i] = (byte)num4;
            }
            this.pixels = null;
            this.colorDepth = 8;
            this.palSize = 7;
            if (this.transparent != Color.Empty)
            {
                this.transIndex = this.FindClosest(this.transparent);
            }
        }

        protected int FindClosest(Color c)
        {
            if (this.colorTab == null)
            {
                return -1;
            }
            int r = (int)c.R;
            int g = (int)c.G;
            int b = (int)c.B;
            int result = 0;
            int num = 16777216;
            int num2 = this.colorTab.Length;
            for (int i = 0; i < num2; i++)
            {
                int num3 = r - (int)(this.colorTab[i++] & 255);
                int num4 = g - (int)(this.colorTab[i++] & 255);
                int num5 = b - (int)(this.colorTab[i] & 255);
                int num6 = num3 * num3 + num4 * num4 + num5 * num5;
                int num7 = i / 3;
                if (this.usedEntry[num7] && num6 < num)
                {
                    num = num6;
                    result = num7;
                }
            }
            return result;
        }

        protected void GetImagePixels()
        {
            int num = this.image.Width;
            int num2 = this.image.Height;
            if (num != this.width || num2 != this.height)
            {
                Image image = new Bitmap(this.width, this.height);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(this.image, 0, 0);
                this.image = image;
                graphics.Dispose();
            }
            this.pixels = new byte[3 * this.image.Width * this.image.Height];
            int num3 = 0;
            Bitmap bitmap = new Bitmap(this.image);
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    this.pixels[num3] = pixel.R;
                    num3++;
                    this.pixels[num3] = pixel.G;
                    num3++;
                    this.pixels[num3] = pixel.B;
                    num3++;
                }
            }
        }

        protected void WriteGraphicCtrlExt()
        {
            this.fs.WriteByte(33);
            this.fs.WriteByte(249);
            this.fs.WriteByte(4);
            int num;
            int num2;
            if (this.transparent == Color.Empty)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                num = 1;
                num2 = 2;
            }
            if (this.dispose >= 0)
            {
                num2 = (this.dispose & 7);
            }
            num2 <<= 2;
            this.fs.WriteByte(Convert.ToByte(num2 | num));
            this.WriteShort(this.delay);
            this.fs.WriteByte(Convert.ToByte(this.transIndex));
            this.fs.WriteByte(0);
        }

        protected void WriteImageDesc()
        {
            this.fs.WriteByte(44);
            this.WriteShort(0);
            this.WriteShort(0);
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            if (this.firstFrame)
            {
                this.fs.WriteByte(0);
                return;
            }
            this.fs.WriteByte(Convert.ToByte(128 | this.palSize));
        }

        protected void WriteLSD()
        {
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            this.fs.WriteByte(Convert.ToByte(240 | this.palSize));
            this.fs.WriteByte(0);
            this.fs.WriteByte(0);
        }

        protected void WriteNetscapeExt()
        {
            this.fs.WriteByte(33);
            this.fs.WriteByte(255);
            this.fs.WriteByte(11);
            this.WriteString("NETSCAPE2.0");
            this.fs.WriteByte(3);
            this.fs.WriteByte(1);
            this.WriteShort(this.repeat);
            this.fs.WriteByte(0);
        }

        protected void WritePalette()
        {
            this.fs.Write(this.colorTab, 0, this.colorTab.Length);
            int num = 768 - this.colorTab.Length;
            for (int i = 0; i < num; i++)
            {
                this.fs.WriteByte(0);
            }
        }

        protected void WritePixels()
        {
            LZWEncoder lZWEncoder = new LZWEncoder(this.width, this.height, this.indexedPixels, this.colorDepth);
            lZWEncoder.Encode(this.fs);
        }

        protected void WriteShort(int value)
        {
            this.fs.WriteByte(Convert.ToByte(value & 255));
            this.fs.WriteByte(Convert.ToByte(value >> 8 & 255));
        }

        protected void WriteString(string s)
        {
            char[] array = s.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                this.fs.WriteByte((byte)array[i]);
            }
        }
    }
}