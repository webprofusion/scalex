using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroPage : ContentPage
    {
        public IntroPage()
        {
            InitializeComponent();
        }

        private async void Scales_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScalesPage());
        }

        private async void Chords_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChordsPage());
        }

        private async void Tablature_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TablaturePage());
        }

        private async void Soundshed_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayerPage());
        }
    }
}