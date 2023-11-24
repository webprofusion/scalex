﻿using Scalex.Helpers;
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

            this.LessonsImage.Source = res.GetSVGImageResource("icon-lessons.svg");

            this.PerformImage.Source = res.GetSVGImageResource("icon-perform.svg");

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

        private async void Lessons_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LessonPage());
        }

        private async void Soundshed_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PlayerPage());
            Device.OpenUri(new Uri("https://soundshed.com?src=app"));
        }

        private async void Design_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DesignerPage());
        }
    }
}