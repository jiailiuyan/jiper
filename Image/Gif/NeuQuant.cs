using System;
using System.Diagnostics;

namespace Ji.ImageHelper.Gif
{
    public class NeuQuant
    {
        protected static readonly int netsize = 256;
        protected static readonly int prime1 = 499;
        protected static readonly int prime2 = 491;
        protected static readonly int prime3 = 487;
        protected static readonly int prime4 = 503;
        protected static readonly int minpicturebytes = 3 * NeuQuant.prime4;
        protected static readonly int maxnetpos = NeuQuant.netsize - 1;
        protected static readonly int netbiasshift = 4;
        protected static readonly int ncycles = 100;
        protected static readonly int intbiasshift = 16;
        protected static readonly int intbias = 1 << NeuQuant.intbiasshift;
        protected static readonly int gammashift = 10;
        protected static readonly int gamma = 1 << NeuQuant.gammashift;
        protected static readonly int betashift = 10;
        protected static readonly int beta = NeuQuant.intbias >> NeuQuant.betashift;
        protected static readonly int betagamma = NeuQuant.intbias << NeuQuant.gammashift - NeuQuant.betashift;
        protected static readonly int initrad = NeuQuant.netsize >> 3;
        protected static readonly int radiusbiasshift = 6;
        protected static readonly int radiusbias = 1 << NeuQuant.radiusbiasshift;
        protected static readonly int initradius = NeuQuant.initrad * NeuQuant.radiusbias;
        protected static readonly int radiusdec = 30;
        protected static readonly int alphabiasshift = 10;
        protected static readonly int initalpha = 1 << NeuQuant.alphabiasshift;
        protected int alphadec;
        protected static readonly int radbiasshift = 8;
        protected static readonly int radbias = 1 << NeuQuant.radbiasshift;
        protected static readonly int alpharadbshift = NeuQuant.alphabiasshift + NeuQuant.radbiasshift;
        protected static readonly int alpharadbias = 1 << NeuQuant.alpharadbshift;
        protected byte[] thepicture;
        protected int lengthcount;
        protected int samplefac;
        protected int[][] network;
        protected int[] netindex = new int[256];
        protected int[] bias = new int[NeuQuant.netsize];
        protected int[] freq = new int[NeuQuant.netsize];
        protected int[] radpower = new int[NeuQuant.initrad];

        public NeuQuant(byte[] thepic, int len, int sample)
        {
            this.thepicture = thepic;
            this.lengthcount = len;
            this.samplefac = sample;
            this.network = new int[NeuQuant.netsize][];
            for (int i = 0; i < NeuQuant.netsize; i++)
            {
                this.network[i] = new int[4];
                int[] array = this.network[i];
                array[0] = (array[1] = (array[2] = (i << NeuQuant.netbiasshift + 8) / NeuQuant.netsize));
                this.freq[i] = NeuQuant.intbias / NeuQuant.netsize;
                this.bias[i] = 0;
            }
        }

        public byte[] ColorMap()
        {
            byte[] array = new byte[3 * NeuQuant.netsize];
            int[] array2 = new int[NeuQuant.netsize];
            for (int i = 0; i < NeuQuant.netsize; i++)
            {
                array2[this.network[i][3]] = i;
            }
            int num = 0;
            for (int j = 0; j < NeuQuant.netsize; j++)
            {
                int num2 = array2[j];
                array[num++] = (byte)this.network[num2][0];
                array[num++] = (byte)this.network[num2][1];
                array[num++] = (byte)this.network[num2][2];
            }
            return array;
        }

        public void Inxbuild()
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < NeuQuant.netsize; i++)
            {
                int[] array = this.network[i];
                int num3 = i;
                int num4 = array[1];
                int[] array2;
                for (int j = i + 1; j < NeuQuant.netsize; j++)
                {
                    array2 = this.network[j];
                    if (array2[1] < num4)
                    {
                        num3 = j;
                        num4 = array2[1];
                    }
                }
                array2 = this.network[num3];
                if (i != num3)
                {
                    int j = array2[0];
                    array2[0] = array[0];
                    array[0] = j;
                    j = array2[1];
                    array2[1] = array[1];
                    array[1] = j;
                    j = array2[2];
                    array2[2] = array[2];
                    array[2] = j;
                    j = array2[3];
                    array2[3] = array[3];
                    array[3] = j;
                }
                if (num4 != num)
                {
                    this.netindex[num] = num2 + i >> 1;
                    for (int j = num + 1; j < num4; j++)
                    {
                        this.netindex[j] = i;
                    }
                    num = num4;
                    num2 = i;
                }
            }
            this.netindex[num] = num2 + NeuQuant.maxnetpos >> 1;
            for (int j = num + 1; j < 256; j++)
            {
                this.netindex[j] = NeuQuant.maxnetpos;
            }
        }

        public void Learn()
        {
            if (this.lengthcount < NeuQuant.minpicturebytes)
            {
                this.samplefac = 1;
            }
            this.alphadec = 30 + (this.samplefac - 1) / 3;
            byte[] array = this.thepicture;
            int num = 0;
            int num2 = this.lengthcount;
            int num3 = this.lengthcount / (3 * this.samplefac);
            int num4 = num3 / NeuQuant.ncycles;
            int num5 = NeuQuant.initalpha;
            int num6 = NeuQuant.initradius;
            int num7 = num6 >> NeuQuant.radiusbiasshift;
            if (num7 <= 1)
            {
                num7 = 0;
            }
            int i;
            for (i = 0; i < num7; i++)
            {
                this.radpower[i] = num5 * ((num7 * num7 - i * i) * NeuQuant.radbias / (num7 * num7));
            }
            int num8;
            if (this.lengthcount < NeuQuant.minpicturebytes)
            {
                num8 = 3;
            }
            else
            {
                if (this.lengthcount % NeuQuant.prime1 != 0)
                {
                    num8 = 3 * NeuQuant.prime1;
                }
                else
                {
                    if (this.lengthcount % NeuQuant.prime2 != 0)
                    {
                        num8 = 3 * NeuQuant.prime2;
                    }
                    else
                    {
                        if (this.lengthcount % NeuQuant.prime3 != 0)
                        {
                            num8 = 3 * NeuQuant.prime3;
                        }
                        else
                        {
                            num8 = 3 * NeuQuant.prime4;
                        }
                    }
                }
            }
            i = 0;
            while (i < num3)
            {
                int b = (int)(array[num] & 255) << NeuQuant.netbiasshift;
                int g = (int)(array[num + 1] & 255) << NeuQuant.netbiasshift;
                int r = (int)(array[num + 2] & 255) << NeuQuant.netbiasshift;
                int j = this.Contest(b, g, r);
                this.Altersingle(num5, j, b, g, r);
                if (num7 != 0)
                {
                    this.Alterneigh(num7, j, b, g, r);
                }
                num += num8;
                if (num >= num2)
                {
                    num -= this.lengthcount;
                }
                i++;
                if (num4 == 0)
                {
                    num4 = 1;
                }
                if (i % num4 == 0)
                {
                    num5 -= num5 / this.alphadec;
                    num6 -= num6 / NeuQuant.radiusdec;
                    num7 = num6 >> NeuQuant.radiusbiasshift;
                    if (num7 <= 1)
                    {
                        num7 = 0;
                    }
                    for (j = 0; j < num7; j++)
                    {
                        this.radpower[j] = num5 * ((num7 * num7 - j * j) * NeuQuant.radbias / (num7 * num7));
                    }
                }
            }
        }

        public int Map(int b, int g, int r)
        {
            int num = 1000;
            int result = -1;
            int num2 = this.netindex[g];
            int num3 = num2 - 1;
            while (num2 < NeuQuant.netsize || num3 >= 0)
            {
                if (num2 < NeuQuant.netsize)
                {
                    int[] array = this.network[num2];
                    int num4 = array[1] - g;
                    if (num4 >= num)
                    {
                        num2 = NeuQuant.netsize;
                    }
                    else
                    {
                        num2++;
                        if (num4 < 0)
                        {
                            num4 = -num4;
                        }
                        int num5 = array[0] - b;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num4 += num5;
                        if (num4 < num)
                        {
                            num5 = array[2] - r;
                            if (num5 < 0)
                            {
                                num5 = -num5;
                            }
                            num4 += num5;
                            if (num4 < num)
                            {
                                num = num4;
                                result = array[3];
                            }
                        }
                    }
                }
                if (num3 >= 0)
                {
                    int[] array = this.network[num3];
                    int num4 = g - array[1];
                    if (num4 >= num)
                    {
                        num3 = -1;
                    }
                    else
                    {
                        num3--;
                        if (num4 < 0)
                        {
                            num4 = -num4;
                        }
                        int num5 = array[0] - b;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num4 += num5;
                        if (num4 < num)
                        {
                            num5 = array[2] - r;
                            if (num5 < 0)
                            {
                                num5 = -num5;
                            }
                            num4 += num5;
                            if (num4 < num)
                            {
                                num = num4;
                                result = array[3];
                            }
                        }
                    }
                }
            }
            return result;
        }

        public byte[] Process()
        {
            this.Learn();
            this.Unbiasnet();
            this.Inxbuild();
            return this.ColorMap();
        }

        public void Unbiasnet()
        {
            for (int i = 0; i < NeuQuant.netsize; i++)
            {
                this.network[i][0] >>= NeuQuant.netbiasshift;
                this.network[i][1] >>= NeuQuant.netbiasshift;
                this.network[i][2] >>= NeuQuant.netbiasshift;
                this.network[i][3] = i;
            }
        }

        protected void Alterneigh(int rad, int i, int b, int g, int r)
        {
            int num = i - rad;
            if (num < -1)
            {
                num = -1;
            }
            int num2 = i + rad;
            if (num2 > NeuQuant.netsize)
            {
                num2 = NeuQuant.netsize;
            }
            int num3 = i + 1;
            int num4 = i - 1;
            int num5 = 1;
            while (num3 < num2 || num4 > num)
            {
                int num6 = this.radpower[num5++];
                if (num3 < num2)
                {
                    int[] array = this.network[num3++];
                    try
                    {
                        array[0] -= num6 * (array[0] - b) / NeuQuant.alpharadbias;
                        array[1] -= num6 * (array[1] - g) / NeuQuant.alpharadbias;
                        array[2] -= num6 * (array[2] - r) / NeuQuant.alpharadbias;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                if (num4 > num)
                {
                    int[] array = this.network[num4--];
                    try
                    {
                        array[0] -= num6 * (array[0] - b) / NeuQuant.alpharadbias;
                        array[1] -= num6 * (array[1] - g) / NeuQuant.alpharadbias;
                        array[2] -= num6 * (array[2] - r) / NeuQuant.alpharadbias;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        protected void Altersingle(int alpha, int i, int b, int g, int r)
        {
            int[] array = this.network[i];
            array[0] -= alpha * (array[0] - b) / NeuQuant.initalpha;
            array[1] -= alpha * (array[1] - g) / NeuQuant.initalpha;
            array[2] -= alpha * (array[2] - r) / NeuQuant.initalpha;
        }

        protected int Contest(int b, int g, int r)
        {
            int num = 2147483647;
            int num2 = num;
            int num3 = -1;
            int result = num3;
            for (int i = 0; i < NeuQuant.netsize; i++)
            {
                int[] array = this.network[i];
                int num4 = array[0] - b;
                if (num4 < 0)
                {
                    num4 = -num4;
                }
                int num5 = array[1] - g;
                if (num5 < 0)
                {
                    num5 = -num5;
                }
                num4 += num5;
                num5 = array[2] - r;
                if (num5 < 0)
                {
                    num5 = -num5;
                }
                num4 += num5;
                if (num4 < num)
                {
                    num = num4;
                    num3 = i;
                }
                int num6 = num4 - (this.bias[i] >> NeuQuant.intbiasshift - NeuQuant.netbiasshift);
                if (num6 < num2)
                {
                    num2 = num6;
                    result = i;
                }
                int num7 = this.freq[i] >> NeuQuant.betashift;
                this.freq[i] -= num7;
                this.bias[i] += num7 << NeuQuant.gammashift;
            }
            this.freq[num3] += NeuQuant.beta;
            this.bias[num3] -= NeuQuant.betagamma;
            return result;
        }
    }
}