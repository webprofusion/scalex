using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace Scalex.UI.Utils
{
    class ImageCustomDrawingOp : ICustomDrawOperation
    {
        private List<SKImage> _images;
        private SKPaint _paintQuality = new SKPaint { IsAntialias = true, FilterQuality = SKFilterQuality.High };


    public ImageCustomDrawingOp(Rect bounds, List<SKImage> images)
        {
            Bounds = bounds;
            _images = images;
        }

        public void Dispose()
        {
            // No-op in this example
        }

        public Rect Bounds { get; }
        public bool HitTest(Point p) => false;
        public bool Equals(ICustomDrawOperation other) => false;
        public void Render(IDrawingContextImpl context)
        {
            var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;
            if (canvas != null)
            {
                if (_images?.Any() == true)
                {
                    canvas.Clear(SKColor.Parse("#f0f0f0"));

                    // draw stuff 
                    int startx = 0;
                    int starty = 0;
                    foreach (var img in _images)
                    {
                        canvas.DrawImage(img, startx, starty, _paintQuality);
                        starty += img.Height;
                    }
                }
            }
        }
    }
}
