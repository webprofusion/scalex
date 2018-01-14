using AlphaTab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablaturePage : ContentPage
    {
        private Models.Song _song;
        private Score _score;
        private ScoreServiceManager _scoreService;
        private Track _selectedTrack;
        private int _selectedTrackIndex = 0;
        private int _playbackSpeed = 100;

        public Models.SongListItem SelectedSong { get; set; }

        public TablaturePage()
        {
            InitializeComponent();
            Playback.OnPlay += new PlaybackControl.PlayEventHandler(StartPlayback);
            Playback.OnStop += new PlaybackControl.StopEventHandler(StopPlayback);
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

            _song = await _scoreService.FetchSongDetailsAsync(SelectedSong.ID);

            //load gp score
            _score = await _scoreService.FetchSongScore(_song, true);
            if (_score == null)
            {
                await DisplayAlert("Load failed", "Could not load song. Check internet connection", "OK");
                return;
            }
            var mostPopularTrack = _song.LatestAvailableRevision?.MostPopularTrack?.ID != null ? _song.LatestAvailableRevision.Tracks.FirstOrDefault(t => t.ID == _song.LatestAvailableRevision?.MostPopularTrack?.ID) : null;
            // this.trackPicker.Items.Clear();
            Track selectedTrack = null;

            List<string> tracks = new List<string>();
            int trackIndex = 0;

            foreach (var t in _score.Tracks)
            {
                tracks.Add(t.Name);
                // this.trackPicker.Items.Add(t.Name);
                trackPicker.Items.Add(t.Name);

                if (_song.LatestAvailableRevision?.MostPopularTrack?.ID != null)
                {
                    if (mostPopularTrack != null && t.Name == mostPopularTrack.Title)
                    {
                        selectedTrack = t;
                        _selectedTrackIndex = trackIndex;
                    }
                }
                trackIndex++;
            }

            if (selectedTrack == null) selectedTrack = _score.Tracks[0];

            trackPicker.SelectedIndex = _selectedTrackIndex;

            //SetCurrentTrack(selectedTrack);
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

        private void StartPlayback()
        {
        }

        private void StopPlayback()
        {
        }

        private async Task PlayMidi()
        {
            var audioFileUrl = _song
                .LatestAvailableRevision
                .Tracks[_selectedTrackIndex]
                .TrackAudio.FirstOrDefault(f => f.Speed == _playbackSpeed).MP3File.AttachmentUrl;
            var audio = Plugin.MediaManager.CrossMediaManager.Current.Play(audioFileUrl);

            /*
#if WINDOWS_UWP
                   // generate midi
            MidiFile file = MidiFileGenerator.GenerateMidiFile(_score);
            byte[] bytes;
            using (var ms = new System.IO.MemoryStream())
            {
                var sw = new StreamWrapper(ms);
                file.WriteTo(sw);

                bytes = ms.ToArray();
            }

                            // write temp midi file
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("output.mid");
            System.Diagnostics.Debug.WriteLine(sampleFile.Path);
            await FileIO.WriteBytesAsync(sampleFile, bytes);

#endif
*/
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

        private void AlphaTabControl_RenderFinished(object sender, AlphaTab.Rendering.RenderFinishedEventArgs e)
        {
            //this.LoadingProgress.IsVisible = false;
        }
    }
}