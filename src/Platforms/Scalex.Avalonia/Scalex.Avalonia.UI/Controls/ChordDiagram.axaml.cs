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

            _diagramRenderer = new Webprofusion.Scalex.Rendering.ChordDiagramRenderer(ViewModels.MainViewModel.GuitarModel);
            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer, 1.5f);

        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (ChordDefinition != null)
            {
                _diagramRenderer.CurrentChordDiagrams = ViewModels.MainViewModel.GuitarModel.GetChordDiagramsByGroup(ChordDefinition.ChordGroup.ToString());
            }

            context.Custom(_customDrawingOp);
        }
    }
}
