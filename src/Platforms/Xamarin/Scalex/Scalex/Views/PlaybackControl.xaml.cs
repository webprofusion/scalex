using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaybackControl : ContentView
    {
        public delegate void StopEventHandler();

        public delegate void PlayEventHandler();

        public StopEventHandler OnStop;
        public PlayEventHandler OnPlay;

        public PlaybackControl()
        {
            InitializeComponent();
        }

        private void StopButton_Clicked(object sender, System.EventArgs e)
        {
            OnStop?.Invoke();
        }

        private void PlayButton_Clicked(object sender, System.EventArgs e)
        {
            OnPlay?.Invoke();
        }
    }
}