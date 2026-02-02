using SkiaSharp;
using Webprofusion.Scalex.Rendering;
using Webprofusion.Scalex.Util;

namespace Scalex.RazorPages.Rendering;

public sealed class SkiaDrawingSurface : IGenericDrawingSurface
{
    private readonly SKCanvas _canvas;
    private float _scale = 1f;
    private readonly SKTypeface _typeface;

    public SkiaDrawingSurface(SKCanvas canvas)
    {
        _canvas = canvas;
        _typeface = SKFontManager.Default.MatchCharacter("Arial", '#');
    }

    public void SetScale(float scale)
    {
        _scale = scale;
        _canvas.Scale(new SKPoint(scale, scale));
    }

    public float GetScale() => _scale;

    public void DrawString(double x, double y, string text) => DrawString(x, y, text, new ColorValue(0, 0, 0));

    public void DrawString(double x, double y, string text, ColorValue color) => DrawString(x, y, text, 9, color);

    public void DrawStringCentered(double y, string text, double canvasWidth, double fontSize)
    {
        double x = (canvasWidth / 2) - ((text.Length * fontSize) / 2);
        DrawString(x, y, text, fontSize);
    }

    public void DrawString(double x, double y, string text, double size) => DrawString(x, y, text, size, new ColorValue(0, 0, 0));

    public void DrawString(double x, double y, string text, double fontSize, ColorValue color)
    {
        using var paint = new SKPaint();
        paint.Typeface = _typeface;
        paint.TextSize = (float)fontSize;
        paint.IsAntialias = true;
        paint.Color = GetColorFromColorValue(color);
        paint.IsStroke = false;
        y = y + (fontSize / 2);
        _canvas.DrawText(text, (float)x, (float)y, paint);
    }

    public void DrawLine(double x1, double y1, double x2, double y2, double strokeThickness, ColorValue color)
    {
        using var paint = new SKPaint();
        paint.IsAntialias = true;
        paint.Color = GetColorFromColorValue(color);
        paint.StrokeWidth = (float)strokeThickness;
        paint.StrokeCap = SKStrokeCap.Round;
        _canvas.DrawLine((float)x1, (float)y1, (float)x2, (float)y2, paint);
    }

    public void DrawArc(double x, double y, double width, bool isArcDown)
    {
    }

    public void Clear(ColorValue backgroundColor)
    {
        _canvas.Clear();
        using var paint = new SKPaint();
        paint.Color = GetColorFromColorValue(backgroundColor);
        _canvas.DrawPaint(paint);
    }

    public void FillRectangle(double x, double y, double w, double h, ColorValue fillColor, ColorValue borderColor)
    {
        var rect = new SKRect((float)x, (float)y, (float)(x + w), (float)(y + h));
        using var paint = new SKPaint();
        paint.Color = GetColorFromColorValue(borderColor);
        _canvas.DrawRect(rect, paint);
    }

    public void FillEllipse(double x1, double y1, double w, double h, ColorValue fillColor, ColorValue borderColor)
    {
        using var paint = new SKPaint();
        paint.IsAntialias = true;

        var rect = new SKRect((float)x1, (float)y1, (float)(x1 + w), (float)(y1 + h));
        paint.Color = GetColorFromColorValue(fillColor);
        paint.IsStroke = false;
        _canvas.DrawOval(rect, paint);

        paint.Color = GetColorFromColorValue(borderColor);
        paint.IsStroke = true;
        paint.StrokeWidth = 0.5f;
        _canvas.DrawOval(rect, paint);
    }

    private static SKColor GetColorFromColorValue(ColorValue value) => new(value.R, value.G, value.B, value.A);
}
