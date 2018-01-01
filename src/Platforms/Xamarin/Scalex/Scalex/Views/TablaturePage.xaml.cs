using AlphaTab.Model;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using AlphaTab.Audio.Model;
using AlphaTab.Audio.Generator;

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

        public Models.SongListItem SelectedSong { get; set; }

        public TablaturePage()
        {
            InitializeComponent();
        }

        public TablaturePage(Models.SongListItem song)
        {
            SelectedSong = song;
            InitializeComponent();

            _scoreService = new ScoreServiceManager();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await LoadTrack();

                await LoadTrackMetadata();
            });
        }

        private async Task LoadTrack()
        {
            //`this.LoadingProgress.IsVisible = true;
            //get first song from favourites

            var song = await _scoreService.FetchSongDetailsAsync(SelectedSong.ID);

            //load gp score
            _score = await _scoreService.FetchSongScore(song, true);

            var mostPopularTrack = song.LatestAvailableRevision?.MostPopularTrack?.ID != null ? song.LatestAvailableRevision.Tracks.FirstOrDefault(t => t.ID == song.LatestAvailableRevision?.MostPopularTrack?.ID) : null;
            // this.trackPicker.Items.Clear();
            Track selectedTrack = null;

            List<string> tracks = new List<string>();
            foreach (var t in _score.Tracks)
            {
                tracks.Add(t.Name);
                // this.trackPicker.Items.Add(t.Name);
                trackPicker.Items.Add(t.Name);

                if (song.LatestAvailableRevision?.MostPopularTrack?.ID != null)
                {
                    if (mostPopularTrack != null && t.Name == mostPopularTrack.Title)
                    {
                        selectedTrack = t;
                    }
                }
            }

            if (selectedTrack == null) selectedTrack = _score.Tracks[0];

            SetCurrentTrack(selectedTrack);
        }

        private async Task LoadTrackMetadata()
        {
            var metadata = await TrackMetadataManager.GetTrackMetadata(_score.Artist, _score.Title);
            if (metadata != null)
            {
                foreach (var img in metadata.TrackArtwork)
                {
                    System.Diagnostics.Debug.WriteLine(img.URL);
                }

                AlbumArt.Source = metadata.TrackArtwork.Last().URL;
            }
        }

        private void SetCurrentTrack(Track t)
        {
            AlphaTabControl.Tracks = new[]
            {
               t
            };

            this.Title = $"{_score.Artist} : {_score.Title} - { t.Name} ";
            this._selectedTrack = t;
            trackPicker.SelectedItem = t;
        }

        private void trackPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (trackPicker.SelectedIndex != -1)
            {
                SetCurrentTrack(_score.Tracks[trackPicker.SelectedIndex]);

                //PlayMidi();
                //this.Perform();
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
                // generate midi
                MidiFile file = MidiFileGenerator.GenerateMidiFile(_score);

                // write midi file
                /*

                  var access = MidiAccessManager.Default;
                  var output = access.OpenOutputAsync(access.Outputs.Last().Id).Result;
                  var music = new MidiMusic();
                  var player = new MidiPlayer(music, output);
                  player.EventReceived += (Commons.Music.Midi.MidiEvent e) => {
                      if (e.EventType == Commons.Music.Midi.MidiEvent.Program)
                          Console.WriteLine($"Program changed: Channel:{e.Channel} Instrument:{e.Msb}");
                  };
                  player.PlayAsync();

                  player.Dispose();*/
            }
        }

        private void AlphaTabControl_RenderFinished(object sender, AlphaTab.Rendering.RenderFinishedEventArgs e)
        {
            //this.LoadingProgress.IsVisible = false;
        }
    }
}