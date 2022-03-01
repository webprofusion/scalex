using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Scalex.UI.Utils;

namespace Scalex.UI.Controls
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

 
            _diagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer(ViewModels.MainViewModel.GuitarModel);

            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer);

        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            _diagramRenderer.SetGuitarModel(ViewModels.MainViewModel.GuitarModel);

            context.Custom(_customDrawingOp);

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);

            var diagramWidth = _diagramRenderer.GetDiagramWidth() * 4; 
            var diagramHeight = _diagramRenderer.GetFretboardHeight() * 2;
            if (this.Width != diagramWidth) this.Width = diagramWidth;
            if (this.Height != diagramWidth) this.Height = diagramHeight;
     
        }
    }
}
