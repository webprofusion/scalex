using AlphaTab;
using AlphaTab.Rendering;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Scalex.UI.Utils;
using Scalex.Utils;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scalex.UI.Controls
{
    public partial class TabDiagram : UserControl
    {
        List<SKImage> partialImages = new List<SKImage>();

        bool _isScoreLoaded = false;
        bool _isRenderInProgress = false;

        Rect _currentBounds;
        AlphaTab.Model.Score _score;

        public TabDiagram()
        {
            this.UseLayoutRounding = true;
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);


            this.isFirstPass = true;

            this.LayoutUpdated += TabDiagram_LayoutUpdated;
        }

        private void TabDiagram_LayoutUpdated(object sender, EventArgs e)
        {
            // only perform render if bounds have changed
            if (_currentBounds != null && !this.Bounds.Equals(_currentBounds) && this.Bounds.Width != _currentBounds.Width)
            {
                _currentBounds = this.Bounds;

            }

            if (_currentBounds == null)
            {
                _currentBounds = this.Bounds;
            }
        }

        bool isFirstPass = true;
        public override void Render(Avalonia.Media.DrawingContext context)
        {
          

            base.Render(context);

   

            //if (!_isRenderInProgress && !isFirstPass)
            // {
            if (partialImages?.Any() == true)
                {
                    context.Custom(new ImageCustomDrawingOp(new Rect(0, 0, Bounds.Width, Bounds.Height), partialImages));
                }
           // }

            if (isFirstPass)
            {
                isFirstPass = false;

                Dispatcher.UIThread.InvokeAsync(LoadScore, DispatcherPriority.Background);
            }
        }

        private async Task<AlphaTab.Model.Score> GetPopularScore()
        {
            var scoreService = new ScoreServiceManager();
            var list = await scoreService.FetchSongsMostViewed();

            var firstSong = list.First();

            var songDetails = await scoreService.FetchSongDetailsAsync(firstSong.ID);
            var score = await scoreService.FetchSongScore(songDetails, true);
            return score;
        }

        private async Task<AlphaTab.Model.Score> GetTestScore()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var stream = assets.Open(new Uri("avares://Scalex.UI/Assets/TestFiles/bends.gp4"));
            byte[] scoreBytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                scoreBytes = memoryStream.ToArray();
            }
            var score = AlphaTab.Importer.ScoreLoader.LoadScoreFromBytes(scoreBytes);
            return score;
        }

        private async Task LoadScore()
        {
            _isRenderInProgress = true;
            var trackIndex = 0;

            if (!_isScoreLoaded)
            {
                _isScoreLoaded = true;
                _score = await GetTestScore();
                //_score = await GetPopularScore();
            }

            if (_score != null)
            {
                var track = _score.Tracks[trackIndex];

                // render score with svg engine and desired rendering width
                var settings = new AlphaTab.Settings();
                settings.Core.Engine = "skia";
                settings.Display.Scale = 1;

                var renderer = new AlphaTab.Rendering.ScoreRenderer(settings)
                {
                    Width = 970// this.Bounds.Width
                };


                var totalWidth = 0;
                var totalHeight = 0;

             

                renderer.PreRender.On(isResize =>
                {
                   
                    totalWidth = 0;
                    totalHeight = 0;
                });

                renderer.Error.On(e =>
                {

                    System.Diagnostics.Debug.WriteLine(e);

                });
                renderer.PartialLayoutFinished.On(e =>
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    // request this render result when finished.
                    renderer.RenderResult(e.Id);
                });
                renderer.PartialRenderFinished.On(r =>
                {
                    if (r != null)
                    {
                        partialImages.Add((SKImage)r.RenderResult);
                    }
                });
                renderer.RenderFinished.On(r =>
                {
                    totalWidth = (int)r.TotalWidth;
                    totalHeight = (int)r.TotalHeight;

                    this.Height = totalHeight;
                    this.Width = totalWidth;
                    _isRenderInProgress = false;
                });
                renderer.RenderScore(_score, new double[] { track.Index });
            }


        }
    }
}
