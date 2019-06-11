
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace HeaderWindow
{

    public class BorderDropInEffectEffect : ShaderEffect
    {
        public static readonly BorderDropInEffectEffect Effect = new BorderDropInEffectEffect();

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BorderDropInEffectEffect), 0);
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Color), typeof(BorderDropInEffectEffect), new UIPropertyMetadata(Color.FromArgb(255, 239, 239, 242), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register("BorderWidth", typeof(double), typeof(BorderDropInEffectEffect), new UIPropertyMetadata(((double)(855D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty BorderHeightProperty = DependencyProperty.Register("BorderHeight", typeof(double), typeof(BorderDropInEffectEffect), new UIPropertyMetadata(((double)(736D)), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty DropInWidthProperty = DependencyProperty.Register("DropInWidth", typeof(double), typeof(BorderDropInEffectEffect), new UIPropertyMetadata(((double)(10D)), PixelShaderConstantCallback(3)));
        public BorderDropInEffectEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/HeaderWindow;component/BorderDropInEffectEffect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(BorderColorProperty);
            this.UpdateShaderValue(BorderWidthProperty);
            this.UpdateShaderValue(BorderHeightProperty);
            this.UpdateShaderValue(DropInWidthProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        /// <summary>The direction of the blur (in degrees).</summary>
        public Color BorderColor
        {
            get
            {
                return ((Color)(this.GetValue(BorderColorProperty)));
            }
            set
            {
                this.SetValue(BorderColorProperty, value);
            }
        }
        /// <summary>The direction of the blur (in degrees).</summary>
        public double BorderWidth
        {
            get
            {
                return ((double)(this.GetValue(BorderWidthProperty)));
            }
            set
            {
                this.SetValue(BorderWidthProperty, value);
            }
        }
        /// <summary>The direction of the blur (in degrees).</summary>
        public double BorderHeight
        {
            get
            {
                return ((double)(this.GetValue(BorderHeightProperty)));
            }
            set
            {
                this.SetValue(BorderHeightProperty, value);
            }
        }
        /// <summary>The direction of the blur (in degrees).</summary>
        public double DropInWidth
        {
            get
            {
                return ((double)(this.GetValue(DropInWidthProperty)));
            }
            set
            {
                this.SetValue(DropInWidthProperty, value);
            }
        }
    }
}
