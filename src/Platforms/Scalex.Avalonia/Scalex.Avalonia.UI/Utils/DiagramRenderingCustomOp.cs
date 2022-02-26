using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;

namespace Scalex.Avalonia.UI.Utils
{
    class DigramRenderingDrawOp : ICustomDrawOperation
    {
        private Webprofusion.Scalex.Rendering.Generic2DRenderer _diagramRenderer;
        public DigramRenderingDrawOp(Rect bounds, Webprofusion.Scalex.Rendering.Generic2DRenderer diagramRenderer)
        {
            Bounds = bounds;
            _diagramRenderer = diagramRenderer;

            SkiaDrawingSurface.ApplyThemeColours(_diagramRenderer);
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
                // draw stuff 
                var r = new Scalex.SkiaDrawingSurface(canvas);

                //canvas.Clear(new SKColor(255, 255, 255));

                canvas.Clear();


                var skiaDrawingSurface = new SkiaDrawingSurface(canvas);
                skiaDrawingSurface.SetScale(3);
                _diagramRenderer.Render(skiaDrawingSurface);

            }
        }
    }
}
