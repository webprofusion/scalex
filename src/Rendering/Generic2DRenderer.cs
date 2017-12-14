using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Util;

#if !SILVERLIGHT && !NETFX_CORE && !XAMARIN

#endif

#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI;
#endif

#if !NETFX_CORE && !SHARPKIT && !MONO && !XAMARIN

#endif

#if SHARPKIT
using SharpKit.JavaScript;
#endif

#if MONO
using Scalex.Android.Core.Rendering;
#endif
#if XAMARIN
using SkiaSharp;
using GuitarToolkit;
#endif

namespace Webprofusion.Scalex.Rendering
{
    public enum ThemeColorPreset { Foreground = 0, ForegroundText = 1, MutedForeground = 2, Subtle = 3, Accent = 4, MutedBackground = 5, Background = 6 };

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
#if MONO
            g = new AndroidRenderer((Android.Graphics.Canvas)canvas);
#endif

#if SHARPKIT
            g = canvas.As<IGenericDrawingSurface>();
#endif

#if NETFX_CORE || DESKTOP || SILVERLIGHT
            if (canvas is Canvas)
            {
                //Set WPF specific settings
                g = new WPFDrawingSurface((Canvas)canvas);
                foreach (GenericColorPreset p in ColorOverrides)
                {
                    ((WPFDrawingSurface)g).OveridePresetColor(p);
                }
            }

            if (width != null && height != null)
            {
                ((Canvas)canvas).Width = (double)width;
                ((Canvas)canvas).Height = (double)height;
            }
#endif

#if !SILVERLIGHT && !NETFX_CORE && !SHARPKIT && !MONO && !XAMARIN
            /*
            float zoomLevel = GuitarModel.GuitarModelSettings.ZoomLevelDefault;
            if (canvas is Graphics)
            {
                //Set GDI Specific settings
                g = new GDIDrawingSurface((Graphics)canvas);

                //Set drawing mode to AntiAlias
                if (GuitarModel.GuitarModelSettings.EnableDiagramHighQuality)
                {
                    ((Graphics)canvas).CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    ((Graphics)canvas).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    ((Graphics)canvas).TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                }
                else
                {
                    ((Graphics)canvas).CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    ((Graphics)canvas).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    ((Graphics)canvas).TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                }

                //set drawing transform to scale drawing depending on zoom level
                ((Graphics)canvas).ScaleTransform(zoomLevel, zoomLevel);

                //fill background color
                SolidBrush backgroundBrush = new SolidBrush(Color.White);
                ((Graphics)canvas).FillRectangle(backgroundBrush, ((Graphics)canvas).VisibleClipBounds);
                backgroundBrush.Dispose();
            }

            if (canvas is PdfSharp.Pdf.PdfDocument)
            {
                //Set PDF specific settings
                g = new PDFDrawingSurface((PdfSharp.Pdf.PdfDocument)canvas);
                ((PDFDrawingSurface)g).RequestNewPage = true;
            }
*/

#endif

#if XAMARIN
			g= new GuitarToolkit.SkiaDrawingSurface((SKCanvas)canvas);
#endif
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