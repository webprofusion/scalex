using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;
using Avalonia.Skia;
using SkiaSharp;
using Webprofusion.Scalex.Util;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;
using Webprofusion.Scalex.Music;

namespace Scalex.UI.Views
{
    public partial class MainView : UserControl
    {
      
        public MainView()
        {
            InitializeComponent();

            scaleList.Items = ViewModels.MainViewModel.GuitarModel.AllScales;
            scaleList.SelectedIndex = 0;

            tuningList.Items = ViewModels.MainViewModel.GuitarModel.AllTunings;
            tuningList.SelectedIndex = 0;

            keyList.Items = ViewModels.MainViewModel.GuitarModel.AllKeys;
            keyList.SelectedIndex = 0;

            chordGroups.Items = ViewModels.MainViewModel.GuitarModel.GetAllChordDefinitions();
            chordGroups.SelectedIndex = 0;
        }

        private void ScaleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var scale = cb.SelectedItem as Webprofusion.Scalex.Music.ScaleItem;
            ViewModels.MainViewModel.GuitarModel.SetScale(scale.ID);
            this.scaleDiagram.InvalidateVisual();
        }

        private void TuningList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var tuning = cb.SelectedItem as Webprofusion.Scalex.Guitar.GuitarTuning;
            ViewModels.MainViewModel.GuitarModel.SetTuning(tuning.ID);
            this.scaleDiagram.InvalidateVisual();
        }

        private void KeyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var key = cb.SelectedItem as string;
            ViewModels.MainViewModel.GuitarModel.SetKey(key);
            this.scaleDiagram.InvalidateVisual();
        }

        private void ChordGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            ChordDefinition def = cb.SelectedItem as ChordDefinition;
            chordDiagram.ChordDefinition = def;
            this.chordDiagram.InvalidateVisual();
        }
    }
}