using SkiaSharp;
using Webprofusion.Scalex.Rendering;
using Webprofusion.Scalex.Util;

namespace Scalex
{
    public class SkiaDrawingSurface : IGenericDrawingSurface
    {
        private SKCanvas canvas;

        /*private static readonly SKColor ForegroundColor = new SKColor(0, 0, 0);
        private static readonly SKColor ForegroundText = new SKColor(0, 0, 0);
        private static readonly SKColor MutedForegroundColor = new SKColor(128, 128, 128);
        private static readonly SKColor SubtleColor = new SKColor(164, 164, 164);

        private static readonly SKColor AccentColor = new SKColor(0xb4, 0x55, 0xb6);
        private static readonly SKColor MutedBackground = new SKColor(32, 32, 32);
        private static readonly SKColor BackgroundColor = new SKColor(255, 255, 255);*/

        //public enum ColorPreset { Foreground=0, ForegroundText=1, MutedForeground=2, Subtle=3, Accent=4, MutedBackground=5, Background=6 };

        public string DefaultFontName = "Arial";

        private float _scale = 4;
        private SKTypeface typeface;

        public SkiaDrawingSurface(SKCanvas g)
        {
            canvas = g;

            // canvas.Scale(new SKPoint(_scale, _scale));

            typeface = SKFontManager.Default.MatchCharacter("Arial", '#');
            System.Diagnostics.Debug.WriteLine("Autoselected font:" + typeface.FamilyName);
        }

        public void SetScale(float scale)
        {
            _scale = scale;
            canvas.Scale(new SKPoint(scale, scale));
        }

        public float GetScale()
        {
            return _scale;
        }

        public static void ApplyThemeColours(Generic2DRenderer surface)
        {
            var foregroundThemeColor = new SKColor(255, 255, 255);
            var mutedForegroundThemeColor = new SKColor(0, 0, 0);
            surface.ColorPalette[ThemeColorPreset.ForegroundText] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
            surface.ColorPalette[ThemeColorPreset.Foreground] = new ColorValue(foregroundThemeColor.Red, foregroundThemeColor.Green, foregroundThemeColor.Blue);
            surface.ColorPalette[ThemeColorPreset.MutedForeground] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
            surface.ColorPalette[ThemeColorPreset.TextShadow] = new ColorValue(mutedForegroundThemeColor.Red, mutedForegroundThemeColor.Green, mutedForegroundThemeColor.Blue);
        }

        #region IGenericDrawingSurface Members

        public void DrawString(double x, double y, string text)
        {
            DrawString(x, y, text, new ColorValue(0, 0, 0));
        }

        public void DrawString(double x, double y, string text, ColorValue color)
        {
            DrawString(x, y, text, 9, color);
        }

        public void DrawStringCentered(double y, string text, double canvasWidth, double fontSize)
        {
            double x = (canvasWidth / 2) - ((text.Length * fontSize) / 2);
            DrawString(x, y, text, fontSize);
        }

        public void DrawString(double x, double y, string text, double size)
        {
            DrawString(x, y, text, size, new ColorValue(0, 0, 0));
        }

        public void DrawString(double x, double y, string text, double fontSize, ColorValue color)
        {
            using (var paint = new SKPaint())
            {
                paint.Typeface = typeface;
                paint.TextSize = (float)fontSize;
                paint.IsAntialias = true;
                paint.Color = GetColorFromColorValue(color);
                paint.IsStroke = false;
                y = y + (fontSize / 2);
                canvas.DrawText(text, (float)x, (float)y, paint);
            }
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorValue color)
        {
            //Pen p = new Pen(GetPresetBrush(color), (float)strokeThickness);
            //canvas.DrawLine(p, (float)x1, (float)y1, (float)x2, (float)y2);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = GetColorFromColorValue(color);

                paint.StrokeWidth = (float)strokeThickness;

                paint.StrokeCap = SKStrokeCap.Round;
                var t = paint.StrokeCap;

                canvas.DrawLine((float)x1, (float)y1, (float)x2, (float)y2, paint);
            }
        }

        public void DrawArc(double x, double y, double width, bool isArcDown)
        {
            //canvas.DrawArc(Pens.Black, (float)x, (float)y, (float)width, 10, 30, 30);
        }

        public void Clear(ColorValue backgroundColor)
        {
            canvas.Clear();
            using (var paint = new SKPaint())
            {
                paint.Color = GetColorFromColorValue(backgroundColor);
                canvas.DrawPaint(paint);
            }
        }

        public void FillRectangle(double x, double y, double w, double h, ColorValue fillColor, ColorValue borderColor)
        {
            var rect = new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h));
            using (var paint = new SKPaint())
            {
                paint.Color = GetColorFromColorValue(borderColor);
                canvas.DrawRect(rect, paint);
            }
        }

        public void FillEllipse(double x1, double y1, double w, double h, ColorValue FillColor, ColorValue BorderColor)
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;

                var rect = new SKRect((float)x1, (float)y1, (float)(x1 + w), (float)(y1 + h));
                paint.Color = GetColorFromColorValue(FillColor);
                paint.IsStroke = false;

                canvas.DrawOval(rect, paint);

                paint.Color = GetColorFromColorValue(BorderColor);
                paint.IsStroke = true;
                paint.StrokeWidth = 0.5f;
                canvas.DrawOval(rect, paint);
            }
        }

        private SKColor GetColorFromColorValue(ColorValue v)
        {
            return new SKColor(v.R, v.G, v.B, v.A);
        }

        #endregion IGenericDrawingSurface Members
    }
}