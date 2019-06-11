using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Helper.JilyControls
{
    public class FntViewControl
    {
        static string s = @"D:\sources\Fonts\qianqiu80.ttf";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var frm = new PrivateFontCollection();
            frm.AddFontFile(s);
            var FML = frm.Families[0];

            //var a = new System.Windows.Media.FontFamily(FML.Name);
            //var im = CreateGlyph(lblFont.Content.ToString(), a, this.lblFont.FontStyle, this.lblFont.FontWeight, this.lblFont.FontStretch, this.lblFont.Foreground);
            //img.Source = im;
        }

    }
}
