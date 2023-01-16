using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;

namespace Scalex.UI.Utils
{
    class DigramRenderingDrawOp : ICustomDrawOperation
    {
        private Webprofusion.Scalex.Rendering.Generic2DRenderer _diagramRenderer;
        private float _scale = 3;

        public DigramRenderingDrawOp(Rect bounds, Webprofusion.Scalex.Rendering.Generic2DRenderer diagramRenderer, float scale = 3)
        {
            Bounds = bounds;
            _diagramRenderer = diagramRenderer;
            _scale = scale;
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
            var leaseFeature = context.GetFeature<ISkiaSharpApiLeaseFeature>();
            if (leaseFeature == null)
                return;
            using var lease = leaseFeature.Lease();
            var canvas = lease.SkCanvas;

            if (canvas != null)
            {
                // draw stuff 

                var skiaDrawingSurface = new SkiaDrawingSurface(canvas);

                skiaDrawingSurface.SetScale(_scale);

                _diagramRenderer.Render(skiaDrawingSurface);
            }
        }
    }
}
