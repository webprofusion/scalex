using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
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

            var guitarModel = chordDiagramRenderer.GuitarModel;

            this.tuningPicker.ItemsSource = guitarModel.AllTunings;
            this.tuningPicker.SelectedIndex = 0;

            this.typePicker.ItemsSource = new Webprofusion.Scalex.Music.ChordManager().ChordDefinitions;
            this.typePicker.SelectedIndex = 0;

            this.keyPicker.ItemsSource = guitarModel.AllKeys;
            this.keyPicker.SelectedIndex = 0;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var skiaDrawingSurface = new SkiaDrawingSurface(surface.Canvas);

            int requiredHeight = chordDiagramRenderer.GetRequiredHeight(skiaDrawingSurface);
            if (this.SkiaCanvas.Height < requiredHeight)
            {
                // this.CanvasView.HeightRequest = requiredHeight;
            }

            var renderScale = (this.Width / 680) / 2;
            if (renderScale < 1.5) renderScale = 1.5;
            skiaDrawingSurface.SetScale((float)renderScale);
            canvas.Clear();

            //scale chords per row depending on available width
            var chordsPerRow = (this.SkiaCanvas.Width / renderScale) / (chordDiagramRenderer.GetRequiredWidthPerChord() * renderScale);
            chordDiagramRenderer.ChordsPerRow = (int)chordsPerRow;

            chordDiagramRenderer.Render(skiaDrawingSurface);
        }

        private void tuningPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tuningPicker.SelectedIndex > -1)
            {
                var tuning = (Webprofusion.Scalex.Guitar.GuitarTuning)tuningPicker.SelectedItem;
                this.chordDiagramRenderer.GuitarModel.SetTuning(tuning.ID);
                this.SkiaCanvas.InvalidateSurface();
                // SetPageTitle();
            }
        }

        private void typePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typePicker.SelectedIndex > -1)
            {
                var chordType = (Webprofusion.Scalex.Music.ScaleItem)typePicker.SelectedItem;
                this.chordDiagramRenderer.CurrentChordDiagrams = this.chordDiagramRenderer.GuitarModel.GetChordDiagramsByGroup("major");
                SkiaCanvas.InvalidateSurface();
                // SetPageTitle();
            }
        }

        private void keyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keyPicker.SelectedIndex > -1)
            {
                this.chordDiagramRenderer.GuitarModel.SetKey(keyPicker.SelectedItem.ToString());
                SkiaCanvas.InvalidateSurface();
            }
        }
    }
}