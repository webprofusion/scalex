using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Scalex.UI.Utils;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scalex.UI.Controls
{
    public partial class TabDiagram : UserControl
    {
        List<SKImage> partialImages = new List<SKImage>();

        public TabDiagram()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            LoadAndRender();
        }

        public override void Render(Avalonia.Media.DrawingContext context)
        {
         
           // base.Render(context);

            context.Custom(new ImageCustomDrawingOp(new Rect(0, 0, Bounds.Width, Bounds.Height), partialImages));
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }

        private void LoadAndRender()
        {

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
           
            var stream = assets.Open(new Uri("avares://Scalex.UI/Assets/TestFiles/Tab.gp4"));
            byte[] scoreBytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                scoreBytes= memoryStream.ToArray();
            }
            var score = AlphaTab.Importer.ScoreLoader.LoadScoreFromBytes(scoreBytes);
            var track = score.Tracks[1];
            // render score with svg engine and desired rendering width
            var settings = new AlphaTab.Settings();
            settings.Display.Scale = 0.8;


            //settings.Core.Engine = "skia";
            var renderer = new AlphaTab.Rendering.ScoreRenderer(settings)
            {
                Width = 970
            };

            var totalWidth = 0;
            var totalHeight = 0;

            renderer.PartialRenderFinished.On(r => { partialImages.Add((SKImage)r.RenderResult); });
            renderer.RenderFinished.On(r =>
            {
                totalWidth = (int)r.TotalWidth;
                totalHeight = (int)r.TotalHeight;

                this.Height = totalHeight;
                this.Width = totalWidth;
            });
            renderer.RenderScore(score, new double[] { track.Index });
        }
    }
}
