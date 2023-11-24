using System;
using Webprofusion.Scalex.Guitar;
using Webprofusion.Scalex.Music;

namespace Webprofusion.Scalex
{
    public enum NoteMarkerDisplayMode
    {
        NoLabel,
        NoteName,
        NoteSequence,
        ScaleInterval,
        Fingering
    }
    /// <summary>
    /// Summary description for CurrentAppSettings. 
    /// </summary>
    public class PrefSettings
    {

        public int StringSpacing { get; set; }
        public int NumberFrets { get; set; }
        public static int MaxNumberFrets = 26;
        public int MarkerSize { get; set; }
        public float ZoomLevelDefault { get; set; }
        public String BackgroundImageFileName { get; set; }
        public bool IsDebugMode { get; set; }
        public bool EnableDiagramBackgroundImage { get; set; }

        public bool EnableFretboardBackgroundFill { get; set; }
        public bool EnableFretNumbers { get; set; }

        public bool EnableDiagramFingering { get; set; }
        public bool EnableDiagramStrings { get; set; }
        public bool EnableDiagramNoteNamesSharp { get; set; }
        public bool EnableDiagramHighQuality { get; set; }
        public bool EnableDisplacedFingeringMarkers { get; set; }
        public bool EnableNoteColours { get; set; }
        public bool EnableDiagramTitle { get; set; }
        public NoteMarkerDisplayMode NoteMarkerDisplayMode { get; set; }
        public TuningManager TuningManager { get; set; }
        public GuitarTuning CurrentTuning { get; set; }
        public ScaleManager ScaleManager { get; set; }

        public PrefSettings()
        {

            StringSpacing = 10;
            NumberFrets = 24;
            MarkerSize = 8;
            ZoomLevelDefault = 2F;
            BackgroundImageFileName = "";
            IsDebugMode = true;
            EnableDiagramBackgroundImage = false;
            EnableFretboardBackgroundFill = false;
            EnableFretNumbers = true;
            EnableDiagramFingering = false;
            EnableDiagramStrings = true;
            NoteMarkerDisplayMode = NoteMarkerDisplayMode.NoteName;
            EnableDiagramHighQuality = true;
            EnableNoteColours = true;
            EnableDisplacedFingeringMarkers = true;
            EnableDiagramTitle = true;

            TuningManager = new TuningManager();
            CurrentTuning = new GuitarTuning();
            ScaleManager = new ScaleManager();
        }

#if !SILVERLIGHT && !NETFX_CORE && !PCL
        /// <summary>
        /// Save current appsettings to AppSettings.xml 
        /// </summary>
    /*   public void SaveSettings()
        {
            TextWriter myStreamWriter = new StreamWriter("AppSettings.xml");
            XmlSerializer mySerializer = new XmlSerializer(typeof(CurrentAppSettings));
            mySerializer.Serialize(myStreamWriter, this);
            myStreamWriter.Close();
            }
        */

        /// <summary>
        /// Load app settings from ApSettings.xml 
        /// </summary>
        /// <returns></returns>
        public static PrefSettings LoadSettings()
        {
            PrefSettings myNewAppSettings = null;

            try
            {
                //TODO: reinstate settings loading
                /*
				TextReader myStreamReader = new StreamReader("AppSettings.xml");
				XmlSerializer mySerializer = new XmlSerializer (typeof(CurrentAppSettings));
				myNewAppSettings=(CurrentAppSettings) mySerializer.Deserialize(myStreamReader);
				myStreamReader.Close();
                 * */
            }
            catch (Exception)
            {
                myNewAppSettings = null;
            }

            return myNewAppSettings;
        }

#endif
    }
}