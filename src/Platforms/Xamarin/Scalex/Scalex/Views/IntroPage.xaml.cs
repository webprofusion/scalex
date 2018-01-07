using Scalex.Helpers;
using System;
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

            ResourceLoader res = new ResourceLoader();
            this.ScalesImage.Source = res.GetSVGImageResource("icon-scales.svg");
            this.ChordsImage.Source = res.GetSVGImageResource("icon-chords.svg");
            this.TablatureImage.Source = res.GetSVGImageResource("icon-tablature.svg");
            this.LessonsImage.Source = res.GetSVGImageResource("icon-lessons.svg");

            this.PerformImage.Source = res.GetSVGImageResource("icon-perform.svg");
            /*if (res.EndsWith("Tablature.png"))
            {
                this.TablatureImage.Source = ImageSource.FromResource(res, assembly);
            }

            if (res.EndsWith("band.png"))
            {
                this.PerformImage.Source = ImageSource.FromResource(res, assembly);
            }

            if (res.EndsWith("speaker-mesh.jpg"))
            {
                this.BackgroundPageImage.Source = ImageSource.FromResource(res, assembly);
                this.BackgroundPageImage.Scale = 1;
            }*/

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
            ChordsImage.GestureRecognizers.Add(tapChordGestureRecognizer);

            // tab image tap
            var tapTabGestureRecognizer = new TapGestureRecognizer();
            tapTabGestureRecognizer.Tapped += (s, e) =>
            {
                Tablature_Clicked(s, e);
            };
            TablatureImage.GestureRecognizers.Add(tapTabGestureRecognizer);

            // perform image tap
            var tapPerformGestureRecognizer = new TapGestureRecognizer();
            tapPerformGestureRecognizer.Tapped += (s, e) =>
            {
                Soundshed_Clicked(s, e);
            };
            PerformImage.GestureRecognizers.Add(tapPerformGestureRecognizer);
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