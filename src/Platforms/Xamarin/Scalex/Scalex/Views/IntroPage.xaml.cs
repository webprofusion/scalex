using System;
using System.Reflection;
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

            //DEBUG
            var assembly = typeof(IntroPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
                if (res.EndsWith("Chords.png"))
                {
                    this.ChordsImage.Source = ImageSource.FromResource(res, assembly);
                }

                if (res.EndsWith("Scales.png"))
                {
                    this.ScalesImage.Source = ImageSource.FromResource(res, assembly);
                }

                if (res.EndsWith("Tablature.png"))
                {
                    this.TablatureImage.Source = ImageSource.FromResource(res, assembly);
                }

                if (res.EndsWith("Perform.png"))
                {
                    this.PerformImage.Source = ImageSource.FromResource(res, assembly);
                }

                if (res.EndsWith("background.png"))
                {
                    this.BackgroundPageImage.Source = ImageSource.FromResource(res, assembly);
                    this.BackgroundPageImage.Scale = 4;
                }
            }

            // scales image tap
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Scales_Clicked(s, e);
            };
            ScalesImage.GestureRecognizers.Add(tapGestureRecognizer);

            // chords image tap
            var tapChordGestureRecognizer = new TapGestureRecognizer();
            tapChordGestureRecognizer.Tapped += (s, e) =>
            {
                Chords_Clicked(s, e);
            };

            // tab image tap
            var tapTabGestureRecognizer = new TapGestureRecognizer();
            tapTabGestureRecognizer.Tapped += (s, e) =>
            {
                Tablature_Clicked(s, e);
            };

            // perform image tap
            var tapPerformGestureRecognizer = new TapGestureRecognizer();
            tapPerformGestureRecognizer.Tapped += (s, e) =>
            {
                Soundshed_Clicked(s, e);
            };
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
            await Navigation.PushAsync(new TabBrowser());
        }

        private async void Lessons_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LessonPage());
        }

        private async void Soundshed_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PlayerPage());
            Device.OpenUri(new Uri("https://soundshed.com?src=app"));
        }
    }
}