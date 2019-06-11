using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Ji.ImageHelper.Gif
{
    public class GifDecoder
    {
        public class GifFrame
        {
            public Image image;
            public int delay;

            public GifFrame(Image im, int del)
            {
                this.image = im;
                this.delay = del;
            }
        }

        public static readonly int STATUS_OK = 0;
        public static readonly int STATUS_FORMAT_ERROR = 1;
        public static readonly int STATUS_OPEN_ERROR = 2;
        protected Stream inStream;
        protected int status;
        protected int width;
        protected int height;
        protected bool gctFlag;
        protected int gctSize;
        protected int loopCount = 1;
        protected int[] gct;
        protected int[] lct;
        protected int[] act;
        protected int bgIndex;
        protected int bgColor;
        protected int lastBgColor;
        protected int pixelAspect;
        protected bool lctFlag;
        protected bool interlace;
        protected int lctSize;
        protected int ix;
        protected int iy;
        protected int iw;
        protected int ih;
        protected Rectangle lastRect;
        protected Image image;
        protected Bitmap bitmap;
        protected Image lastImage;
        protected byte[] block = new byte[256];
        protected int blockSize;
        protected int dispose;
        protected int lastDispose;
        protected bool transparency;
        protected int delay;
        protected int transIndex;
        protected static readonly int MaxStackSize = 4096;
        protected short[] prefix;
        protected byte[] suffix;
        protected byte[] pixelStack;
        protected byte[] pixels;
        protected ArrayList frames;
        protected int frameCount;

        public int GetDelay(int n)
        {
            this.delay = -1;
            if (n >= 0 && n < this.frameCount)
            {
                this.delay = ((GifDecoder.GifFrame)this.frames[n]).delay;
            }
            return this.delay;
        }

        public int GetFrameCount()
        {
            return this.frameCount;
        }

        public Image GetImage()
        {
            return this.GetFrame(0);
        }

        public int GetLoopCount()
        {
            return this.loopCount;
        }

        private int[] GetPixels(Bitmap bitmap)
        {
            int[] array = new int[3 * this.image.Width * this.image.Height];
            int num = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    array[num] = (int)pixel.R;
                    num++;
                    array[num] = (int)pixel.G;
                    num++;
                    array[num] = (int)pixel.B;
                    num++;
                }
            }
            return array;
        }

        private void SetPixels(int[] pixels)
        {
            int num = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color color = Color.FromArgb(pixels[num++]);
                    this.bitmap.SetPixel(j, i, color);
                }
            }
        }

        protected void SetPixels()
        {
            int[] array = this.GetPixels(this.bitmap);
            if (this.lastDispose > 0)
            {
                if (this.lastDispose == 3)
                {
                    int num = this.frameCount - 2;
                    if (num > 0)
                    {
                        this.lastImage = this.GetFrame(num - 1);
                    }
                    else
                    {
                        this.lastImage = null;
                    }
                }
                if (this.lastImage != null)
                {
                    int[] sourceArray = this.GetPixels(new Bitmap(this.lastImage));
                    Array.Copy(sourceArray, 0, array, 0, this.width * this.height);
                    if (this.lastDispose == 2)
                    {
                        Graphics graphics = Graphics.FromImage(this.image);
                        Color color = Color.Empty;
                        if (this.transparency)
                        {
                            color = Color.FromArgb(0, 0, 0, 0);
                        }
                        else
                        {
                            color = Color.FromArgb(this.lastBgColor);
                        }
                        Brush brush = new SolidBrush(color);
                        graphics.FillRectangle(brush, this.lastRect);
                        brush.Dispose();
                        graphics.Dispose();
                    }
                }
            }
            int num2 = 1;
            int num3 = 8;
            int num4 = 0;
            for (int i = 0; i < this.ih; i++)
            {
                int num5 = i;
                if (this.interlace)
                {
                    if (num4 >= this.ih)
                    {
                        num2++;
                        switch (num2)
                        {
                            case 2:
                                {
                                    num4 = 4;
                                    break;
                                }
                            case 3:
                                {
                                    num4 = 2;
                                    num3 = 4;
                                    break;
                                }
                            case 4:
                                {
                                    num4 = 1;
                                    num3 = 2;
                                    break;
                                }
                        }
                    }
                    num5 = num4;
                    num4 += num3;
                }
                num5 += this.iy;
                if (num5 < this.height)
                {
                    int num6 = num5 * this.width;
                    int j = num6 + this.ix;
                    int num7 = j + this.iw;
                    if (num6 + this.width < num7)
                    {
                        num7 = num6 + this.width;
                    }
                    int num8 = i * this.iw;
                    while (j < num7)
                    {
                        int num9 = (int)(this.pixels[num8++] & 255);
                        int num10 = this.act[num9];
                        if (num10 != 0)
                        {
                            array[j] = num10;
                        }
                        j++;
                    }
                }
            }
            this.SetPixels(array);
        }

        public Image GetFrame(int n)
        {
            Image result = null;
            if (n >= 0 && n < this.frameCount)
            {
                result = ((GifDecoder.GifFrame)this.frames[n]).image;
            }
            return result;
        }

        public Size GetFrameSize()
        {
            return new Size(this.width, this.height);
        }

        public int Read(Stream inStream)
        {
            this.Init();
            if (inStream != null)
            {
                this.inStream = inStream;
                this.ReadHeader();
                if (!this.Error())
                {
                    this.ReadContents();
                    if (this.frameCount < 0)
                    {
                        this.status = GifDecoder.STATUS_FORMAT_ERROR;
                    }
                }
                inStream.Close();
            }
            else
            {
                this.status = GifDecoder.STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        public int Read(string name)
        {
            this.status = GifDecoder.STATUS_OK;
            try
            {
                name = name.Trim().ToLower();
                this.status = this.Read(new FileInfo(name).OpenRead());
            }
            catch (IOException)
            {
                this.status = GifDecoder.STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        protected void DecodeImageData()
        {
            int num = -1;
            int num2 = this.iw * this.ih;
            if (this.pixels == null || this.pixels.Length < num2)
            {
                this.pixels = new byte[num2];
            }
            if (this.prefix == null)
            {
                this.prefix = new short[GifDecoder.MaxStackSize];
            }
            if (this.suffix == null)
            {
                this.suffix = new byte[GifDecoder.MaxStackSize];
            }
            if (this.pixelStack == null)
            {
                this.pixelStack = new byte[GifDecoder.MaxStackSize + 1];
            }
            int num3 = this.Read();
            int num4 = 1 << num3;
            int num5 = num4 + 1;
            int num6 = num4 + 2;
            int num7 = num;
            int num8 = num3 + 1;
            int num9 = (1 << num8) - 1;
            for (int i = 0; i < num4; i++)
            {
                this.prefix[i] = 0;
                this.suffix[i] = (byte)i;
            }
            int num16;
            int num15;
            int num14;
            int num13;
            int num12;
            int num11;
            int num10 = num11 = (num12 = (num13 = (num14 = (num15 = (num16 = 0)))));
            for (int j = 0; j < num2; j++)
            {
                if (num14 == 0)
                {
                    if (num10 < num8)
                    {
                        if (num12 == 0)
                        {
                            num12 = this.ReadBlock();
                            if (num12 <= 0)
                            {
                                break;
                            }
                            num16 = 0;
                        }
                        num11 += (int)(this.block[num16] & 255) << num10;
                        num10 += 8;
                        num16++;
                        num12--;
                        continue;
                    }
                    int i = num11 & num9;
                    num11 >>= num8;
                    num10 -= num8;
                    if (i > num6 || i == num5)
                    {
                        break;
                    }
                    if (i == num4)
                    {
                        num8 = num3 + 1;
                        num9 = (1 << num8) - 1;
                        num6 = num4 + 2;
                        num7 = num;
                        continue;
                    }
                    if (num7 == num)
                    {
                        this.pixelStack[num14++] = this.suffix[i];
                        num7 = i;
                        num13 = i;
                        continue;
                    }
                    int num17 = i;
                    if (i == num6)
                    {
                        this.pixelStack[num14++] = (byte)num13;
                        i = num7;
                    }
                    while (i > num4)
                    {
                        this.pixelStack[num14++] = this.suffix[i];
                        i = (int)this.prefix[i];
                    }
                    num13 = (int)(this.suffix[i] & 255);
                    if (num6 >= GifDecoder.MaxStackSize)
                    {
                        break;
                    }
                    this.pixelStack[num14++] = (byte)num13;
                    this.prefix[num6] = (short)num7;
                    this.suffix[num6] = (byte)num13;
                    num6++;
                    if ((num6 & num9) == 0 && num6 < GifDecoder.MaxStackSize)
                    {
                        num8++;
                        num9 += num6;
                    }
                    num7 = num17;
                }
                num14--;
                this.pixels[num15++] = this.pixelStack[num14];
            }
            for (int j = num15; j < num2; j++)
            {
                this.pixels[j] = 0;
            }
        }

        protected bool Error()
        {
            return this.status != GifDecoder.STATUS_OK;
        }

        protected void Init()
        {
            this.status = GifDecoder.STATUS_OK;
            this.frameCount = 0;
            this.frames = new ArrayList();
            this.gct = null;
            this.lct = null;
        }

        protected int Read()
        {
            int result = 0;
            try
            {
                result = this.inStream.ReadByte();
            }
            catch (IOException)
            {
                this.status = GifDecoder.STATUS_FORMAT_ERROR;
            }
            return result;
        }

        protected int ReadBlock()
        {
            this.blockSize = this.Read();
            int i = 0;
            if (this.blockSize > 0)
            {
                try
                {
                    while (i < this.blockSize)
                    {
                        int num = this.inStream.Read(this.block, i, this.blockSize - i);
                        if (num == -1)
                        {
                            break;
                        }
                        i += num;
                    }
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                if (i < this.blockSize)
                {
                    this.status = GifDecoder.STATUS_FORMAT_ERROR;
                }
            }
            return i;
        }

        protected int[] ReadColorTable(int ncolors)
        {
            int num = 3 * ncolors;
            int[] array = null;
            byte[] array2 = new byte[num];
            int num2 = 0;
            try
            {
                num2 = this.inStream.Read(array2, 0, array2.Length);
            }
            catch (IOException)
            {
            }
            if (num2 < num)
            {
                this.status = GifDecoder.STATUS_FORMAT_ERROR;
            }
            else
            {
                array = new int[256];
                int i = 0;
                int num3 = 0;
                while (i < ncolors)
                {
                    int num4 = (int)(array2[num3++] & 255);
                    int num5 = (int)(array2[num3++] & 255);
                    int num6 = (int)(array2[num3++] & 255);
                    array[i++] = (int)((long)-16777216 | ((long)((long)num4 << 16)) | ((long)((long)num5 << 8)) | ((long)num6));
                }
            }
            return array;
        }

        protected void ReadContents()
        {
            bool flag = false;
            while (!flag && !this.Error())
            {
                int num = this.Read();
                int num2 = num;
                if (num2 <= 33)
                {
                    if (num2 == 0)
                    {
                        continue;
                    }
                    if (num2 == 33)
                    {
                        num = this.Read();
                        int num3 = num;
                        if (num3 == 249)
                        {
                            this.ReadGraphicControlExt();
                            continue;
                        }
                        if (num3 != 255)
                        {
                            this.Skip();
                            continue;
                        }
                        this.ReadBlock();
                        string text = "";
                        for (int i = 0; i < 11; i++)
                        {
                            text += (char)this.block[i];
                        }
                        if (text.Equals("NETSCAPE2.0"))
                        {
                            this.ReadNetscapeExt();
                            continue;
                        }
                        this.Skip();
                        continue;
                    }
                }
                else
                {
                    if (num2 == 44)
                    {
                        this.ReadImage();
                        continue;
                    }
                    if (num2 == 59)
                    {
                        flag = true;
                        continue;
                    }
                }
                this.status = GifDecoder.STATUS_FORMAT_ERROR;
            }
        }

        protected void ReadGraphicControlExt()
        {
            this.Read();
            int num = this.Read();
            this.dispose = (num & 28) >> 2;
            if (this.dispose == 0)
            {
                this.dispose = 1;
            }
            this.transparency = ((num & 1) != 0);
            this.delay = this.ReadShort() * 10;
            this.transIndex = this.Read();
            this.Read();
        }

        protected void ReadHeader()
        {
            string text = "";
            for (int i = 0; i < 6; i++)
            {
                text += (char)this.Read();
            }
            if (!text.StartsWith("GIF"))
            {
                this.status = GifDecoder.STATUS_FORMAT_ERROR;
                return;
            }
            this.ReadLSD();
            if (this.gctFlag && !this.Error())
            {
                this.gct = this.ReadColorTable(this.gctSize);
                this.bgColor = this.gct[this.bgIndex];
            }
        }

        protected void ReadImage()
        {
            this.ix = this.ReadShort();
            this.iy = this.ReadShort();
            this.iw = this.ReadShort();
            this.ih = this.ReadShort();
            int num = this.Read();
            this.lctFlag = ((num & 128) != 0);
            this.interlace = ((num & 64) != 0);
            this.lctSize = 2 << (num & 7);
            if (this.lctFlag)
            {
                this.lct = this.ReadColorTable(this.lctSize);
                this.act = this.lct;
            }
            else
            {
                this.act = this.gct;
                if (this.bgIndex == this.transIndex)
                {
                    this.bgColor = 0;
                }
            }
            int num2 = 0;
            if (this.transparency)
            {
                num2 = this.act[this.transIndex];
                this.act[this.transIndex] = 0;
            }
            if (this.act == null)
            {
                this.status = GifDecoder.STATUS_FORMAT_ERROR;
            }
            if (this.Error())
            {
                return;
            }
            this.DecodeImageData();
            this.Skip();
            if (this.Error())
            {
                return;
            }
            this.frameCount++;
            this.bitmap = new Bitmap(this.width, this.height);
            this.image = this.bitmap;
            this.SetPixels();
            this.frames.Add(new GifDecoder.GifFrame(this.bitmap, this.delay));
            if (this.transparency)
            {
                this.act[this.transIndex] = num2;
            }
            this.ResetFrame();
        }

        protected void ReadLSD()
        {
            this.width = this.ReadShort();
            this.height = this.ReadShort();
            int num = this.Read();
            this.gctFlag = ((num & 128) != 0);
            this.gctSize = 2 << (num & 7);
            this.bgIndex = this.Read();
            this.pixelAspect = this.Read();
        }

        protected void ReadNetscapeExt()
        {
            do
            {
                this.ReadBlock();
                if (this.block[0] == 1)
                {
                    int num = (int)(this.block[1] & 255);
                    int num2 = (int)(this.block[2] & 255);
                    this.loopCount = (num2 << 8 | num);
                }
            }
            while (this.blockSize > 0 && !this.Error());
        }

        protected int ReadShort()
        {
            return this.Read() | this.Read() << 8;
        }

        protected void ResetFrame()
        {
            this.lastDispose = this.dispose;
            this.lastRect = new Rectangle(this.ix, this.iy, this.iw, this.ih);
            this.lastImage = this.image;
            this.lastBgColor = this.bgColor;
            this.lct = null;
        }

        protected void Skip()
        {
            do
            {
                this.ReadBlock();
            }
            while (this.blockSize > 0 && !this.Error());
        }
    }
}