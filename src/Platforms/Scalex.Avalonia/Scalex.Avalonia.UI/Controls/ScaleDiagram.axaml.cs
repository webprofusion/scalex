using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Scalex.UI.Utils;
using System;
using Webprofusion.Scalex.Music;

namespace Scalex.UI.Controls
{
    public partial class ScaleDiagram : UserControl
    {
        private Webprofusion.Scalex.Rendering.ScaleDiagramRenderer _diagramRenderer;

        private DigramRenderingDrawOp _customDrawingOp;
        Rect _currentBounds;
        public ScaleDiagram()
        {
            InitializeComponent();

        }



        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);


            _diagramRenderer = new Webprofusion.Scalex.Rendering.ScaleDiagramRenderer(ViewModels.MainViewModel.GuitarModel);

            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer, 3);

            /*  AddHandler(PointerPressedEvent, (object sender, Avalonia.Input.PointerPressedEventArgs e) =>
              {

              }, Avalonia.Interactivity.RoutingStrategies.Tunnel, handledEventsToo: true);
            */
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
                //Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Render);
             
                System.Diagnostics.Debug.WriteLine($"Fret:{note.Value.FretNumber} String:{note.Value.StringNumber + 1} Note:{note.Value.Note.ToString()}");
                this.Width -= 0.0001;
            }

        }


        public override void Render(DrawingContext context)
        {
            base.Render(context);

            _diagramRenderer.SetGuitarModel(ViewModels.MainViewModel.GuitarModel);

            context.Custom(_customDrawingOp);

            //Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);

            var diagramWidth = _diagramRenderer.GetDiagramWidth() * 4;
            var diagramHeight = _diagramRenderer.GetFretboardHeight() * 2;
            if (this.Width != diagramWidth) this.Width = diagramWidth;
            if (this.Height != diagramWidth) this.Height = diagramHeight;

        }
    }
}
