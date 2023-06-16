using Avalonia;
using Avalonia.Controls;
using Scalex.UI.ViewModels;
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

            tuningList.ItemsSource = MainViewModel.GuitarModel.AllTunings;
            tuningList.SelectedIndex = 0;

            keyList.ItemsSource = MainViewModel.GuitarModel.AllKeys;
            keyList.SelectedIndex = 0;

            chordGroups.ItemsSource = MainViewModel.GuitarModel.GetAllChordDefinitions();
            chordGroups.SelectedIndex = 0;

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


        private void ScaleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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