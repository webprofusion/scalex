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

            _customDrawingOp = new DigramRenderingDrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _diagramRenderer);

            AddHandler(PointerPressedEvent, (object sender, Avalonia.Input.PointerPressedEventArgs e) => {
                var point = e.GetCurrentPoint(this);
                Webprofusion.Scalex.Rendering.ScaleDiagramRenderer.NoteItem? note = _diagramRenderer.GetNoteAtPoint(point.Position.X, point.Position.Y);

                if (note != null)
                {
                    System.Diagnostics.Debug.WriteLine(note);
                }

            }, Avalonia.Interactivity.RoutingStrategies.Tunnel, handledEventsToo:true);

        }

        protected override void OnPointerPressed(Avalonia.Input.PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

          
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
