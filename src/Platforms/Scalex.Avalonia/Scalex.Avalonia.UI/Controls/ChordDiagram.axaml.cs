using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Scalex.UI.Utils;
using Webprofusion.Scalex.Music;

namespace Scalex.UI.Controls
{
    public partial class ChordDiagram : UserControl
    {
        public ChordDefinition ChordDefinition { get; set; }

        private Webprofusion.Scalex.Rendering.ChordDiagramRenderer _diagramRenderer;

        private DiagramRenderingDrawOp _customDrawingOp;

        public ChordDiagram()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _diagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer(ViewModels.MainViewModel.GuitarModel);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (ChordDefinition != null)
            {
                _diagramRenderer.CurrentChordDiagrams = ViewModels.MainViewModel.GuitarModel.GetChordDiagramsByGroup(ChordDefinition.ChordGroup.ToString());
            }

            if (_customDrawingOp == null)
            {
                _customDrawingOp = new DiagramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer, 1.5f);
            }

            context.Custom(_customDrawingOp);
        }
    }
}
