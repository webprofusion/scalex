using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Webprofusion.Scalex.Rendering;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Scalex.Uno.Shared.Controls
{
    public sealed partial class ScaleBrowser : UserControl
    {

        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer diagramRenderer;
        public ScaleBrowser()
        {
            this.InitializeComponent();

            diagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer();

            // TODO : guitar model settings changes should cause global update to scales/chords etc
            diagramRenderer.GuitarModel.IsMultiScale = false;
            diagramRenderer.GuitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers = false;


            SkiaDrawingSurface.ApplyThemeColours(diagramRenderer);

            var guitarModel = diagramRenderer.GuitarModel;

            // tuning list
            this.tuningPicker.ItemsSource = guitarModel.AllTunings;
            this.tuningPicker.SelectedIndex = 0;

            // scale list
            this.scalePicker.ItemsSource = guitarModel.AllScales;
            this.scalePicker.SelectedIndex = 0;
        }

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			

			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = e.Surface.Canvas;

			var skiaDrawingSurface = new SkiaDrawingSurface(e.Surface.Canvas);


			// get the screen density for scaling
			var display = DisplayInformation.GetForCurrentView();
			var scale = display.LogicalDpi / 96.0f;
			var scaledSize = new SKSize(e.Info.Width / scale, e.Info.Height / scale);

			// handle the device screen density
			//canvas.Scale(scale);

            var diagramRequiredWidth = diagramRenderer.GetDiagramWidth();
            var canvasWidth = info.Width;
            float scaleFactor = (float)canvasWidth / (float)diagramRequiredWidth;
            if (scaleFactor < 2) scaleFactor = 2;

            scaleFactor = 3;
    
            skiaDrawingSurface.SetScale((float)scaleFactor);

            canvas.Clear();

            diagramRenderer.Render(skiaDrawingSurface);
        }


        private void TuningPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tuningPicker.SelectedIndex > -1)
            {
                var tuning = (Webprofusion.Scalex.Guitar.GuitarTuning)tuningPicker.SelectedItem;
                this.diagramRenderer.GuitarModel.SetTuning(tuning.ID);
                SkiaCanvas.Invalidate();
                // SetPageTitle();
            }
        }

        private void ScalePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scalePicker.SelectedIndex > -1)
            {
                var scale = (Webprofusion.Scalex.Music.ScaleItem)scalePicker.SelectedItem;
                this.diagramRenderer.GuitarModel.SetScale(scale.ID);
                SkiaCanvas.Invalidate();
                //SetPageTitle();
            }
        }
    }
}
