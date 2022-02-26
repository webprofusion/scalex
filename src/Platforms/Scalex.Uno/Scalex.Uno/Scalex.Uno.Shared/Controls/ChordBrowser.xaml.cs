using SkiaSharp;
using SkiaSharp.Views.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Webprofusion.Scalex.Guitar;
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
    public sealed partial class ChordBrowser : UserControl
    {
        private GuitarModel _guitarModel = new GuitarModel();
        private Webprofusion.Scalex.Rendering.ChordDiagramRenderer diagramRenderer;
        public ChordBrowser()
        {
            this.InitializeComponent();

            diagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer();

            // TODO : guitar model settings changes should cause global update to scales/chords etc
            _guitarModel.IsMultiScale = false;
            _guitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers = false;

            SkiaDrawingSurface.ApplyThemeColours(diagramRenderer);

   
            // tuning list
            this.tuningPicker.ItemsSource = _guitarModel.AllTunings;
            this.tuningPicker.SelectedIndex = 0;

            // chord types
            /*List<ChordDefinition> chordGroups = new List<ChordDefinition>();
			chordGroups.Add(new ChordDefinition(ChordGroup.Common, "Common Guitar Chords", "popular", "popular", null));
			chordGroups.AddRange(chordDiagramRenderer.GuitarModel.GetAllChordDefinitions());
			typePicker.ItemsSource = chordGroups;
			this.typePicker.SelectedIndex = 0;*/
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {


            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = e.Surface.Canvas;

            var skiaDrawingSurface = new SkiaDrawingSurface(e.Surface.Canvas);



            int requiredHeight = diagramRenderer.GetRequiredHeight(skiaDrawingSurface);
            if (this.SkiaCanvas.Height < requiredHeight)
            {

                // this.CanvasView.HeightRequest = requiredHeight;
            }

            // work out the width we need for our diagram
            var diagramRequiredWidth = diagramRenderer.GetRequiredWidthPerChord();
            var canvasWidth = info.Width;

            var chordsPerRow = canvasWidth / diagramRequiredWidth;
            if (chordsPerRow > 10) chordsPerRow = 10;
            diagramRenderer.ChordsPerRow = (int)chordsPerRow - 1;

            float scaleFactor = (float)canvasWidth / (float)(diagramRequiredWidth * chordsPerRow);
            if (scaleFactor < 1.5) scaleFactor = 1.5f;

            skiaDrawingSurface.SetScale((float)scaleFactor);


            diagramRenderer.Render(skiaDrawingSurface);
        }

        private void TuningPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tuningPicker.SelectedIndex > -1)
            {
                var tuning = (Webprofusion.Scalex.Guitar.GuitarTuning)tuningPicker.SelectedItem;
                _guitarModel.SetTuning(tuning.ID);
                SkiaCanvas.Invalidate();
                // SetPageTitle();
            }
        }
    }
}
