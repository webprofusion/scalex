using AlphaTab.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if !WINDOWS_UWP

using Commons.Music.Midi;

#endif

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablaturePage : ContentPage
    {
        private Score _score;
        private ScoreServiceManager _scoreService;
        private Track _selectedTrack;

        public TablaturePage()
        {
            InitializeComponent();

            _scoreService = new ScoreServiceManager();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await LoadTrack();
            });
        }

        private async Task LoadTrack()
        {
            //get first song from favourites

            var favourites = await _scoreService.FetchSongsMostViewed();
            var song = await _scoreService.FetchSongDetailsAsync(favourites[0].ID);

            //load gp score
            _score = await _scoreService.FetchSongScore(song, true);

            // this.trackPicker.Items.Clear();
            List<string> tracks = new List<string>();
            foreach (var t in _score.Tracks)
            {
                tracks.Add(t.Name);
                // this.trackPicker.Items.Add(t.Name);
                trackPicker.Items.Add(t.Name);
            }

            SetCurrentTrack(_score.Tracks[0]);
        }

        private void SetCurrentTrack(Track t)
        {
            AlphaTabControl.Tracks = new[]
            {
               t
            };

            this.Title = $"{_score.Artist} : {_score.Title} - { t.Name} ";
            this._selectedTrack = t;
        }

        private void trackPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (trackPicker.SelectedIndex != -1)
            {
                SetCurrentTrack(_score.Tracks[trackPicker.SelectedIndex]);

                this.Perform();
            }
        }

        private void Perform()
        {
            if (this._selectedTrack != null)
            {
                foreach (var s in _selectedTrack.Staves)
                {
                    foreach (var b in s.Bars)
                    {
                        foreach (var v in b.Voices)
                        {
                            foreach (var beat in v.Beats)
                            {
                                foreach (var n in beat.Notes)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Fret {n.Fret} String {n.String}");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PlayMidi()
        {
            if (trackPicker.SelectedIndex > -1)
            {
                var track = _score.Tracks[trackPicker.SelectedIndex];
                var midiFile = new AlphaTab.Audio.Model.MidiFile();
                var midiFileHandler = new AlphaTab.Audio.Generator.MidiFileHandler(midiFile);
                var midiGenerator = new AlphaTab.Audio.Generator.MidiFileGenerator(_score, midiFileHandler);
#if !WINDOWS_UWP
                var vt = new VirtualMidiTimeManager();
                var music = new MidiMusic();
                var buffer = AlphaTab.IO.ByteBuffer.Empty();

                midiFile.WriteTo(buffer);
                var bytes = buffer.ReadAll();
                Stream stream = new MemoryStream(bytes);
                var player = new MidiPlayer(MidiMusic.Read(stream), MidiAccessManager.Empty, vt);
                player.PlayAsync();

                vt.AdvanceBy(10000);
                player.PauseAsync();
                player.Dispose();
#endif
            }
        }
    }
}