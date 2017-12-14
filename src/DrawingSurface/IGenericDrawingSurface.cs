using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webprofusion.Scalex.Util;


namespace Webprofusion.Scalex.Rendering
{
    public interface IGenericDrawingSurface
    {
        void DrawString(double x, double y, string text);
        void DrawString(double x, double y, string text, ColorPreset color);
        void DrawString(double x, double y, string text,  double fontSize, ColorPreset color);
        void DrawString(double x, double y, string text, double fontSize);
        void DrawStringCentered(double y, string text, double canvasWidth, double fontSize);
        void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorPreset color);
        void DrawArc(double x, double y, double width, bool isArcDown);
        void FillRectangle(double x1, double y1, double w, double h, ColorPreset FillColor, ColorPreset BorderColor);
        void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorPreset BorderColor);
        void FillEllipse(double x1, double y1, double w, double h, ColorPreset FillColor, ColorValue BorderColor);
        void FillEllipse(double x1, double y1, double w, double h, ColorValue FillColor, ColorValue BorderColor);
        void Clear();

    }
}
