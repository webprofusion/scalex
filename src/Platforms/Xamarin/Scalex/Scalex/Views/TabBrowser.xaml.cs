using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabBrowser : ContentPage
    {
        public ObservableCollection<Models.SongListItem> Items { get; set; }
        private ScoreServiceManager _scoreService;

        public TabBrowser()
        {
            InitializeComponent();

            _scoreService = new ScoreServiceManager();

            Items = new ObservableCollection<Models.SongListItem>();

            MyListView.ItemsSource = Items;

            //load popular tracks
            Device.BeginInvokeOnMainThread(async () =>
            {
                var list = await _scoreService.FetchSongsMostViewed();
                Items = new ObservableCollection<Models.SongListItem>(list);
                MyListView.ItemsSource = Items;
            });
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            var songItem = (Models.SongListItem)e.Item;

            await Navigation.PushAsync(new TablaturePage(songItem));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}