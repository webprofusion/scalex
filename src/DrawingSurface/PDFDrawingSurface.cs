using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    public class PDFDrawingSurface : IGenericDrawingSurface
    {
        private PdfDocument document = new PdfDocument();
        private string filename = "tablature.pdf";
        private string defaultFontName = "Arial";
        private PdfPage currentPage = null;
        public bool RequestNewPage { get; set; }
        private double yoffset = 0;
        private double lastY = 0;
        private double defaultFontSize = 12;

        private double InitPageOffset(ref double y)
        {
            return InitPageOffset(ref y, true);
        }

        private void StartNewPage()
        {
            currentPage = document.AddPage();
        }

        private double InitPageOffset(ref double y, bool addPage)
        {
            double offset = 0;

            double pageStartY = (document.PageCount - 1) * currentPage.Height.Value;

            if (y - yoffset > currentPage.Height.Value)
            {
                addPage = true;
                yoffset = pageStartY;// +currentPage.Height.Value;
            }
            else
            {
                addPage = false;
            }

            if (RequestNewPage && yoffset == 0) RequestNewPage = false;

            if (addPage || RequestNewPage)
            {
                //add new page
                StartNewPage();

                if (RequestNewPage) StartNewPage();
                RequestNewPage = false;

                /*
                if (RequestNewPage)
                {
                    offset = y;
                }
                else
                {
                    offset = currentPage.Height.Value;
                }
                y = y - offset;
                */
            }
            else
            {
                //standard translation is to subtract the current page start Y
                //if (y > pageStartY && document.PageCount > 0)
                //if (y > currentPage.Height.Value && document.PageCount > 0)
                /* if (y > currentPage.Height.Value && document.PageCount > 0)
                 {
                     offset = currentPage.Height.Value * document.PageCount;
                     y = y - offset;
                 }*/
            }
            offset = yoffset;

            y = y - offset;

            lastY = y;

            return offset;
        }

        public PDFDrawingSurface(PdfDocument canvas)
        {
            document = canvas;
            if (document.Pages.Count == 0)
            {
                currentPage = document.AddPage();

                // Create the root bookmark. You can set the style and the color.
                PdfOutline outline = new PdfOutline("Root", currentPage, true);
                //
                //currentPage.Contents.Outlines.Add("Root", currentPage, true, PdfOutlineStyle.Bold, XColors.Black);
            }
            else
            {
                currentPage = document.Pages[document.Pages.Count - 1];
            }
        }

        public void SaveDocument()
        {
            // Save the document...
            document.Save(filename);
            // ...and start a viewer
            Process.Start(filename);
        }

        #region IGenericDrawingSurface Members

        public void DrawString(double x, double y, string Text)
        {
            DrawString(x, y, Text, ColorPreset.Foreground);
        }

        public void DrawString(double x, double y, string Text, double fontSize, ColorPreset color)
        {
            InitPageOffset(ref y);

            using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
            {
                XFont font = new XFont(defaultFontName, defaultFontSize, XFontStyle.Regular);
                gfx.DrawString(Text, font, XBrushes.Navy, x, y, XStringFormats.BottomCenter);
            }
        }

        public void DrawString(double x, double y, string Text, ColorPreset color)
        {
            DrawString(x, y, Text, defaultFontSize, color);
        }

        public void DrawStringCentered(double y, string text, double canvasWidth, double fontSize)
        {
            InitPageOffset(ref y);

            using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
            {
                XFont font = new XFont(defaultFontName, defaultFontSize, XFontStyle.Regular);
                gfx.DrawString(text, font, XBrushes.Navy, canvasWidth / 2, y, XStringFormats.BottomCenter);
            }

            //double x = (canvasWidth / 2) - ((text.Length * fontSize) / 2);
            //DrawString(x, y, text, fontSize);
        }

        public void DrawString(double x, double y, string text, double size)
        {
            InitPageOffset(ref y);

            using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
            {
                XFont font = new XFont(defaultFontName, size, XFontStyle.Regular);
                gfx.DrawString(text, font, XBrushes.Navy, x, y);
            }
            /* TextBlock t = new TextBlock();

             t.TextWrapping = TextWrapping.Wrap;
             t.FontSize = size;
             t.Text = text;

             Canvas.SetTop(t, y);
             Canvas.SetLeft(t, x);

             canvas.Children.Add(t);
             * */
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorPreset color)
        {
            y2 = y2 - InitPageOffset(ref y1);

            using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
            {
                gfx.DrawLine(XPens.Black, x1, y1, x2, y2);
            }
            /*Line line = new Line();

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

#if !SILVERLIGHT
            line.Stroke = Brushes.DarkGray;
#else
            line.Stroke = BlackBrush;
#endif

            line.StrokeThickness = strokeThickness;

            canvas.Children.Add(line);
             * */
        }

        public void DrawArc(double x, double y, double width, bool ArcDown)
        {
            InitPageOffset(ref y);

            /*
#if !SILVERLIGHT
            // Add an arc segment
            Path path = new Path();

            path.Stroke = Brushes.Black;

            PathGeometry pathGeo = new PathGeometry();
            PathSegmentCollection segs = new PathSegmentCollection();

            ArcSegment seg = new ArcSegment(new Point(x + width, y), new Size(1, 1), 0, false, ArcDown ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true);

            segs.Add(seg);

            PathFigure fig = new PathFigure(new Point(x, y), segs, false);

            pathGeo.Figures.Add(fig);
            path.Data = pathGeo;

            canvas.Children.Add(path);
#endif
             * */
        }

        public void Clear()
        {
            //document.Pages[0].re
            //canvas.Children.Clear();
        }

        public void FillRectangle(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            // y2 = y2 -
            InitPageOffset(ref y);

            using (XGraphics gfx = XGraphics.FromPdfPage(currentPage))
            {
                XPen pen = new XPen(XColors.White);
                XBrush brush = XBrushes.White;
                gfx.DrawRectangle(pen, brush, x, y, w, h);
            }
            /*  Rectangle rect = new Rectangle();
              rect.Width = x2 - x2;
              rect.Height = y2 - y1;

  #if !SILVERLIGHT
              rect.Fill = Brushes.WhiteSmoke;
  #else
              rect.Fill = WhiteBrush;
  #endif

              Canvas.SetTop(rect, y1);
              Canvas.SetLeft(rect, x1);

              canvas.Children.Add(rect);
              */
        }

        public void FillEllipse(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
        }

        public void FillEllipse(double x, double y, double w, double h, ColorValue FillColor, ColorValue BorderColor)
        {
        }

        public void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorValue BorderColor)
        {
            throw new NotImplementedException();
        }

        #endregion IGenericDrawingSurface Members
    }
}