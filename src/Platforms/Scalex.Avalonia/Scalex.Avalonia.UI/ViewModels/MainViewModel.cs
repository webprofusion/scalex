using System.Collections.Generic;
using System.Diagnostics;
using Webprofusion.Scalex;
using Webprofusion.Scalex.Guitar;

namespace Scalex.UI.ViewModels
{

    public class MarkerLabelStyle
    {
        public string Name { get; set; }
        public NoteMarkerDisplayMode Value { get; set; }
    }

    public class MainViewModel : ViewModelBase
    {
        public string AppTitle => "Scalex Guitar Toolkit";

        private static GuitarModel? _guitarModel;
        public static GuitarModel GuitarModel { get { return _guitarModel ?? (_guitarModel = new GuitarModel()); } }

        public static List<MarkerLabelStyle> MarkerLabelStyles { get; set; } = new List<MarkerLabelStyle> {
            new MarkerLabelStyle { Name = "None", Value = NoteMarkerDisplayMode.NoLabel },
            new MarkerLabelStyle { Name = "Degree", Value = NoteMarkerDisplayMode.ScaleInterval },
            new MarkerLabelStyle { Name = "Sequence", Value = NoteMarkerDisplayMode.NoteSequence },
            new MarkerLabelStyle { Name = "Note Name", Value = NoteMarkerDisplayMode.NoteName }
            };
                                                                                                                         
        public MainViewModel()
        {
            GuitarModel.IsMultiScale = false;
            GuitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers = true;
            GuitarModel.FretMarkerStyle = FretMarkerStyle.Dots;
        }

    }
}
