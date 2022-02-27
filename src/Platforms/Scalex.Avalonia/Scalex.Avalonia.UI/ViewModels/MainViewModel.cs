using System;
using System.Collections.Generic;
using System.Text;
using Webprofusion.Scalex.Guitar;

namespace Scalex.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string AppTitle => "Scalex Guitar Toolkit";

        private static GuitarModel? _guitarModel;
        public static GuitarModel GuitarModel { get { return _guitarModel ?? (_guitarModel = new GuitarModel()); } }

        public MainViewModel()
        {
            GuitarModel.IsMultiScale = false;
            GuitarModel.GuitarModelSettings.EnableDisplacedFingeringMarkers = true;
            GuitarModel.FretMarkerStyle = FretMarkerStyle.Dots;
            GuitarModel.EnableNoteNames = false;
            GuitarModel.GuitarModelSettings.EnableDiagramNoteSequence = true;
           // GuitarModel.GuitarModelSettings.EnableFretboardBackgroundFill = true;

        }
    }
}
