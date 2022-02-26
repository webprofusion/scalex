using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Scalex.Avalonia.UI.Utils;

namespace Scalex.Avalonia.UI.Controls
{
    public partial class ScaleDiagram : UserControl
    {
        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer _diagramRenderer;

        private DigramRenderingDrawOp _customDrawingOp;

        public ScaleDiagram()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

           
        }

        public override void Render(DrawingContext context)
        {
            var guitarModel = ViewModels.MainViewModel.GuitarModel;

            _diagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer(guitarModel);

            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer);

            context.Custom(_customDrawingOp);

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);

            this.Width = _diagramRenderer.GetDiagramWidth()*4;
            this.Height = _diagramRenderer.GetFretboardHeight() * 2;
        }
    }
}
