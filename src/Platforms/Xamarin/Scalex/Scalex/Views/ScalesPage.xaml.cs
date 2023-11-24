﻿using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Webprofusion.Scalex.Guitar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScalesPage : ContentPage
    {
        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer scaleDiagramRenderer;
        private GuitarModel guitarModel = new GuitarModel();

        public ScalesPage()
        {
            InitializeComponent();
            scaleDiagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer(guitarModel);
            /*scaleDiagramRenderer.GuitarModel.GuitarModelSettings.EnableDiagramTitle = false;
            scaleDiagramRenderer.GuitarModel.IsMultiScale = false;
            scaleDiagramRenderer.GuitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers = false;
            */
            //scaleDiagramRenderer.GuitarModel.GuitarModelSettings.EnableFretboardBackgroundFill = true;

            //theme
            /* var resource = Application.Current.Resources;

             scaleDiagramRenderer.ColorPalette[ThemeColorPreset.ForegroundText] = new Webprofusion.Scalex.Util.ColorValue(255, 255, 255);

             scaleDiagramRenderer.ColorPalette[ThemeColorPreset.Foreground] = new Webprofusion.Scalex.Util.ColorValue(255, 255, 255);

             scaleDiagramRenderer.ColorPalette[ThemeColorPreset.Background] = new Webprofusion.Scalex.Util.ColorValue(0, 0, 0);*/

            /* ColorPalette.Add(ThemeColorPreset.ForegroundText, new ColorValue(0, 0, 0));
             ColorPalette.Add(ThemeColorPreset.MutedForeground, new ColorValue(128, 128, 128));
             ColorPalette.Add(ThemeColorPreset.Background, new ColorValue(255, 255, 255));
             ColorPalette.Add(ThemeColorPreset.MutedBackground, new ColorValue(0, 0, 0));
             ColorPalette.Add(ThemeColorPreset.Subtle, new ColorValue(164, 164, 164));
             ColorPalette.Add(ThemeColorPreset.Accent, new ColorValue(0xb4, 0x55, 0xb6));*/

            SkiaDrawingSurface.ApplyThemeColours(scaleDiagramRenderer);

            //setup list of tunings

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
            this.Title = $"Scales - {guitarModel.SelectedKey} {guitarModel.SelectedScale.Name}";
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var skiaDrawingSurface = new SkiaDrawingSurface(surface.Canvas);

            var diagramRequiredWidth = scaleDiagramRenderer.GetDiagramWidth();
            var canvasWidth = info.Width;
            float scaleFactor = (float)canvasWidth / (float)diagramRequiredWidth;
            if (scaleFactor < 2) scaleFactor = 2;

            skiaDrawingSurface.SetScale((float)scaleFactor);

            canvas.Clear();

            scaleDiagramRenderer.Render(skiaDrawingSurface);
        }

        private void tuningPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tuningPicker.SelectedIndex > -1)
            {
                var tuning = (Webprofusion.Scalex.Guitar.GuitarTuning)tuningPicker.SelectedItem;
                guitarModel.SetTuning(tuning.ID);
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }

        private void scalePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scalePicker.SelectedIndex > -1)
            {
                var scale = (Webprofusion.Scalex.Music.ScaleItem)scalePicker.SelectedItem;
                guitarModel.SetScale(scale.ID);
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }

        private void keyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keyPicker.SelectedIndex > -1)
            {
                guitarModel.SetKey(keyPicker.SelectedItem.ToString());
                SkiaCanvas.InvalidateSurface();
                SetPageTitle();
            }
        }
    }
}