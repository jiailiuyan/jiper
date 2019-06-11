using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Helper.JilyData
{

    public class TTFInfo
    {
        public FileInfo TTFFileInfo { get; private set; }

        private GlyphTypeface GlyphTypeface { get; set; }

        public Size Bounds { get; private set; }

        public string ErrorMessage { get; private set; }

        public TTFInfo(string file)
        {
            this.TTFFileInfo = new FileInfo(file);
            this.GlyphTypeface = new GlyphTypeface(new Uri(file, UriKind.Absolute));
        }

        public TTFInfo(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
        {
            this.TTFFileInfo = null;
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface = null;
            if (typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                this.GlyphTypeface = glyphTypeface;
            }

            if (this.GlyphTypeface == null)
            {
                var missfont = Application.Current.MainWindow.FontFamily;
                typeface = new Typeface(missfont, fontStyle, fontWeight, fontStretch);
                if (typeface.TryGetGlyphTypeface(out glyphTypeface))
                {
                    this.GlyphTypeface = glyphTypeface;
                }

                this.ErrorMessage = "当前系统无此字体";
            }
        }

        public ImageSource GetStrImage(string str, double fontsize, Brush foreBrush = null)
        {
            if (str != null)
            {
                var strs = ConverterText(str);
                if (str.Length > 0 && this.GlyphTypeface != null)
                {
                    DrawingGroup dg = new DrawingGroup();
                    double width = 1;
                    double height = 1;
                    foreach (var text in strs)
                    {
                        var glyphIndexes = new ushort[text.Length];
                        var advanceWidths = new double[text.Length];
                        for (int n = 0; n < text.Length; n++)
                        {
                            var glyphIndex = this.GlyphTypeface.CharacterToGlyphMap[text[n]];
                            glyphIndexes[n] = glyphIndex;
                            advanceWidths[n] = this.GlyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                        }

                        var gr = new GlyphRun(this.GlyphTypeface, 0, false, 1.0, glyphIndexes, new Point(0, 0), advanceWidths, null, null, null, null, null, null);
                        var glyphRunDrawing = new GlyphRunDrawing(foreBrush ?? Brushes.White, gr);
                        var w = glyphRunDrawing.Bounds.Width * fontsize;
                        var h = glyphRunDrawing.Bounds.Height * fontsize;
                        ImageDrawing dring = new ImageDrawing(new DrawingImage(glyphRunDrawing), new Rect(new Point(0, height), new Size(w, h)));
                        dg.Children.Add(dring);

                        width += w;
                        height += h;
                    }
                    Bounds = new Size(width, height);
                    return new DrawingImage(dg);
                }
            }

            return null;
        }

        public List<string> ConverterText(string text)
        {
            var t = text.Replace("&amp;", "&");
            t = t.Replace("\t", "   ");
            return t.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
        }

    }
}
