using Avalonia;
using Avalonia.Controls;
using Scalex.UI.ViewModels;
using System.Linq;
using Webprofusion.Scalex;
using Webprofusion.Scalex.Music;

namespace Scalex.UI.Views
{
    public partial class MainView : UserControl
    {

        public AppSettings _appSettings { get; set; } = new AppSettings();
        public MainView()
        {
            InitializeComponent();

            _suspendSettingsChanges = true;
            scaleList.ItemsSource = MainViewModel.GuitarModel.AllScales;
            scaleList.SelectedIndex = 0;

            arpScaleList.ItemsSource = MainViewModel.GuitarModel.GetAllChordDefinitions();
            arpScaleList.SelectedIndex = 0;


            tuningList.ItemsSource = MainViewModel.GuitarModel.AllTunings;
            tuningList.SelectedIndex = 0;

            keyList.ItemsSource = MainViewModel.GuitarModel.AllKeys;
            keyList.SelectedIndex = 0;

            arpKeyList.ItemsSource = MainViewModel.GuitarModel.AllKeys;
            arpKeyList.SelectedIndex = 0;

            chordGroups.ItemsSource = MainViewModel.GuitarModel.GetAllChordDefinitions();
            chordGroups.SelectedIndex = 0;

            markerModeList.ItemsSource = System.Enum.GetValues(typeof(NoteMarkerDisplayMode));
            markerModeList.SelectedIndex = 0;

            numberOfFrets.ItemsSource = new int[] { 12, 22, 24, 26 };
            markerModeList.SelectedIndex = 0;

            LoadSettings();
            _suspendSettingsChanges = false;
            ApplySettings();
        }

        bool _suspendSettingsChanges = false;
        public void LoadSettings()
        {

            var appSettingsProvider = ((Scalex.UI.App)Application.Current).GetSettingsProvider();
            _appSettings = appSettingsProvider.LoadSettings();

        }

        public void ApplySettings()
        {
            if (_appSettings != null)
            {

                MainViewModel.GuitarModel.GuitarModelSettings.NoteMarkerDisplayMode = Webprofusion.Scalex.NoteMarkerDisplayMode.NoteName;

                MainViewModel.GuitarModel.SetNumberOfFrets(12);

                if (_appSettings.SelectedScale != null)
                {
                    MainViewModel.GuitarModel.SetScale((int)_appSettings.SelectedScale);
                    scaleList.SelectedItem = MainViewModel.GuitarModel.SelectedScale;
                }
                if (_appSettings.SelectedTuning != null)
                {
                    MainViewModel.GuitarModel.SetTuning((int)_appSettings.SelectedTuning);
                    tuningList.SelectedItem = MainViewModel.GuitarModel.SelectedTuning;
                }
                if (_appSettings.SelectedKey != null)
                {
                    MainViewModel.GuitarModel.SetKey(_appSettings.SelectedKey);
                    keyList.SelectedItem = MainViewModel.GuitarModel.SelectedKey;
                }

                if (_appSettings.SelectedArpeggioKey != null)
                {
                    arpKeyList.SelectedItem = arpKeyList.Items.FirstOrDefault(x => (string)x == _appSettings.SelectedArpeggioKey);
                }
                if (_appSettings.SelectedArpeggio != null)
                {
                    arpScaleList.SelectedItem = arpScaleList.Items.FirstOrDefault(x => ((ChordDefinition)x).ID == _appSettings.SelectedArpeggio);
                }
            }
        }

        public void SaveSettings()
        {
            if (!_suspendSettingsChanges)
            {
                var appSettingsProvider = ((Scalex.UI.App)Application.Current).GetSettingsProvider();
                appSettingsProvider.SaveSettings(_appSettings);
            }
        }

        private void MainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabControl?.SelectedIndex == null) return;

            if (mainTabControl.SelectedIndex == 0)
            {
                var scale = scaleList.SelectedItem as Webprofusion.Scalex.Music.ScaleItem;
                MainViewModel.GuitarModel.SetScale(scale.ID);
                var key = keyList.SelectedItem as string;
                MainViewModel.GuitarModel.SetKey(key);
                this.scaleDiagram.InvalidateVisual();
            }
            else if (mainTabControl.SelectedIndex == 1)
            {
                var arpeggio = arpScaleList.SelectedItem as Webprofusion.Scalex.Music.ChordDefinition;
                MainViewModel.GuitarModel.SetScale(arpeggio);
                var key = arpKeyList.SelectedItem as string;
                MainViewModel.GuitarModel.SetKey(key);
                this.arpeggioDiagram.InvalidateVisual();
            }
            else if (mainTabControl.SelectedIndex == 2)
            { }
        }

        private void MarkerMode_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var markerMode = (NoteMarkerDisplayMode)markerModeList.SelectedItem;

            MainViewModel.GuitarModel.GuitarModelSettings.NoteMarkerDisplayMode = markerMode;
        }

        private void Frets_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            var frets = numberOfFrets.SelectedItem as int?;

            if (frets != null)
            {
                MainViewModel.GuitarModel.SetNumberOfFrets((int)frets);

            }
        }

        private void ScaleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (mainTabControl.SelectedIndex == 0)
            {
                var cb = e.Source as ComboBox;
                var scale = cb.SelectedItem as Webprofusion.Scalex.Music.ScaleItem;
                MainViewModel.GuitarModel.SetScale(scale.ID);
                this.scaleDiagram.InvalidateVisual();

                if (!_suspendSettingsChanges)
                {
                    _appSettings.SelectedScale = scale.ID;
                    this.SaveSettings();
                }
            }
        }

        private void ArpeggioScaleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabControl.SelectedIndex == 1)
            {
                var cb = e.Source as ComboBox;
                var arpeggio = cb.SelectedItem as Webprofusion.Scalex.Music.ChordDefinition;

                MainViewModel.GuitarModel.SetScale(arpeggio);
                this.arpeggioDiagram.InvalidateVisual();

                if (!_suspendSettingsChanges)
                {
                    _appSettings.SelectedArpeggio = arpeggio.ID;
                    this.SaveSettings();
                }
            }
        }

        private void TuningList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var tuning = cb.SelectedItem as Webprofusion.Scalex.Guitar.GuitarTuning;
            MainViewModel.GuitarModel.SetTuning(tuning.ID);
            this.scaleDiagram.InvalidateVisual();


            if (!_suspendSettingsChanges)
            {
                _appSettings.SelectedTuning = tuning.ID;
                this.SaveSettings();
            }
        }

        private void KeyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var key = cb.SelectedItem as string;
            MainViewModel.GuitarModel.SetKey(key);
            this.scaleDiagram.InvalidateVisual();


            if (!_suspendSettingsChanges)
            {
                _appSettings.SelectedKey = key;
                this.SaveSettings();
            }
        }

        private void ArpeggioKeyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            var key = cb.SelectedItem as string;
            MainViewModel.GuitarModel.SetKey(key);
            this.arpeggioDiagram.InvalidateVisual();

            if (!_suspendSettingsChanges)
            {
                _appSettings.SelectedArpeggioKey = key;
                this.SaveSettings();
            }
        }

        private void ChordGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = e.Source as ComboBox;
            ChordDefinition def = cb.SelectedItem as ChordDefinition;
            chordDiagram.ChordDefinition = def;
            this.chordDiagram.InvalidateVisual();

            if (!_suspendSettingsChanges)
            {
                this.SaveSettings();
            }

        }
    }
}