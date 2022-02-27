using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Scalex.UI.Utils;
using Webprofusion.Scalex.Music;

namespace Scalex.UI.Controls
{
    public partial class ChordDiagram : UserControl
    {
        public ChordDefinition ChordDefinition { get; set; }

        private Webprofusion.Scalex.Rendering.ChordDiagramRenderer _diagramRenderer;

        private DigramRenderingDrawOp _customDrawingOp;

        public ChordDiagram()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            var guitarModel = ViewModels.MainViewModel.GuitarModel;

            _diagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer(guitarModel);

            if (ChordDefinition != null)
            {
                _diagramRenderer.CurrentChordDiagrams = guitarModel.GetChordDiagramsByGroup(ChordDefinition.ChordGroup.ToString());
            }


            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer, 1.5f);

            context.Custom(_customDrawingOp);

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);

            this.Width = _diagramRenderer.GetRequiredWidthPerChord() * _diagramRenderer.ChordsPerRow;
            this.Height = 500;
        }
    }
}
