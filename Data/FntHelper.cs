using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Helper.JilyImage;

namespace Helper.JilyData
{

    public class FntInfo
    {

        public FileInfo FntFileInfo { get; set; }

        /// <summary> 字体名称 </summary>
        public string Name { get; set; }

        /// <summary> 大小为32像素 </summary>
        public int Size { get; set; }

        /// <summary> 加粗 </summary>
        public bool Bold { get; set; }

        /// <summary> 斜体 </summary>
        public bool Italic { get; set; }

        /// <summary> 编码字符集，没有填写值即使用默认 </summary>
        public string Charset { get; set; }

        /// <summary> 使用Unicode </summary>
        public bool Unicode { get; set; }

        /// <summary> 纵向缩放百分比 </summary>
        public double StretchH { get; set; }

        /// <summary> 开启平滑 </summary>
        public bool Smooth { get; set; }

        /// <summary> 开启抗锯齿 </summary>
        public bool Aa { get; set; }

        /// <summary> 内边距，文字与边框的空隙 </summary>
        public FntPadding Padding { get; set; }

        /// <summary> 外边距，就是相临边缘的距离 </summary>
        public FntPadding Spacing { get; set; }

        public FntCommon Common { get; set; }

        /// <summary> 此种字体共用到图数 </summary>
        public List<FntPage> Pages { get; set; }

        public List<FntChar> Chars { get; set; }

        public List<FntKerning> Kernings { get; set; }

        public Rect Bounds { get; private set; }

        private FntInfo()
        {
            this.Common = new FntCommon();
            this.Pages = new List<FntPage>();
            this.Chars = new List<FntChar>();
            this.Kernings = new List<FntKerning>();
        }

        public FntInfo(string fntpath, Encoding encoding = null)
            : this()
        {
            this.FntFileInfo = new FileInfo(fntpath);
            if (this.FntFileInfo.Exists)
            {
                var fntlines = File.ReadAllLines(this.FntFileInfo.FullName, encoding ?? Encoding.Default);
                foreach (var item in fntlines)
                {
                    if (item.StartsWith("info"))
                    {
                        DecodeInfo(item);
                    }
                    else if (item.StartsWith("common"))
                    {
                        DecodeCommon(item);
                    }
                    else if (item.StartsWith("page"))
                    {
                        DecodePage(item);
                    }
                    else if (item.StartsWith("char") && !item.StartsWith("chars"))
                    {
                        DecodeChar(item);
                    }
                    else if (item.StartsWith("kerning") && !item.StartsWith("kernings"))
                    {
                        DecodeKerning(item);
                    }
                }
            }
        }

        private void DecodeInfo(string str)
        {
            this.Name = GetValueAfterTittle(str, "face=\"", '"');
            this.Size = int.Parse(GetValueAfterTittle(str, "size="));
            this.Bold = int.Parse(GetValueAfterTittle(str, "bold=")) != 0;
            this.Italic = int.Parse(GetValueAfterTittle(str, "italic=")) != 0;
            this.Charset = GetValueAfterTittle(str, "charset=\"", '"');
            this.Unicode = int.Parse(GetValueAfterTittle(str, "unicode=")) != 0;
            this.StretchH = int.Parse(GetValueAfterTittle(str, "stretchH="));
            this.Smooth = int.Parse(GetValueAfterTittle(str, "smooth=")) != 0;
            this.Aa = int.Parse(GetValueAfterTittle(str, "aa=")) != 0;
            this.Padding = new FntPadding(GetValueAfterTittle(str, "padding="));
            this.Spacing = new FntPadding(GetValueAfterTittle(str, "spacing="));
        }

        private void DecodeCommon(string str)
        {
            this.Common.LineHeight = int.Parse(GetValueAfterTittle(str, "lineHeight="));
            this.Common.Base = int.Parse(GetValueAfterTittle(str, "base="));
            this.Common.ScaleW = int.Parse(GetValueAfterTittle(str, "scaleW="));
            this.Common.ScaleH = int.Parse(GetValueAfterTittle(str, "scaleH="));
            this.Common.Packed = int.Parse(GetValueAfterTittle(str, "lineHeight=")) != 0;
        }

        private void DecodePage(string str)
        {
            var page = new FntPage();
            page.Id = int.Parse(GetValueAfterTittle(str, "id="));
            page.FilePath = GetValueAfterTittle(str, "file=\"", '"');
            page.RealPath = this.FntFileInfo.DirectoryName + "\\" + page.FilePath;
            this.Pages.Add(page);
        }

        private void DecodeChar(string str)
        {
            var fntchar = new FntChar();
            fntchar.Id = int.Parse(GetValueAfterTittle(str, "id="));
            fntchar.X = int.Parse(GetValueAfterTittle(str, "x="));
            fntchar.Y = int.Parse(GetValueAfterTittle(str, "y="));
            fntchar.Width = int.Parse(GetValueAfterTittle(str, "width="));
            fntchar.Height = int.Parse(GetValueAfterTittle(str, "height="));
            fntchar.Xoffset = int.Parse(GetValueAfterTittle(str, "xoffset="));
            fntchar.Yoffset = int.Parse(GetValueAfterTittle(str, "yoffset="));
            fntchar.Xadvance = int.Parse(GetValueAfterTittle(str, "xadvance="));
            fntchar.Page = int.Parse(GetValueAfterTittle(str, "page="));
            fntchar.Chnl = int.Parse(GetValueAfterTittle(str, "chnl="));

            fntchar.InitData(this);

            this.Chars.Add(fntchar);
        }

        private void DecodeKerning(string str)
        {
            var kerning = new FntKerning();
            kerning.First = int.Parse(GetValueAfterTittle(str, "first="));
            kerning.Second = int.Parse(GetValueAfterTittle(str, "second="));
            kerning.Amount = int.Parse(GetValueAfterTittle(str, "amount="));
            this.Kernings.Add(kerning);
        }

        public ImageSource GetStrImage(string text)
        {
            //1 移除不在范围内的字符
            List<char> chars = new List<char>();
            foreach (var item in text)
            {
                if (this.Chars.FirstOrDefault(fc => fc.Id == (int)item) != null)
                {
                    chars.Add(item);
                }
            }

            int viewwidth = 1;
            int viewheight = 1;
            int count = chars.Count - 1;
            for (int i = 0; i <= count; i++)
            {
                char item = chars[i];
                var c = this.Chars.FirstOrDefault(fc => fc.Id == (int)item);
                if (c != null)
                {
                    //设置宽高，用最大高度
                    viewheight = (c.Height + c.Yoffset) > viewheight ? (c.Height + c.Yoffset) : viewheight;
                    viewwidth += (c.Xadvance);

                    if (i != 0)
                    {
                        var kerning = this.Kernings.FirstOrDefault(k => k.First == (int)item && k.Second == (int)chars[i - 1]);
                        if (kerning != null)
                        {
                            //补齐长度，在存在偏移的时候，偏移量也计算在总长度
                            viewwidth += kerning.Amount;
                        }
                    }
                }
            }

            int currentxoffset = 0;
            var img = new Bitmap(viewwidth, viewheight);
            using (var g = Graphics.FromImage(img))
            {
                for (int i = 0; i <= count; i++)
                {
                    char item = chars[i];
                    var c = this.Chars.FirstOrDefault(fc => fc.Id == (int)item);
                    if (c != null)
                    {
                        var xposition = currentxoffset + c.Xoffset;
                        int yposition = c.Yoffset;

                        if (i != 0)
                        {
                            //前后两个字符进行偏移
                            var kerning = this.Kernings.FirstOrDefault(k => k.First == (int)item && k.Second == (int)chars[i - 1]);
                            if (kerning != null)
                            {
                                //确定位置，和前一个相比，加上需要偏移的位置
                                xposition += kerning.Amount;
                            }
                        }
                        //当显示文字为空格 char 32 时不需要绘制
                        if (c.Data != null)
                        {
                            g.DrawImage(c.Data, xposition, yposition, c.Width, c.Height);
                        }
                        //叠加当前绘制过的长度
                        currentxoffset += (c.Xadvance);
                    }
                }
            }

            Bounds = new Rect(0, 0, viewwidth, viewheight);

            return img.ConvertImageSource();
        }

        private string GetValueAfterTittle(string s, string tittle, char endwith = ' ')
        {
            //补齐一个空格，用来判断最后的字符
            var str = s + " ";
            if (!string.IsNullOrWhiteSpace(str))
            {
                int firstindex = str.IndexOf(tittle) + tittle.Length;
                int lastindex = str.IndexOf(endwith, firstindex);
                if (firstindex != -1 && lastindex != -1)
                {
                    return new string(str.Skip(firstindex).Take(lastindex - firstindex).ToArray());
                }
            }
            return "";
        }

    }

    public class FntCommon
    {
        /// <summary> 行高，如果遇到换行符时，绘制字的位置坐标的Y值在换行后增加的像素值 </summary>
        public int LineHeight { get; set; }

        /// <summary> 字的基本大小 </summary>
        public int Base { get; set; }

        /// <summary> 图片宽 </summary>
        public int ScaleW { get; set; }

        /// <summary>
        /// 图片高
        /// </summary>
        public int ScaleH { get; set; }

        //没搞明白什么用
        //alphaChnl = int.Parse(GetValueAfterTittle(str, "alphaChnl="));
        //redChnl = int.Parse(GetValueAfterTittle(str, "redChnl="));
        //greenChnl = int.Parse(GetValueAfterTittle(str, "greenChnl="));
        //blueChnl = int.Parse(GetValueAfterTittle(str, "blueChnl="));

        /// <summary> 图片压缩 </summary>
        public bool Packed { get; set; }

        public FntCommon()
        {

        }
    }

    public class FntPage
    {
        /// <summary> 当前页数 </summary>
        public int Id { get; set; }

        /// <summary> 图片相对fnt文件的路径 </summary>
        public string FilePath { get; set; }

        private string realPath;
        public string RealPath
        {
            get { return realPath; }
            set
            {
                realPath = value;
                FntImage = BitmapHelper.GetBitmap(realPath);
            }
        }

        public Bitmap FntImage { get; private set; }

    }

    public class FntChar
    {

        /// <summary> 文字的 ID </summary>
        public int Id { get; set; }

        /// <summary> 所在图片的X </summary>
        public int X { get; set; }
        /// <summary> 所在图片的Y </summary>
        public int Y { get; set; }

        /// <summary> 宽 </summary>
        public int Width { get; set; }
        /// <summary> 高 </summary>
        public int Height { get; set; }

        /// <summary> 像素偏移X </summary>
        public int Xoffset { get; set; }
        /// <summary> 像素偏移Y </summary>
        public int Yoffset { get; set; }

        /// <summary> 绘制完后相应位置的x往后移 ... 像素再画下一个字 </summary>
        public int Xadvance { get; set; }

        /// <summary> 所在图片页 </summary>
        public int Page { get; set; }

        /// <summary> 未知 </summary>
        public int Chnl { get; set; }

        public Bitmap Data { get; private set; }

        static int count = 0;
        public void InitData(FntInfo fntinfo)
        {
            count++;
            var page = fntinfo.Pages.FirstOrDefault(i => i.Id == this.Page);

            if (page != null)
            {
                var rect = new Rectangle(this.X, this.Y, this.Width, this.Height);
                if (Rectangle.Empty != rect && !(this.Width == 0 && this.Height == 0))
                {
                    this.Data = page.FntImage.Clone(rect, page.FntImage.PixelFormat);
                }
            }
        }

        public override string ToString()
        {
            return ((char)this.Id).ToString();
        }
    }

    public class FntKerning
    {
        /// <summary> 位移的前字符 </summary>
        public int First { get; set; }

        /// <summary> 位移的后字符 </summary>
        public int Second { get; set; }

        /// <summary> X右移位置，正为右 </summary>
        public int Amount { get; set; }

    }

    public struct FntPadding
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }
        public int V4 { get; set; }

        public FntPadding(int v1, int v2, int v3, int v4)
            : this()
        {
            this.V1 = v1;
            this.V2 = v2;
            this.V3 = v3;
            this.V4 = v4;
        }

        public FntPadding(string value)
            : this()
        {
            var values = value.Split(',');
            if (values.Length == 2)
            {
                this.V1 = int.Parse(values[0]);
                this.V2 = int.Parse(values[0]);
                this.V3 = int.Parse(values[1]);
                this.V4 = int.Parse(values[1]);
            }
            else if (value.Length == 4)
            {
                this.V1 = int.Parse(values[0]);
                this.V2 = int.Parse(values[1]);
                this.V3 = int.Parse(values[2]);
                this.V4 = int.Parse(values[3]);
            }
        }
    }
}
