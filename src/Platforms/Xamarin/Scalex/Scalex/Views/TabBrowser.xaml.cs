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

            ResultList.ItemsSource = Items;

            //load popular tracks
            Device.BeginInvokeOnMainThread(async () =>
            {
                var list = await _scoreService.FetchSongsMostViewed();
                Items = new ObservableCollection<Models.SongListItem>(list);
                ResultList.ItemsSource = Items;
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

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            //search based on keyword
            Device.BeginInvokeOnMainThread(async () =>
            {
                var list = await _scoreService.FetchSearchResults(SearchKeyword.Text, 100);
                Items = new ObservableCollection<Models.SongListItem>(list);
                ResultList.ItemsSource = Items;
            });
        }
    }
}