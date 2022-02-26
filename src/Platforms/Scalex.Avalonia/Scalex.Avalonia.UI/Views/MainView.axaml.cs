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

namespace Scalex.Avalonia.UI.Views
{
    public partial class MainView : UserControl
    {
      
        public MainView()
        {
            InitializeComponent();

            scaleList.Items = ViewModels.MainViewModel.GuitarModel.AllScales;
            scaleList.SelectedIndex = 0;
        }

        private void ScaleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var scale = cb.SelectedItem as Webprofusion.Scalex.Music.ScaleItem;
            ViewModels.MainViewModel.GuitarModel.SetScale(scale.ID);
            this.scaleDiagram.InvalidateVisual();
        }
    }
}