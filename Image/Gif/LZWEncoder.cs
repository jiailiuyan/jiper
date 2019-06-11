using System;
using System.IO;

namespace Ji.ImageHelper.Gif
{
    public class LZWEncoder
    {
        private static readonly int EOF = -1;
        private int imgW;
        private int imgH;
        private byte[] pixAry;
        private int initCodeSize;
        private int remaining;
        private int curPixel;
        private static readonly int BITS = 12;
        private static readonly int HSIZE = 5003;
        private int n_bits;
        private int maxbits = LZWEncoder.BITS;
        private int maxcode;
        private int maxmaxcode = 1 << LZWEncoder.BITS;
        private int[] htab = new int[LZWEncoder.HSIZE];
        private int[] codetab = new int[LZWEncoder.HSIZE];
        private int hsize = LZWEncoder.HSIZE;
        private int free_ent;
        private bool clear_flg;
        private int g_init_bits;
        private int ClearCode;
        private int EOFCode;
        private int cur_accum;
        private int cur_bits;

        private int[] masks = new int[]
        {
            0,
            1,
            3,
            7,
            15,
            31,
            63,
            127,
            255,
            511,
            1023,
            2047,
            4095,
            8191,
            16383,
            32767,
            65535
        };

        private int a_count;
        private byte[] accum = new byte[256];

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            this.imgW = width;
            this.imgH = height;
            this.pixAry = pixels;
            this.initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, Stream outs)
        {
            this.accum[this.a_count++] = c;
            if (this.a_count >= 254)
            {
                this.Flush(outs);
            }
        }

        private void ClearTable(Stream outs)
        {
            this.ResetCodeTable(this.hsize);
            this.free_ent = this.ClearCode + 2;
            this.clear_flg = true;
            this.Output(this.ClearCode, outs);
        }

        private void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; i++)
            {
                this.htab[i] = -1;
            }
        }

        private void Compress(int init_bits, Stream outs)
        {
            this.g_init_bits = init_bits;
            this.clear_flg = false;
            this.n_bits = this.g_init_bits;
            this.maxcode = this.MaxCode(this.n_bits);
            this.ClearCode = 1 << init_bits - 1;
            this.EOFCode = this.ClearCode + 1;
            this.free_ent = this.ClearCode + 2;
            this.a_count = 0;
            int num = this.NextPixel();
            int num2 = 0;
            for (int i = this.hsize; i < 65536; i *= 2)
            {
                num2++;
            }
            num2 = 8 - num2;
            int num3 = this.hsize;
            this.ResetCodeTable(num3);
            this.Output(this.ClearCode, outs);
            int num4;
            while ((num4 = this.NextPixel()) != LZWEncoder.EOF)
            {
                int i = (num4 << this.maxbits) + num;
                int num5 = num4 << num2 ^ num;
                if (this.htab[num5] == i)
                {
                    num = this.codetab[num5];
                }
                else
                {
                    if (this.htab[num5] >= 0)
                    {
                        int num6 = num3 - num5;
                        if (num5 == 0)
                        {
                            num6 = 1;
                        }
                        while (true)
                        {
                            if ((num5 -= num6) < 0)
                            {
                                num5 += num3;
                            }
                            if (this.htab[num5] == i)
                            {
                                break;
                            }
                            if (this.htab[num5] < 0)
                            {
                                goto IL_121;
                            }
                        }
                        num = this.codetab[num5];
                        continue;
                    }
                    IL_121:
                    this.Output(num, outs);
                    num = num4;
                    if (this.free_ent < this.maxmaxcode)
                    {
                        this.codetab[num5] = this.free_ent++;
                        this.htab[num5] = i;
                    }
                    else
                    {
                        this.ClearTable(outs);
                    }
                }
            }
            this.Output(num, outs);
            this.Output(this.EOFCode, outs);
        }

        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(this.initCodeSize));
            this.remaining = this.imgW * this.imgH;
            this.curPixel = 0;
            this.Compress(this.initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(Stream outs)
        {
            if (this.a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(this.a_count));
                outs.Write(this.accum, 0, this.a_count);
                this.a_count = 0;
            }
        }

        private int MaxCode(int n_bits)
        {
            return (1 << n_bits) - 1;
        }

        private int NextPixel()
        {
            if (this.remaining == 0)
            {
                return LZWEncoder.EOF;
            }
            this.remaining--;
            int num = this.curPixel + 1;
            if (num < this.pixAry.GetUpperBound(0))
            {
                byte b = this.pixAry[this.curPixel++];
                return (int)(b & 255);
            }
            return 255;
        }

        private void Output(int code, Stream outs)
        {
            this.cur_accum &= this.masks[this.cur_bits];
            if (this.cur_bits > 0)
            {
                this.cur_accum |= code << this.cur_bits;
            }
            else
            {
                this.cur_accum = code;
            }
            this.cur_bits += this.n_bits;
            while (this.cur_bits >= 8)
            {
                this.Add((byte)(this.cur_accum & 255), outs);
                this.cur_accum >>= 8;
                this.cur_bits -= 8;
            }
            if (this.free_ent > this.maxcode || this.clear_flg)
            {
                if (this.clear_flg)
                {
                    this.maxcode = this.MaxCode(this.n_bits = this.g_init_bits);
                    this.clear_flg = false;
                }
                else
                {
                    this.n_bits++;
                    if (this.n_bits == this.maxbits)
                    {
                        this.maxcode = this.maxmaxcode;
                    }
                    else
                    {
                        this.maxcode = this.MaxCode(this.n_bits);
                    }
                }
            }
            if (code == this.EOFCode)
            {
                while (this.cur_bits > 0)
                {
                    this.Add((byte)(this.cur_accum & 255), outs);
                    this.cur_accum >>= 8;
                    this.cur_bits -= 8;
                }
                this.Flush(outs);
            }
        }
    }
}