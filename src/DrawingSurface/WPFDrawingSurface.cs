using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

#if NETFX_CORE
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;
using Webprofusion.Scalex.Util;
#else

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Webprofusion.Scalex.Util;
#endif

namespace Webprofusion.Scalex.Rendering
{
    public class WPFDrawingSurface : IGenericDrawingSurface
    {
        private double textYOffset = 0;
        private double maxCoordBounds = 1000;
        private Canvas canvas;

        SolidColorBrush ForegroundBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush ForegroundText = new SolidColorBrush(Colors.White);
        SolidColorBrush MutedForegroundBrush = new SolidColorBrush(Colors.DarkGray);
        SolidColorBrush SubtleBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush AccentBrush = new SolidColorBrush(Colors.Orange);
        SolidColorBrush BackgroundBrush = new SolidColorBrush(Colors.Black);

        private FontFamily TitleFont = null;
        public void SetFontFamily(string fontFamilyName)
        {
            TitleFont = (FontFamily)new FontFamily(fontFamilyName);
        }


        public void OveridePresetColor(GenericColorPreset presetColor)
        {
#if NETFX_CORE
           var color = Windows.UI.Color.FromArgb(presetColor.A, presetColor.R, presetColor.G, presetColor.B);
#else
            var color = System.Windows.Media.Color.FromArgb(presetColor.A, presetColor.R, presetColor.G, presetColor.B);
#endif


            if (presetColor.preset == ColorPreset.Background) BackgroundBrush = new SolidColorBrush(color);
            if (presetColor.preset == ColorPreset.Foreground) ForegroundBrush = new SolidColorBrush(color);
            if (presetColor.preset == ColorPreset.ForegroundText) ForegroundText = new SolidColorBrush(color);
            if (presetColor.preset == ColorPreset.MutedForeground) MutedForegroundBrush = new SolidColorBrush(color);
            if (presetColor.preset == ColorPreset.Subtle) SubtleBrush = new SolidColorBrush(color);
            if (presetColor.preset == ColorPreset.Accent) AccentBrush = new SolidColorBrush(color);

        }

        /*        public void OveridePresetColor(ColorPreset preset, Color color)
                {
                    if (preset == ColorPreset.Foreground) ForegroundBrush = new SolidColorBrush(color);
                    if (preset == ColorPreset.ForegroundText) ForegroundText = new SolidColorBrush(color);
                    if (preset == ColorPreset.MutedForeground) MutedForegroundBrush = new SolidColorBrush(color);
                    if (preset == ColorPreset.Subtle) SubtleBrush = new SolidColorBrush(color);
                    if (preset == ColorPreset.Accent) AccentBrush = new SolidColorBrush(color);
                    if (preset == ColorPreset.Background) BackgroundBrush = new SolidColorBrush(color);

                }

                */

        public WPFDrawingSurface(Canvas targetCanvas)
        {
            canvas = targetCanvas;
            TitleFont = (FontFamily)new FontFamily("Arial");

#if !SILVERLIGHT && !NETFX_CORE
            RenderOptions.SetEdgeMode(canvas, EdgeMode.Aliased);
#else
            textYOffset = -2;
#endif

        }

        #region IGenericDrawingSurface Members

        public void DrawString(double x, double y, string Text)
        {
            DrawString(x, y, Text, ColorPreset.ForegroundText);
        }

        public void DrawString(double x, double y, string Text, ColorPreset color)
        {
            DrawString(x, y + textYOffset, Text, 6, color);
        }

        public void DrawStringCentered(double y, string text, double canvasWidth, double fontSize)
        {
            double x = (canvasWidth / 2) - ((text.Length * fontSize) / 2);
            DrawString(x, y + textYOffset, text, fontSize);
        }

        public void DrawString(double x, double y, string text, double size)
        {
            DrawString(x, y, text, size, ColorPreset.ForegroundText);
        }

        public void DrawString(double x, double y, string text, double size, ColorPreset color)
        {
            y += textYOffset;

            TextBlock t = new TextBlock();
            t.Name = Guid.NewGuid().ToString();
            t.TextWrapping = TextWrapping.Wrap;
            t.FontSize = size;
            t.Text = text;
            t.Foreground = GetPresetBrush(color);
            t.FontFamily = TitleFont;

            Canvas.SetTop(t, y);
            Canvas.SetLeft(t, x);

            canvas.Children.Add(t);
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorPreset color)
        {
            Line line = new Line();

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            line.Stroke = GetPresetBrush(color);
            line.StrokeThickness = strokeThickness;

            canvas.Children.Add(line);
        }

        public void DrawArc(double x, double y, double width, bool ArcDown)
        {
#if !SILVERLIGHT
            // Add an arc segment
            Path path = new Path();

            path.Stroke = ForegroundBrush;

            PathGeometry pathGeo = new PathGeometry();
            PathSegmentCollection segs = new PathSegmentCollection();

            ArcSegment seg = new ArcSegment();
            seg.Point = new Point(x + width, y);
            seg.Size= new Size(1, 1);
            seg.SweepDirection= ArcDown ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;

            segs.Add(seg);

            //segs.Add(new ArcSegment { 
                //Point = new Point(x + width, y),
                //seg.Size= new Size(2,2),
            //});

            PathFigure fig = new PathFigure();
            fig.StartPoint = new Point(x, y);
            
            fig.Segments=segs;

            pathGeo.Figures.Add(fig);
            path.Data = pathGeo;

            canvas.Children.Add(path);
#endif
        }

        public void Clear()
        {
            canvas.Children.Clear();
        }

        public void FillRectangle(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            Rectangle rect = new Rectangle();
            rect.Width = w;
            rect.Height = h;

            rect.Fill = GetPresetBrush(FillColor);
            rect.Stroke = GetPresetBrush(BorderColor);

            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);

            canvas.Children.Add(rect);
        }

        public void FillEllipse(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            var fillColorValue = GetPresetBrush(FillColor);
            var strokeColorValue = GetPresetBrush(BorderColor);

            this.FillEllipse(x, y, w, h,
                new ColorValue { A = fillColorValue.Color.A, R = fillColorValue.Color.R, G = fillColorValue.Color.G, B = fillColorValue.Color.B },
                new ColorValue { A = strokeColorValue.Color.A, R = strokeColorValue.Color.R, G = strokeColorValue.Color.G, B = strokeColorValue.Color.B }
                );
        }

        public void FillEllipse(double x, double y, double w, double h, ColorValue FillColor, ColorValue BorderColor)
        {
            /*if (x < 0 || x > maxCoordBounds) return;
            if (y < 0 || y > maxCoordBounds) return;*/

            Ellipse e = new Ellipse();

            e.Width = w;
            e.Height = h;

#if NETFX_CORE
            e.Fill = new SolidColorBrush(Color.FromArgb(FillColor.A, FillColor.R, FillColor.G, FillColor.B));
            e.Stroke = new SolidColorBrush(Color.FromArgb(BorderColor.A, BorderColor.R, BorderColor.G, BorderColor.B));
#else
            e.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(FillColor.A, FillColor.R, FillColor.G, FillColor.B));
            e.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BorderColor.A, BorderColor.R, BorderColor.G, BorderColor.B));
#endif


            Canvas.SetTop(e, y);
            Canvas.SetLeft(e, x);

            canvas.Children.Add(e);
        }


        public void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorValue BorderColor)
        {
            var fillColorValue = GetPresetBrush(FillColor);
            
            this.FillEllipse(x1, y1, w, h,
                new ColorValue { A = fillColorValue.Color.A, R = fillColorValue.Color.R, G = fillColorValue.Color.G, B = fillColorValue.Color.B },
                BorderColor
                );
        }

        private SolidColorBrush GetPresetBrush(ColorPreset c)
        {
            switch (c)
            {
                case ColorPreset.Foreground:
                    return ForegroundBrush;
                case ColorPreset.ForegroundText:
                    return ForegroundText;
                case ColorPreset.MutedForeground:
                    return MutedForegroundBrush;
                case ColorPreset.Subtle:
                    return SubtleBrush;
                case ColorPreset.Accent:
                    return AccentBrush;
                case ColorPreset.Background:
                    return BackgroundBrush;
                default:
                    return ForegroundBrush;
            }
        }

        #endregion


    }

}
