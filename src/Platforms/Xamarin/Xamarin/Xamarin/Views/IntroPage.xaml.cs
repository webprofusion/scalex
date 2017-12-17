using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scalex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroPage : ContentPage
    {
        private ImageSource backgroundImg;

        public IntroPage()
        {
            InitializeComponent();

            //DEBUG
            var assembly = typeof(IntroPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
                if (res.EndsWith("Images.guitar.png"))
                {
                    this.LogoImage.Source = ImageSource.FromResource(res, assembly);
                }
            }
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
            //await Navigation.PushAsync(new PlayerPage());
            Device.OpenUri(new Uri("https://soundshed.com?src=app"));
        }
    }
}