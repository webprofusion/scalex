using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChordsPage : ContentPage
    {
        private Webprofusion.Scalex.Rendering.ChordDiagramRenderer chordDiagramRenderer;

        public ChordsPage()
        {
            InitializeComponent();

            chordDiagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var skiaDrawingSurface = new SkiaDrawingSurface(surface.Canvas);

            int requiredHeight = chordDiagramRenderer.GetRequiredHeight(skiaDrawingSurface);
            if (this.CanvasView.Height < requiredHeight)
            {
                this.CanvasView.HeightRequest = requiredHeight;
            }

            var renderScale = (this.Width / 680) / 2;
            if (renderScale < 0.25) renderScale = 0.25;
            skiaDrawingSurface.SetScale((float)renderScale);
            canvas.Clear();

            /* SKPaint paint = new SKPaint
             {
                 Style = SKPaintStyle.Stroke,
                 Color = Color.Red.ToSKColor(),
                 StrokeWidth = 50
             };
             canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);
             */
            /* if (showFill)
             {
                 paint.Style = SKPaintStyle.Fill;
                 paint.Color = SKColors.Blue;
                 canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);
             }*/
            chordDiagramRenderer.Render(skiaDrawingSurface);
        }
    }
}