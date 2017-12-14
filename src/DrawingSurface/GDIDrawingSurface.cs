using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    public class GDIDrawingSurface : IGenericDrawingSurface
    {
        private Graphics canvas;

        private Brush ForegroundBrush = Brushes.Black;
        private Brush MutedForegroundBrush = Brushes.DarkGray;
        private Brush SubtleBrush = Brushes.LightGray;
        private Brush BackgroundBrush = Brushes.White;
        private Brush AccentBrush = new System.Drawing.SolidBrush((Color)new System.Drawing.ColorConverter().ConvertFrom("#FF312121"));

        public string DefaultFontName = "Arial";

        public GDIDrawingSurface(Graphics g)
        {
            canvas = g;
        }

        #region IGenericDrawingSurface Members

        public void DrawString(double x, double y, string text)
        {
            DrawString(x, y, text, ColorPreset.Foreground);
        }

        public void DrawString(double x, double y, string text, ColorPreset color)
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
            DrawString(x, y, text, size, ColorPreset.Foreground);
        }

        public void DrawString(double x, double y, string text, double fontSize, ColorPreset color)
        {
            Font f = new Font(DefaultFontName, (float)fontSize, GraphicsUnit.Pixel);
            canvas.DrawString(text, f, GetPresetBrush(color), (float)x, (float)y);
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorPreset color)
        {
            Pen p = new Pen(GetPresetBrush(color), (float)strokeThickness);
            canvas.DrawLine(p, (float)x1, (float)y1, (float)x2, (float)y2);
        }

        public void DrawArc(double x, double y, double width, bool isArcDown)
        {
            canvas.DrawArc(Pens.Black, (float)x, (float)y, (float)width, 10, 30, 30);
        }

        public void Clear()
        {
        }

        public void FillRectangle(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            System.Drawing.RectangleF rect = new RectangleF((float)x, (float)y, (float)w, (float)h);
            canvas.FillRectangle(GetPresetBrush(FillColor), rect);
            canvas.DrawRectangle(new Pen(GetPresetBrush(BorderColor)), rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillEllipse(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            System.Drawing.RectangleF rect = new RectangleF((float)x, (float)y, (float)w, (float)h);
            canvas.FillEllipse(GetPresetBrush(FillColor), rect);
            canvas.DrawEllipse(new Pen(GetPresetBrush(BorderColor)), rect);
        }

        public void FillEllipse(double x, double y, double w, double h, ColorValue FillColor, ColorValue BorderColor)
        {
            System.Drawing.RectangleF rect = new RectangleF((float)x, (float)y, (float)w, (float)h);
            Brush fillBrush = new System.Drawing.SolidBrush(Color.CadetBlue);
            Pen strokePen = new System.Drawing.Pen(Color.Aquamarine);
            canvas.FillEllipse(fillBrush, rect);
            canvas.DrawEllipse(strokePen, rect);
        }

        private Brush GetPresetBrush(ColorPreset c)
        {
            switch (c)
            {
                case ColorPreset.Foreground:
                    return ForegroundBrush;

                case ColorPreset.ForegroundText:
                    return BackgroundBrush;

                case ColorPreset.MutedForeground:
                    return MutedForegroundBrush;

                case ColorPreset.Subtle:
                    return SubtleBrush;

                case ColorPreset.Accent:
                    return AccentBrush;

                default:
                    return ForegroundBrush;
            }
        }

        public void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorValue BorderColor)
        {
            throw new NotImplementedException();
        }

        #endregion IGenericDrawingSurface Members
    }
}