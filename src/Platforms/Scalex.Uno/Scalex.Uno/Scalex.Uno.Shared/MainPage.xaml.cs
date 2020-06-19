using SkiaSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

#if WINDOWS_UWP
using SkiaSharp.Views.UWP;
#endif


#if ANDROID
using SkiaSharp.Views.Android;
#endif


#if __WASM__
using SkiaSharp.Views.UWP;
#endif
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Scalex.Uno
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private Webprofusion.Scalex.Rendering.ChordDiagramRenderer chordDiagramRenderer;

		public MainPage()
        {
            this.InitializeComponent();

			chordDiagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer();

			SkiaDrawingSurface.ApplyThemeColours(chordDiagramRenderer);

			var guitarModel = chordDiagramRenderer.GuitarModel;

			// tuning list
			/*this.tuningPicker.ItemsSource = guitarModel.AllTunings;
			this.tuningPicker.SelectedIndex = 0;

			// chord types
			List<ChordDefinition> chordGroups = new List<ChordDefinition>();
			chordGroups.Add(new ChordDefinition(ChordGroup.Common, "Common Guitar Chords", "popular", "popular", null));
			chordGroups.AddRange(chordDiagramRenderer.GuitarModel.GetAllChordDefinitions());
			typePicker.ItemsSource = chordGroups;
			this.typePicker.SelectedIndex = 0;*/

		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			/*
			// the the canvas and properties
			var canvas = e.Surface.Canvas;

			// get the screen density for scaling
			var display = DisplayInformation.GetForCurrentView();
			var scale = display.LogicalDpi / 96.0f;
			var scaledSize = new SKSize(e.Info.Width / scale, e.Info.Height / scale);

			// handle the device screen density
			canvas.Scale(scale);

			// make sure the canvas is blank
			canvas.Clear(SKColors.White);

			// draw some text
			var paint = new SKPaint
			{
				Color = SKColors.Black,
				IsAntialias = true,
				Style = SKPaintStyle.Fill,
				TextAlign = SKTextAlign.Center,
				TextSize = 24
			};
			var coord = new SKPoint(scaledSize.Width / 2, (scaledSize.Height + paint.TextSize) / 2);
			canvas.DrawText("SkiaSharp", coord, paint);
			*/
			///////////////

			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = e.Surface.Canvas;

			var skiaDrawingSurface = new SkiaDrawingSurface(e.Surface.Canvas);

			int requiredHeight = chordDiagramRenderer.GetRequiredHeight(skiaDrawingSurface);
			if (this.SkiaCanvas.Height < requiredHeight)
			{
				// this.CanvasView.HeightRequest = requiredHeight;
			}

			// work out the width we need for our diagram
			var diagramRequiredWidth = chordDiagramRenderer.GetRequiredWidthPerChord();
			var canvasWidth = info.Width;

			var chordsPerRow = canvasWidth / diagramRequiredWidth;
			if (chordsPerRow > 10) chordsPerRow = 10;
			chordDiagramRenderer.ChordsPerRow = (int)chordsPerRow - 1;

			float scaleFactor = (float)canvasWidth / (float)(diagramRequiredWidth * chordsPerRow);
			if (scaleFactor < 1.5) scaleFactor = 1.5f;

			skiaDrawingSurface.SetScale((float)scaleFactor);

			canvas.Clear();

			chordDiagramRenderer.Render(skiaDrawingSurface);
		}
	}
}
