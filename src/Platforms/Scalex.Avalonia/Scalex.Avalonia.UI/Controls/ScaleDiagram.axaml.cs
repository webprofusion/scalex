using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Scalex.UI.Utils;

namespace Scalex.UI.Controls
{
    public partial class ScaleDiagram : UserControl
    {
        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer? _diagramRenderer;

        private DiagramRenderingDrawOp? _customDrawingOp;

        public ScaleDiagram()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _diagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer(ViewModels.MainViewModel.GuitarModel);

            _customDrawingOp = new DiagramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer, 3);


        }

        protected override void OnPointerPressed(Avalonia.Input.PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            var point = e.GetCurrentPoint(this);
            var pos = e.GetPosition(this);

            float scaling = 3;

            var pointerX = point.Position.X / scaling;
            var pointerY = point.Position.Y / scaling;
            var note = _diagramRenderer.GetNoteAtPoint(pointerX, pointerY);

            if (note != null)
            {
                _diagramRenderer.HighlightNote(note.Value);

                System.Diagnostics.Debug.WriteLine($"Fret:{note.Value.FretNumber} String:{note.Value.StringNumber + 1} Note:{note.Value.Note.ToString()}");
            }

        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            _diagramRenderer.SetGuitarModel(ViewModels.MainViewModel.GuitarModel);

            context.Custom(_customDrawingOp);
        }
    }
}
