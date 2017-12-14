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
    public partial class ScalesPage : ContentPage
    {
        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer scaleDiagramRenderer;

        public ScalesPage()
        {
            InitializeComponent();
            scaleDiagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer();
            scaleDiagramRenderer.GuitarModel.GuitarModelSettings.EnableDiagramTitle = false;
            //setup list of tunings
            var guitarModel = scaleDiagramRenderer.GuitarModel;

            this.tuningPicker.ItemsSource = guitarModel.AllTunings;
            this.tuningPicker.SelectedIndex = 0;

            this.scalePicker.ItemsSource = guitarModel.AllScales;
            this.scalePicker.SelectedIndex = 0;

            this.keyPicker.ItemsSource = guitarModel.AllKeys;
            this.keyPicker.SelectedIndex = 0;

            SetPageTitle();
        }

        private void SetPageTitle()
        {
            var guitarModel = this.scaleDiagramRenderer.GuitarModel;
            this.Title = $"Scales - {guitarModel.SelectedKey} {guitarModel.SelectedScale.Name}";
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            var skiaDrawingSurface = new SkiaDrawingSurface(surface.Canvas);
            var renderScale = (this.Width / 680) / 2;
            if (renderScale < 0.25) renderScale = 0.25;
            skiaDrawingSurface.SetScale((float)renderScale);

            canvas.Clear();

            scaleDiagramRenderer.Render(skiaDrawingSurface);
        }

        private void tuningPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tuningPicker.SelectedIndex > -1)
            {
                var tuning = (Webprofusion.Scalex.Guitar.GuitarTuning)tuningPicker.SelectedItem;
                this.scaleDiagramRenderer.GuitarModel.SetTuning(tuning.ID);
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }

        private void scalePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scalePicker.SelectedIndex > -1)
            {
                var scale = (Webprofusion.Scalex.Music.ScaleItem)scalePicker.SelectedItem;
                this.scaleDiagramRenderer.GuitarModel.SetScale(scale.ID);
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }

        private void keyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keyPicker.SelectedIndex > -1)
            {
                this.scaleDiagramRenderer.GuitarModel.SetKey(keyPicker.SelectedItem.ToString());
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }
    }
}