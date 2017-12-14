using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Webprofusion.Scalex.Util;

namespace Webprofusion.Scalex.Rendering
{
    public class WebDrawingSurface : IGenericDrawingSurface
    {
        //private string filename = "tablature.html";
        //private string defaultFontName = "Arial";
        //private double yoffset = 0;
        //private double lastY = 0;

        public bool RequestNewPage { get; set; }
        private double defaultFontSize = 12;
        private StringBuilder document = new StringBuilder();

        private void StartNewPage()
        {
        }

        public WebDrawingSurface()
        {
            document.AppendLine("<html><head>");
            //<script src=\"resources/raphael-min.js\" type=\"text/javascript\" charset=\"utf-8\"/>
            string rContent = System.IO.File.ReadAllText(Application.StartupPath + "\\resources\\raphael-min.js");
            document.AppendLine("<script type=\"text/javascript\">" + rContent + "</script>");
            document.AppendLine("</head>");

            document.AppendLine("<body><script type=\"text/javascript\"> var p = Raphael(10, 50, 960, 8000); ");
        }

        /// <summary>
        /// return final HTML document for rendered score
        /// </summary>
        /// <returns></returns>
        public string GetHTMLDocument()
        {
            return document.ToString() + "</script></body></html>";
        }

        #region IGenericDrawingSurface Members

        public void DrawString(double x, double y, string Text, double fontSize, ColorPreset color)
        {
            document.AppendLine("p.text(" + x + (fontSize / 2) + ", " + (y + (fontSize / 2)) + ", \"" + Text + "\");");
        }

        public void DrawString(double x, double y, string Text)
        {
            DrawString(x, y, Text, ColorPreset.Foreground);
        }

        public void DrawString(double x, double y, string Text, ColorPreset color)
        {
            DrawString(x, y, Text, defaultFontSize, color);
        }

        public void DrawStringCentered(double y, string text, double canvasWidth, double fontSize)
        {
            double x = (canvasWidth / 2) - ((text.Length * fontSize) / 2);
            DrawString(x, y, text, fontSize);
        }

        public void DrawString(double x, double y, string text, double size)
        {
            this.DrawString(x, y, text);
        }

        public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorPreset color)
        {
            document.AppendLine("p.path(\"M" + x1 + " " + y1 + "L" + x2 + " " + y2 + "\");");
        }

        public void DrawArc(double x, double y, double width, bool ArcDown)
        {
            //InitPageOffset(ref y);

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

        public void FillRectangle(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            document.AppendLine("p.rect(" + x + "," + y + "," + w + "," + h + ");");
        }

        public void FillEllipse(double x, double y, double w, double h, ColorPreset FillColor, ColorPreset BorderColor)
        {
            document.AppendLine("p.ellipse(" + x + "," + y + "," + w + "," + h + ");");
        }

        public void FillEllipse(double x, double y, double w, double h, ColorValue FillColor, ColorValue BorderColor)
        {
            document.AppendLine("p.ellipse(" + x + "," + y + "," + w + "," + h + ");");
        }

        public void Clear()
        {
            document.AppendLine("p.clear();");
        }

        public void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorValue BorderColor)
        {
            throw new System.NotImplementedException();
        }

        #endregion IGenericDrawingSurface Members
    }
}