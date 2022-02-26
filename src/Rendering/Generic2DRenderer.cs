using System;
using System.Collections.Generic;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    public enum ThemeColorPreset
    {
        Foreground = 0,
        TextShadow = 1,
        ForegroundText = 2,
        MutedForeground = 3,
        Subtle = 4,
        Accent = 5,
        MutedBackground = 6,
        Background = 7
    };

    public class GenericColorPreset
    {
        public GenericColorPreset(ThemeColorPreset presetToReplace, byte a, byte r, byte g, byte b)
        {
            preset = presetToReplace;
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public ThemeColorPreset preset { get; set; }
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }

    public class Generic2DRenderer
    {
        protected enum RenderOutput { GDI, WPF, PDF };

        public Dictionary<ThemeColorPreset, ColorValue> ColorPalette { get; set; }
        public bool EnableRendering { get; set; }

        public Generic2DRenderer()
        {
            ColorPalette = new Dictionary<ThemeColorPreset, ColorValue>();

            // setup default colour theme
            ColorPalette.Add(ThemeColorPreset.Foreground, new ColorValue(0, 0, 0));
            ColorPalette.Add(ThemeColorPreset.ForegroundText, new ColorValue(0, 0, 0));
            ColorPalette.Add(ThemeColorPreset.MutedForeground, new ColorValue(128, 128, 128));
            ColorPalette.Add(ThemeColorPreset.Background, new ColorValue(255, 255, 255));
            ColorPalette.Add(ThemeColorPreset.MutedBackground, new ColorValue(0, 0, 0));
            ColorPalette.Add(ThemeColorPreset.Subtle, new ColorValue(164, 164, 164));
            ColorPalette.Add(ThemeColorPreset.Accent, new ColorValue(0xb4, 0x55, 0xb6));

            EnableRendering = true;
        }

        public IGenericDrawingSurface InitialiseDrawingSurface(IGenericDrawingSurface canvas, double? width = null, double? height = null)
        {
            IGenericDrawingSurface g = canvas;

            return g;
        }

        public virtual void Render(IGenericDrawingSurface canvas)
        {
            throw new NotImplementedException();
        }

#if !SILVERLIGHT && !NETFX_CORE && !SHARPKIT && !MONO && !XAMARIN

        /*public void RenderPNG(string FileName)
        {
            Bitmap bmp = new Bitmap(900, 300);
            Graphics g = Graphics.FromImage(bmp);

            this.Render(g);

            //save png
            bmp.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);

            bmp.Dispose();
            g.Dispose();
        }*/

#endif
    }
}